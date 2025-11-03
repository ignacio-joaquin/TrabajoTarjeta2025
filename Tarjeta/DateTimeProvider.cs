using System;

namespace Tarjeta
{
    public static class DateTimeProvider
    {
        private static Func<DateTime> _nowProvider = () => DateTime.Now;

        public static DateTime Now
        {
            get { return _nowProvider(); }
        }

        public static void SetDateTimeProvider(Func<DateTime> provider)
        {
            _nowProvider = provider;
        }

        public static void ResetToDefault()
        {
            _nowProvider = () => DateTime.Now;
        }
    }
}