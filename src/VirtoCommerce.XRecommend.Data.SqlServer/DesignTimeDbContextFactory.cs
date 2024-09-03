using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.SqlServer;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RecommendDbContext>
{
    public RecommendDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<RecommendDbContext>();
        var connectionString = args.Length != 0 ? args[0] : "Server=(local);User=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseSqlServer(
            connectionString,
            options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

        return new RecommendDbContext(builder.Options);
    }
}
