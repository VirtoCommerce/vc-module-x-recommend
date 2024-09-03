using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.MySql;
using VirtoCommerce.Platform.Data.PostgreSql;
using VirtoCommerce.Platform.Data.SqlServer;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.XRecommend.Core;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data;
using VirtoCommerce.XRecommend.Data.Repositories;
using VirtoCommerce.XRecommend.Data.Services;

namespace VirtoCommerce.XRecommend.Web;

public class Module : IModule, IHasConfiguration
{
    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        var graphQLBuilder = new CustomGraphQLBuilder(serviceCollection);
        graphQLBuilder.AddSchema(typeof(CoreAssemblyMarker), typeof(DataAssemblyMarker));

        serviceCollection.AddDbContext<RecommendDbContext>(options =>
        {
            var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
            var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ?? Configuration.GetConnectionString("VirtoCommerce");

            switch (databaseProvider)
            {
                case "MySql":
                    options.UseMySqlDatabase(connectionString);
                    break;
                case "PostgreSql":
                    options.UsePostgreSqlDatabase(connectionString);
                    break;
                default:
                    options.UseSqlServerDatabase(connectionString);
                    break;
            }
        });

        serviceCollection.AddTransient<IRecommendRepository, RecommendRepository>();
        serviceCollection.AddTransient<Func<IRecommendRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IRecommendRepository>());

        //serviceCollection.AddTransient<IRecommendService, RecommendService>();
        //serviceCollection.AddTransient<IRecommendSearchService, RecommendServiceSearchService>();

        serviceCollection.AddTransient<IRecommendationsService, RelatedProductsRecommendationsService>();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;

        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
        settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.StoreLevelSettings, nameof(Store));
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}
