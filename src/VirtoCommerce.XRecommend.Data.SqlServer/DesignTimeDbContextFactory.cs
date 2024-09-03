using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.XRecommend.Data.Repositories;

namespace VirtoCommerce.XRecommend.Data.SqlServer;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<XRecommendDbContext>
{
    public XRecommendDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<XRecommendDbContext>();
        var connectionString = args.Length != 0 ? args[0] : "Server=(local);User=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseSqlServer(
            connectionString,
            options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

        return new XRecommendDbContext(builder.Options);
    }
}
