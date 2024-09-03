using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.PostgreSql;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RecommendDbContext>
{
    public RecommendDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<RecommendDbContext>();
        var connectionString = args.Any() ? args[0] : "Server=localhost;Username=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

        return new RecommendDbContext(builder.Options);
    }
}
