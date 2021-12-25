using DNTPersianUtils.Core;

namespace Datiss.Budget.Extensions
{

    public static class PersianExtensions
    {

        public static string CorrectYeKe(this string s)
            => s?.ApplyCorrectYeKe();
    }
}
