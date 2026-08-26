using Microsoft.Extensions.Configuration;

namespace Helpdesk.Provider
{
    public static class HelpdeskConfigProvider
    {
        public static IConfiguration Configuration = null!;

        private static string SectionName
        {
            get
            {
#if DEBUG
                return "Settings_Debug";
#else
                return "Settings_Release";
#endif
            }
        }

        private static T GetSectionValue<T>(string keyName)
        {
            return Configuration.GetSection(SectionName).GetValue<T>(keyName);
        }

        public static T SectionValue<T>(this SectionValueKeys sectionValueKeys)
        {
            return GetSectionValue<T>(sectionValueKeys.GetKeyName());
        }

        public static string APIBaseURL
        {
            get
            {
                return SectionValueKeys.ApiUrl.SectionValue<string>();
            }
        }

        public static string WebBaseURL
        {
            get
            {
                return SectionValueKeys.WebUrl.SectionValue<string>();
            }
        }

        public static int TargetId
        {
            get
            {
                return SectionValueKeys.TargetId.SectionValue<int>();
            }
        }

        private static string GetKeyName(this SectionValueKeys secValue)
        {
            switch (secValue)
            {
                case SectionValueKeys.ApiUrl:
                    return "ApiUrl";

                case SectionValueKeys.WebUrl:
                    return "WebUrl";

                case SectionValueKeys.TargetId:
                    return "tId";

                default:
                    throw new Exception("Enum not defined");
            }
        }
    }

    public enum SectionValueKeys
    {
        ApiUrl,
        WebUrl,
        TargetId
    }
}