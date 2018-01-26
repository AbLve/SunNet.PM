
using System;
namespace SF.Framework.Mvc.Search
{
    public class UnixTime
    {
        private static DateTime _baseTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// Will unixtime conversion for.net DateTime
        /// </summary>
        /// <param name="timeStamp">Number of seconds</param>
        /// <returns>After the conversion of the time</returns>
        public static DateTime FromUnixTime(long timeStamp)
        {
            return new DateTime((timeStamp + 8 * 60 * 60) * 10000000 + _baseTime.Ticks);
            //return BaseTime.AddSeconds(timeStamp);
            //return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timeStamp);
        }

        /// <summary>
        /// Will .net DateTime conversion for Unix time
        /// </summary>
        /// <param name="dateTime">To convert time</param>
        /// <returns>After the conversion of the time</returns>
        public static long FromDateTime(DateTime dateTime)
        {
            return (dateTime.Ticks - _baseTime.Ticks) / 10000000 - 8 * 60 * 60;
            //return (dateTime.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks) / 10000000;
        }
    }
}
