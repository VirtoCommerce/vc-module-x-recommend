using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.XRecommend.Core;
using VirtoCommerce.XRecommend.Core.Services;
using VirtoCommerce.XRecommend.Data;
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

        serviceCollection.AddTransient<IRecommendService, RecommendService>();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;

        // Register permissions
        var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
        permissionsRegistrar.RegisterPermissions(ModuleInfo.Id, "XRecommend", ModuleConstants.Security.Permissions.AllPermissions);
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}
