using Gotlandsrussen.Data;
using Gotlandsrussen.Models;
using Gotlandsrussen.Models.DTOs;
using HotelGotlandsrussen.Models.DTOs;
using HotelGotlandsrussenLIBRARY.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotlandsrussen.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<BookingDto>> GetAllFutureBookings()  //Florent
        {
            return await _context.Bookings
                .Where(b => b.FromDate >= DateOnly.FromDateTime(DateTime.Today)
                    && b.IsCancelled == false)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    GuestName = b.Guest.LastName + ", " + b.Guest.FirstName,
                    RoomNames = b.BookingRooms.Select(br => br.Room.Name).ToList(),
                    BookedFromDate = b.FromDate,
                    BookedToDate = b.ToDate,
                    NumberOfAdults = b.NumberOfAdults,
                    NumberOfChildren = b.NumberOfChildren,
                }).ToListAsync();
        }

        public async Task<Booking?> GetById(int id)
        {
            return await _context.Bookings
                .Include(b => b.BookingRooms)
                .ThenInclude(br => br.Room)
                .ThenInclude(r => r.RoomType)
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task Update(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<BookingDto>> GetBookingHistory()   //Margarita
        {
            return await _context.Bookings
                .Where(b => b.FromDate <= DateOnly.FromDateTime(DateTime.Today))
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    GuestName = b.Guest.LastName + ", " + b.Guest.FirstName,
                    RoomNames = b.BookingRooms.Select(br => br.Room.Name).ToList(),
                    BookedFromDate = b.FromDate,
                    BookedToDate = b.ToDate,
                    NumberOfAdults = b.NumberOfAdults,
                    NumberOfChildren = b.NumberOfChildren,
                }).ToListAsync();
        }

        public async Task<Booking?> UpdateBookingAsync(UpdateBookingDto updatedBooking)  //Florent
        {
            var booking = await _context.Bookings
               .Include(b => b.BookingRooms)
                 .ThenInclude(br => br.Room)
                 .ThenInclude(r => r.RoomType)
                 .FirstOrDefaultAsync(b => b.Id == updatedBooking.Id);

            if (booking == null)
                return null;

            var roomIds = booking.BookingRooms.Select(br => br.RoomId).ToList();

            bool hasConflict = await _context.BookingRooms
                 .Include(br => br.Booking)
                 .AnyAsync(br =>
                     roomIds.Contains(br.RoomId) &&
                     br.Booking.Id != booking.Id &&
                     !br.Booking.IsCancelled &&
                     updatedBooking.FromDate < br.Booking.ToDate &&
                     updatedBooking.ToDate > br.Booking.FromDate
                 );

            if (hasConflict)
                throw new InvalidOperationException("Vald tid krockar med en annan bokning.");


            int totalBeds = await _context.Rooms
                .Where(r => roomIds.Contains(r.Id))
                .SumAsync(r => r.RoomType.NumberOfBeds);

            int totalGuests = updatedBooking.NumberOfAdults + updatedBooking.NumberOfChildren;

            if (totalGuests > totalBeds)
                throw new InvalidOperationException("För många gäster för det valda rummets kapacitet.");


            booking.FromDate = updatedBooking.FromDate;
            booking.ToDate = updatedBooking.ToDate;
            booking.NumberOfAdults = updatedBooking.NumberOfAdults;
            booking.NumberOfChildren = updatedBooking.NumberOfChildren;
            booking.Breakfast = updatedBooking.Breakfast;

            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> CreateBooking(int guestId, DateOnly fromDate, DateOnly toDate, int adults, int children, bool breakfast) //margarita 
        {
            //check if guest exists
            var existingGuest = await _context.Guests.AnyAsync(b => b.Id == guestId);

            //check if room is available on the chosen dates
            var availableRoom = await _context.Rooms
              .Where(r => r.RoomType.NumberOfBeds >= (adults + children))
              .Where(r => !_context.BookingRooms
                  .Any(br => br.RoomId == r.Id &&
                     br.Booking.FromDate <= toDate &&
                     br.Booking.ToDate >= fromDate))
              .Select(r => new RoomDto
              {
                  Id = r.Id,
                  RoomName = r.Name,
                  RoomTypeName = r.RoomType.Name,
                  NumberOfBeds = r.RoomType.NumberOfBeds,
                  PricePerNight = r.RoomType.PricePerNight
              })
              .FirstOrDefaultAsync();


            // Find the first available room
            //var availableRoom = await _context.Rooms
            //    .Where(r => r.RoomType.NumberOfBeds >= (adults + children))
            //    .Where(r => !_context.BookingRooms
            //        .Any(br => br.RoomId == r.Id &&
            //                   br.Booking.FromDate <= toDate &&
            //                   br.Booking.ToDate >= fromDate))
            //    .FirstOrDefaultAsync();

            var bookingRooms = availableRoom.Id;

            var newBooking = new Booking
            {
                GuestId = guestId,
                FromDate = fromDate,
                ToDate = toDate,
                NumberOfAdults = adults,
                NumberOfChildren = children,
                Breakfast = breakfast,
                BookingRooms = new List<BookingRoom>
                {
                    new BookingRoom
                    {
                        RoomId = availableRoom.Id
                    }
                }
            };
           
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return newBooking;
        }

    }
}
