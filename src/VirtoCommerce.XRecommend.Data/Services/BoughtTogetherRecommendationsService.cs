using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.Services;

public class BoughtTogetherRecommendationsService : IRecommendationsService
{
    private readonly Func<IRecommendRepository> _repositoryFactory;

    public string Model { get; set; } = "bought-together";

    public BoughtTogetherRecommendationsService(Func<IRecommendRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<IList<string>> GetRecommendationsAsync(GetRecommendationsCriteria criteria)
    {
        using var repository = _repositoryFactory();

        return await repository.GetBoughtTogetherProductIdsAsync(criteria);
    }
}
