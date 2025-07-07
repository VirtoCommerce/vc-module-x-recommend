using System;
using GraphQL.MicrosoftDI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.MySql.Extensions;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;
using VirtoCommerce.Platform.Data.SqlServer.Extensions;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.XRecommend.Core;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data;
using VirtoCommerce.XRecommend.Data.Authorization;
using VirtoCommerce.XRecommend.Data.MySql;
using VirtoCommerce.XRecommend.Data.PostgreSql;
using VirtoCommerce.XRecommend.Data.Repositories;
using VirtoCommerce.XRecommend.Data.Services;
using VirtoCommerce.XRecommend.Data.SqlServer;

namespace VirtoCommerce.XRecommend.Web;

public class Module : IModule, IHasConfiguration
{
    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        _ = new GraphQLBuilder(serviceCollection, builder =>
        {
            builder.AddSchema(serviceCollection, typeof(CoreAssemblyMarker), typeof(DataAssemblyMarker));
        });

        serviceCollection.AddDbContext<XRecommendDbContext>(options =>
        {
            var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
            var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ?? Configuration.GetConnectionString("VirtoCommerce");

            switch (databaseProvider)
            {
                case "MySql":
                    options.UseMySqlDatabase(connectionString, typeof(MySqlDataAssemblyMarker), Configuration);
                    break;
                case "PostgreSql":
                    options.UsePostgreSqlDatabase(connectionString, typeof(PostgreSqlDataAssemblyMarker), Configuration);
                    break;
                default:
                    options.UseSqlServerDatabase(connectionString, typeof(SqlServerDataAssemblyMarker), Configuration);
                    break;
            }
        });

        serviceCollection.AddTransient<IRecommendRepository, RecommendRepository>();
        serviceCollection.AddTransient<Func<IRecommendRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IRecommendRepository>());

        serviceCollection.AddTransient<IHistoricalEventService, HistoricalEventService>();
        serviceCollection.AddTransient<IHistoricalEventSearchService, HistoricalEventSearchService>();
        serviceCollection.AddTransient<IRecommendationsService, RelatedProductsRecommendationsService>();
        serviceCollection.AddTransient<IRecommendationsService, BoughtTogetherRecommendationsService>();
        serviceCollection.AddSingleton<IAuthorizationHandler, RecommendationsAuthorizationHandler>();

        serviceCollection.AddTransient<ISearchQueryService, SearchQueryService>();
        serviceCollection.AddTransient<ISearchQuerySearchService, SearchQuerySearchService>();
        serviceCollection.AddSingleton<IAuthorizationHandler, SearchHistoryAuthorizationHandler>();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;

        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
        settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.StoreLevelSettings, nameof(Store));

        // Apply migrations
        using var serviceScope = serviceProvider.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<XRecommendDbContext>();
        dbContext.Database.Migrate();
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}
