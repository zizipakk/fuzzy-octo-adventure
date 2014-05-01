using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace Tax.MvcGlobalisationSupport
{
    public static class CultureManager
    {
        public const string CultureNameEnglish = "en";
        public const string CultureNameHungarian = "hu";
        public const string CultureNameGerman = "de";

        public static Dictionary<string, System.Guid> CultureToID { get; set; }
        static Dictionary<string, CultureInfo> SupportedCultures { get; set; }

        public static string CurrentCultureTwoLetter()
        { 
            return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }

        public static string CultureClass(string code=null)
        {
            if (String.IsNullOrEmpty(code))
            {
                code = CurrentCultureTwoLetter();
            }
            return string.Format("flag-{0}", code);
        }

        public static string CultureName(string code = null)
        {
            if (String.IsNullOrEmpty(code))
            {
                code = CurrentCultureTwoLetter();
            }
            switch (code)
            {
                case CultureNameEnglish:
                    return "English";
                case CultureNameHungarian:
                    return "Magyar";
                case CultureNameGerman:
                    return "Deutsch";
                default:
                    return "";
            }
        }

        public static string CultureShortname(string code = null)
        {
            if (String.IsNullOrEmpty(code))
            {
                code = CurrentCultureTwoLetter();
            }
            switch (code)
            {
                case CultureNameEnglish:
                    return "EN";
                case CultureNameHungarian:
                    return "HU";
                case CultureNameGerman:
                    return "DE";
                default:
                    return "";
            }
        }

        static CultureInfo DefaultCulture
        {
            get
            {
                return SupportedCultures[CultureNameHungarian];
            }
        }

        static void AddSupportedCulture(string name)
        {
            SupportedCultures.Add(name, CultureInfo.CreateSpecificCulture(name));
        }

        static void InitializeSupportedCultures()
        {
            SupportedCultures = new Dictionary<string, CultureInfo>();
            AddSupportedCulture(CultureNameHungarian);
            AddSupportedCulture(CultureNameEnglish);
            AddSupportedCulture(CultureNameGerman);
        }

        static string ConvertToShortForm(string code)
        {
            return code.Substring(0, 2);
        }

        static bool CultureIsSupported(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;
            code = code.ToLowerInvariant();
            if (code.Length == 2)
                return SupportedCultures.ContainsKey(code);
            return CultureFormatChecker.FormattedAsCulture(code) && SupportedCultures.ContainsKey(ConvertToShortForm(code));
        }

        static CultureInfo GetCulture(string code)
        {
            if (!CultureIsSupported(code))
                return DefaultCulture;
            string shortForm = ConvertToShortForm(code).ToLowerInvariant(); ;
            return SupportedCultures[shortForm];
        }

        public static void SetCulture(string code)
        {
            CultureInfo cultureInfo = GetCulture(code);
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }

        static CultureManager()
        {
            InitializeSupportedCultures();
            CultureToID = new Dictionary<string, System.Guid>();
            CultureToID.Add(CultureNameHungarian, new Guid("0E64694A-EC2D-4A78-AFFB-4C734E7E38A6"));
            CultureToID.Add(CultureNameEnglish, new Guid("BE5DB246-2061-4C42-B1D5-467EF08AE158"));
            CultureToID.Add(CultureNameGerman, new Guid("6EFE193D-EFC8-46FB-B588-07CDEE45DD70"));
        }
    }
}
