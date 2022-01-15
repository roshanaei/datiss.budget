using System;
using DNTPersianUtils.Core;

namespace Datiss.Budget.Common
{

    public interface IDateService
    {
        DateTime UtcNow { get; }

        DateTime Now { get; }

        string NowShortShamsiDate { get; }

        string NowLongShamsiDate { get; }

        string NowShortShamsiDateTime { get; }

        string NowLongShamsiDateTime { get; }

        string NowFirendlyShamsiDate { get; }

        string NowFriendlyShamsiDateTime { get; }
    }

    public class DateService : IDateService
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;

        public string NowShortShamsiDate
            => Now.ToShortPersianDateString();

        public string NowLongShamsiDate
            => Now.ToLongPersianDateString();

        public string NowShortShamsiDateTime
            => Now.ToShortPersianDateTimeString();

        public string NowLongShamsiDateTime
            => Now.ToLongPersianDateTimeString();

        public string NowFirendlyShamsiDate
            => Now.ToFriendlyPersianDateTextify(appendHhMm: false);

        public string NowFriendlyShamsiDateTime
            => Now.ToFriendlyPersianDateTextify(appendHhMm: true);
    }
}
