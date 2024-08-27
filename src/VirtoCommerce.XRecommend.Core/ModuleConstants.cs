using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.XRecommend.Core;

public static class ModuleConstants
{
    public static class Security
    {
        public static class Permissions
        {
            public const string Create = "XRecommend:create";
            public const string Read = "XRecommend:read";
            public const string Update = "XRecommend:update";
            public const string Delete = "XRecommend:delete";

            public static string[] AllPermissions { get; } =
            {
                Create,
                Read,
                Update,
                Delete,
            };
        }
    }

    public static class Settings
    {
        public static class General
        {
            public static SettingDescriptor RecommendationsEnabled { get; } = new()
            {
                Name = "XRecommend.RecommendationsEnabled",
                GroupName = "Recommendations|General",
                ValueType = SettingValueType.Boolean,
                IsPublic = true,
                DefaultValue = true,
            };


            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return RecommendationsEnabled;
                }
            }
        }

        public static IEnumerable<SettingDescriptor> AllSettings
        {
            get
            {
                return General.AllGeneralSettings;
            }
        }

        public static IEnumerable<SettingDescriptor> StoreLevelSettings
        {
            get
            {
                yield return General.RecommendationsEnabled;
            }
        }
    }

}
