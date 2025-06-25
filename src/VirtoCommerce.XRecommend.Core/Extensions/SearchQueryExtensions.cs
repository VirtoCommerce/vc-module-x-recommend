using System;
using VirtoCommerce.XRecommend.Core.Models;

namespace VirtoCommerce.XRecommend.Core.Extensions;

public static class SearchQueryExtensions
{
    public static string GetCustomerCacheKey(this ISearchQuery searchQuery)
    {
        ArgumentNullException.ThrowIfNull(searchQuery);

        return $"{searchQuery.UserId}_{searchQuery.OrganizationId}_{searchQuery.StoreId}";
    }
}
