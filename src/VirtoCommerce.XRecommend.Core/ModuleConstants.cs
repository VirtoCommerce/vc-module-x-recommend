using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.XRecommend.Core;

public static class ModuleConstants
{
    public const int DefaultMaxRecommendations = 5;
    public const int DefaultMaxProducts = 5;

    public static class EventTypes
    {
        public const string Click = "click";
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

            public static SettingDescriptor MinConversionEvensCount { get; } = new()
            {
                Name = "XRecommend.MinConversionEvensCount",
                GroupName = "Recommendations|General",
                ValueType = SettingValueType.Integer,
                IsPublic = true,
                DefaultValue = 1000,
            };

            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return RecommendationsEnabled;
                    yield return MinConversionEvensCount;
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
                yield return General.MinConversionEvensCount;
            }
        }
    }
}
