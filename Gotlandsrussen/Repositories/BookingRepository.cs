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

        public async Task CreateBooking(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        //public async Task<Booking> CreateBooking(int guestId, List<int> roomId, DateOnly fromDate, DateOnly toDate, int adults, int children, bool breakfast) //margarita 
        //{
        //    var newBooking = new CreateBookingDto
        //    {
        //        GuestId = guestId,
        //        FromDate = fromDate,
        //        ToDate = toDate,
        //        NumberOfAdults = adults,
        //        NumberOfChildren = children,
        //        Breakfast = breakfast,
        //        Rooms = new List<RoomDto>
        //        {
        //            //new RoomDto
        //            //{
        //            //    Id = availableRoom.Id,
        //            //    RoomName = availableRoom.RoomName,
        //            //    RoomTypeName = availableRoom.RoomTypeName,
        //            //    NumberOfBeds = availableRoom.NumberOfBeds,
        //            //    PricePerNight = availableRoom.PricePerNight
        //            //}
        //        }
             
        //    };

        //    var addBooking = new Booking
        //    {
        //        GuestId = guestId,
        //        FromDate = fromDate,
        //        ToDate = toDate,
        //        NumberOfAdults = adults,
        //        NumberOfChildren = children,
        //        Breakfast = breakfast,
        //        BookingRooms = new List<BookingRoom>
        //        {
        //             new BookingRoom
        //             {
        //                 BookingId = newBooking.BookingId,
        //                 RoomId= newBooking.RoomId
        //             }
        //        }
        //    };

        //    //var bookingRooms = new BookingRoom
        //    //{
        //    //    BookingId = newBooking.BookingId,
        //    //    RoomId = newBooking.RoomId
        //    //};

        //    _context.Bookings.Add(addBooking);
        //    //_context.BookingRooms.Add(bookingRooms);
        //    //await _context.SaveChangesAsync();

        //    return addBooking;

        //}

    }
}
