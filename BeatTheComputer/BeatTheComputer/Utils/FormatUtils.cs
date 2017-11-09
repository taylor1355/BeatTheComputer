using System;

namespace BeatTheComputer.Utils
{
    static class FormatUtils
    {
        public static String humanReadableTime(TimeSpan time)
        {
            if (time.TotalMilliseconds < 10) {
                return time.TotalMilliseconds.ToString("0.000") + " ms";
            } else if (time.TotalMilliseconds < 100) {
                return time.TotalMilliseconds.ToString("0.00") + " ms";
            } else if (time.TotalMilliseconds < 1000) {
                return time.TotalMilliseconds.ToString("0.0") + " ms";
            } else if (time.TotalSeconds < 10) {
                return time.TotalSeconds.ToString("0.00") + " s";
            } else if (time.TotalSeconds < 60) {
                return time.TotalSeconds.ToString("0.0") + " s";
            } else if (time.TotalMinutes < 60) {
                return time.Minutes + " m : " + time.Seconds.ToString("00") + " s";
            } else {
                return time.Hours.ToString("N0") + " hr : " + time.Minutes.ToString("00") + " m : " + time.Seconds.ToString("00") + " s";
            }
        }

        public static String humanReadableNumber(int num)
        {
            return num.ToString("N0");
        }
    }
}
