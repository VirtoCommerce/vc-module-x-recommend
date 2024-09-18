using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Services;
using VirtoCommerce.XRecommend.Core;
using VirtoCommerce.XRecommend.Core.Models;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data.Repositories;


namespace VirtoCommerce.XRecommend.Data.Services;

public class BoughtTogetherRecommendationsService : IRecommendationsService
{
    private readonly Func<IRecommendRepository> _repositoryFactory;
    private readonly IStoreService _storeService;

    public string Model { get; set; } = "bought-together";

    public BoughtTogetherRecommendationsService(Func<IRecommendRepository> repositoryFactory, IStoreService storeService)
    {
        _repositoryFactory = repositoryFactory;
        _storeService = storeService;
    }

    public async Task<IList<string>> GetRecommendationsAsync(GetRecommendationsCriteria criteria)
    {
        var store = await _storeService.GetNoCloneAsync(criteria.StoreId);
        if (store == null)
        {
            return [];
        }

        using var repository = _repositoryFactory();

        return await repository.GetBoughtTogetherProductIdsAsync(criteria, store.Settings.GetValue<int>(ModuleConstants.Settings.General.MinConversionEvensCount));
    }
}
