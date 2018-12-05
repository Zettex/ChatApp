using System;
using System.Configuration;

namespace ChatApp
{
    public static class Extensions
    {
        static TimeZoneInfo TZInfo = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["Timezone"]);

        /// <summary>
        /// Convert the passed datetime from UTC timezone to configured timezone in web.config.
        /// </summary>
        /// <param name="utcDT"></param>
        /// <returns></returns>
        public static string ToConfigLocalDate(this DateTime utcDT)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDT, TZInfo).ToString("dd.MM.yyyy");
        }

        public static string ToConfigLocalTime(this DateTime utcDT)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDT, TZInfo).ToShortTimeString();
        }
    }
}