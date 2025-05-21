using System.Globalization;

namespace Gotlandsrussen.Models.DTOs
{
    public class YearWeekBookingsDto
    {
        public int Year { get; set; }
        public int Week { get; set; }
        public List<BookingDto>? Bookings { get; set; }
    }



    // OBS - denna ska ligga någon annanstans. Skapa mapp. Var?
    public static class DateOnlyExtensions
    {
        public static (int Year, int Week) GetIsoYearAndWeek(this DateOnly date)
        {
            var dateTime = date.ToDateTime(TimeOnly.MinValue);
            var culture = CultureInfo.GetCultureInfo("sv-SE");
            var calendar = culture.Calendar;
            var weekRule = culture.DateTimeFormat.CalendarWeekRule;
            var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

            int week = calendar.GetWeekOfYear(dateTime, weekRule, firstDayOfWeek);
            int year = ISOWeek.GetYear(dateTime);

            return (year, week);
        }
    }

}
