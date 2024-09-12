using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.PostgreSql;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<XRecommendDbContext>
{
    public XRecommendDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<XRecommendDbContext>();
        var connectionString = args.Length != 0 ? args[0] : "Server=localhost;Username=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

        return new XRecommendDbContext(builder.Options);
    }
}
