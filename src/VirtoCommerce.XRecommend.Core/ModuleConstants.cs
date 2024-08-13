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
}
