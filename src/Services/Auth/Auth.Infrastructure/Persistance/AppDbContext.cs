using Auth.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistance;
public class AppDbContext : IdentityDbContext<User,
                                                Role,
                                                int,
                                                IdentityUserClaim<int>,
                                                UserRole,
                                                IdentityUserLogin<int>,
                                                IdentityRoleClaim<int>,
                                                IdentityUserToken<int>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    public DbSet<Admin> Admins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureRelationships(builder);
        ChangeDefaultIdentityTableNames(builder);
        ApplyTPTApproach(builder);
    }

    private static void ConfigureRelationships(ModelBuilder builder)
    {
        /// [User] * <have> * [Role]
        builder.Entity<UserRole>()
          .HasKey(ur => new { ur.UserId, ur.RoleId });

        builder
            .Entity<User>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder
            .Entity<Role>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
    }

    private void ChangeDefaultIdentityTableNames(ModelBuilder builder)
    {
        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");
        builder.Entity<UserRole>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");

    }

    private void ApplyTPTApproach(ModelBuilder builder)
    {
        builder.Entity<Admin>().ToTable("Admins").HasBaseType<User>(); 
        builder.Entity<Seller>().ToTable("Sellers").HasBaseType<User>();
        builder.Entity<Customer>().ToTable("Customers").HasBaseType<User>();
    }

}
