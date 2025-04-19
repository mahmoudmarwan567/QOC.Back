using System.Globalization;

namespace QOC.Infrastructure.Helpers
{
    public static class CultureHelper
    {
        public static bool IsArabic() =>
            CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";
    }
}
