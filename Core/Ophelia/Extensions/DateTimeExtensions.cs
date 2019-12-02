using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Web;

namespace Ophelia
{
    public static class DateTimeExtensions
    {
        public static DateTime SetTime(this DateTime date, string Time, string format = "dd.MM.yyyy")
        {
            if (!string.IsNullOrEmpty(Time) && Time.IndexOf(":") > -1)
            {
                var TwentyfourTimeFormat = true;
                var Morning = false;
                if (Time.IndexOf("pm", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    TwentyfourTimeFormat = false;
                    Morning = false;
                    Time = Time.Replace("pm", "").Replace("PM", "").Trim();
                }
                else if (Time.IndexOf("am", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    TwentyfourTimeFormat = false;
                    Morning = true;
                    Time = Time.Replace("am", "").Replace("AM", "").Trim();
                }

                var parts = Time.Split(':');
                var hour = Convert.ToInt32(parts[0]);
                var Minute = Convert.ToInt32(parts[1]);
                var second = 0;
                if (parts.Length > 2)
                    second = Convert.ToInt32(parts[2]);

                if (!TwentyfourTimeFormat)
                {
                    if (!Morning)
                        hour += 12;
                }
                if (hour >= 24)
                    hour = 0;
                date = new DateTime(date.Year, date.Month, date.Day, hour, Minute, second);
            }
            return date;
        }

        public static DateTime ConvertAndAddTime(this string datetime, string Time, string format = "dd.MM.yyyy")
        {
            return datetime.ConvertToDate(format).SetTime(Time);
        }

        public static DateTime FormatAndAddTime<TModel>(this TModel entity, Expression<Func<TModel, DateTime>> expression, string Time, string format = "dd.MM.yyyy")
        {
            MemberExpression exp = (MemberExpression)expression.Body;

            var model = expression.Compile().Invoke(entity);
            var RequestData = HttpContext.Current.Request[exp.Member.Name];
            var value = Convert.ToString(HttpContext.Current.Request[exp.Member.Name]).ConvertAndAddTime(Time, format);
            if (value == DateTime.MinValue)
                return model;
            else
                return value;
        }

        public static DateTime FormatAndAddTime<TModel>(this TModel entity, Expression<Func<TModel, DateTime?>> expression, string Time, string format = "dd.MM.yyyy")
        {
            MemberExpression exp = (MemberExpression)expression.Body;

            var model = expression.Compile().Invoke(entity);
            var RequestData = HttpContext.Current.Request[exp.Member.Name];
            var value = Convert.ToString(HttpContext.Current.Request[exp.Member.Name]).ConvertAndAddTime(Time, format);
            if (value == DateTime.MinValue)
                return model.GetValueOrDefault(DateTime.MinValue);
            else
                return value;
        }

        public static DateTime Format<TModel>(this TModel entity, Expression<Func<TModel, DateTime?>> expression, string format = "dd.MM.yyyy")
        {
            MemberExpression exp = (MemberExpression)expression.Body;

            var model = expression.Compile().Invoke(entity);
            var RequestData = HttpContext.Current.Request[exp.Member.Name];
            var value = HttpContext.Current.Request[exp.Member.Name].ConvertToDate(format);
            if (value == DateTime.MinValue)
                return model.GetValueOrDefault(DateTime.MinValue);
            else
                return value;
        }

        public static DateTime Format<TModel>(this TModel entity, Expression<Func<TModel, DateTime>> expression, string format = "dd.MM.yyyy")
        {
            MemberExpression exp = (MemberExpression)expression.Body;

            var model = expression.Compile().Invoke(entity);
            var RequestData = HttpContext.Current.Request[exp.Member.Name];
            var value = HttpContext.Current.Request[exp.Member.Name].ConvertToDate(format);
            if (value == DateTime.MinValue)
                return model;
            else
                return value;
        }

        public static DateTime ConvertToDate(this string dateString, string format = "dd.MM.yyyy")
        {
            if (!string.IsNullOrEmpty(dateString))
            {
                if (dateString.IndexOf("/") > -1)
                    format = "MM/dd/yyyy";

                var value = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture);
                return value;
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Tarih değerinin uygunluğunu kontrol eder ve uygun olmayan değerler için sql server'ın alt sınır tarih değerini döner.
        /// </summary>
        public static DateTime ValidateDate(this DateTime? datetime)
        {
            DateTime minValue = new DateTime(1753, 1, 1);
            return (!datetime.HasValue || datetime.Value <= minValue) ? minValue : datetime.Value;
        }

        /// <summary>
        /// Verilen tarih değerini '23 Ekim 1988 Çarşamba' formatında string ifadeye çevirir. 
        /// </summary>
        public static string ToTrString(this DateTime datetime)
        {
            return datetime.ToString("dd MMMM yyyy, dddd", new CultureInfo("tr-TR"));
        }

        public static string AsFileName(this DateTime datetime, string baseFileName)
        {
            if (string.IsNullOrEmpty(baseFileName)) return datetime.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
            return string.Format("{0}-{1}", baseFileName, datetime.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture));
        }

        public static string AsDirectoryName(this DateTime datetime, string regex = "yyyy-MM-dd")
        {
            return datetime.ToString(regex, CultureInfo.InvariantCulture);
        }

        public static DateTime FirstDateOfWeek(this DateTime datetime, int year, int weekOfYear)
        {
            DateTime firstDay = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - firstDay.DayOfWeek;

            DateTime firstThursday = firstDay.AddDays(daysOffset);
            var calendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
            int firstWeek = calendar.GetWeekOfYear(firstThursday, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNumber = weekOfYear;
            if (firstWeek <= 1) weekNumber -= 1;

            datetime = firstThursday.AddDays(weekNumber * 7);
            return datetime.AddDays(-3);
        }

        public static DateTime Trim(this DateTime datetime, long roundTicks)
        {
            return new DateTime(datetime.Ticks - datetime.Ticks % roundTicks);
        }

        public static DateTime TrimTime(this DateTime datetime)
        {
            return datetime.Trim(TimeSpan.TicksPerDay);
        }

        public static TimeSpan Round(this TimeSpan time, TimeSpan roundingInterval, MidpointRounding roundingType)
        {
            return new TimeSpan(
                Convert.ToInt64(Math.Round(
                    time.Ticks / (decimal)roundingInterval.Ticks,
                    roundingType
                )) * roundingInterval.Ticks
            );
        }

        public static TimeSpan Round(this TimeSpan time, TimeSpan roundingInterval)
        {
            return Round(time, roundingInterval, MidpointRounding.AwayFromZero);
        }

        public static DateTime Round(this DateTime datetime, TimeSpan roundingInterval)
        {
            return new DateTime((datetime - DateTime.MinValue).Round(roundingInterval).Ticks);
        }

        public static DateTime RoundMinute(this DateTime datetime, int accumulator)
        {
            var result = datetime.Round(TimeSpan.FromMinutes(accumulator));
            if (result < datetime)
                result = result.AddMinutes(accumulator);
            return result;
        }

        public static double UnixTicks(this DateTime datetime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1);
            DateTime universal = datetime.ToUniversalTime();
            TimeSpan timespan = new TimeSpan(universal.Ticks - unixStart.Ticks);
            return timespan.TotalMilliseconds;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var EndOfWeek = dt.StartOfWeek(startOfWeek);
            return EndOfWeek.AddDays(7).AddMilliseconds(-1); ;
        }


        /// <summary>
        /// 6: Saturday, 0 Sunday
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="WeekendDays"></param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime dt, string WeekendDays = "6,0")
        {
            return WeekendDays.Contains(((int)dt.DayOfWeek).ToString());
        }

        public static bool IsWithinWorkingHours(this DateTime dt, string StartTime = "08:00", string EndTime = "17:00")
        {
            var startDate = Convert.ToDateTime(dt.ToShortDateString() + " " + StartTime);
            var endDate = Convert.ToDateTime(dt.ToShortDateString() + " " + EndTime);
            return dt >= startDate && dt <= endDate;
        }

        public static bool IsAfterMidnight(this DateTime dt, string MidnightTime = "00:00", string DayLightTime = "05:00")
        {
            var startDate = Convert.ToDateTime(dt.ToShortDateString() + " " + MidnightTime);
            var endDate = Convert.ToDateTime(dt.ToShortDateString() + " " + DayLightTime);
            return dt >= startDate && dt <= endDate;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1).Date;
        }

        public static DateTime EndOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }

        public static DateTime StartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1).Date;
        }

        public static DateTime EndOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 12, 31).Date;
        }

        public static DateTime StartOfDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }

        public static DateTime EndOfDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }

        public static List<DateTime> DaysOfYear(this DateTime dt, bool IsWeekDays = false, bool IsWeekend = false)
        {
            dt = new DateTime(dt.Year, 1, 1).Date; //Ocak
            var dates = Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList();
            dt = dt.AddMonths(1); //Şubat
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Mart
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Nisan
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Mayıs
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Haziran
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Temmuz
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Ağustos
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Eylül
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Ekim
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Kasım
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());
            dt = dt.AddMonths(1); //Aralık
            dates.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(n => new DateTime(dt.Year, dt.Month, n)).ToList());

            var weekends = new List<DateTime>();
            if (IsWeekDays)
            {
                weekends = (from d in dates
                            where d.DayOfWeek == DayOfWeek.Monday || d.DayOfWeek == DayOfWeek.Tuesday || d.DayOfWeek == DayOfWeek.Wednesday || d.DayOfWeek == DayOfWeek.Thursday || d.DayOfWeek == DayOfWeek.Friday
                            select d).ToList();
            }
            else if (IsWeekend)
            {
                weekends = (from d in dates
                            where d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday
                            select d).ToList();
            }
            else
            {
                weekends = dates;
            }

            return weekends;
        }

        public static bool IsDate(this object val)
        {
            if (val == null)
                return false;

            return Microsoft.VisualBasic.Information.IsDate(val);
        }
    }
}
