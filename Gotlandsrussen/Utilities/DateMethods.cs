using System.Globalization;

namespace Gotlandsrussen.Utilities
{
    public static class DateMethods
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
