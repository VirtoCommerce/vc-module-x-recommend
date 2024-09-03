using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.XRecommend.Data.Repositories;

public class RecommendDbContext : DbContextBase
{
    public RecommendDbContext(DbContextOptions<RecommendDbContext> options)
        : base(options)
    {
    }

    protected RecommendDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<XRecommendEntity>().ToTable("XRecommend").HasKey(x => x.Id);
        //modelBuilder.Entity<XRecommendEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

        switch (Database.ProviderName)
        {
            case "Pomelo.EntityFrameworkCore.MySql":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.XRecommend.Data.MySql"));
                break;
            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.XRecommend.Data.PostgreSql"));
                break;
            case "Microsoft.EntityFrameworkCore.SqlServer":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.XRecommend.Data.SqlServer"));
                break;
        }
    }
}
