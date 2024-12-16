using Auth.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

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
        builder.Entity<User>(u => u.ToTable("Users"));
        builder.Entity<Role>(r => { r.ToTable(name: "Roles"); });
        builder.Entity<UserRole>(ur => { ur.ToTable("UserRoles"); });
        builder.Entity<IdentityUserClaim<int>>(uc => { uc.ToTable("UserClaims"); });
        builder.Entity<IdentityUserLogin<int>>(ul => { ul.ToTable("UserLogins"); });
        builder.Entity<IdentityUserToken<int>>(ut => { ut.ToTable("UserTokens"); });
        builder.Entity<IdentityRoleClaim<int>>(rc => { rc.ToTable("RoleClaims"); });

    }

    private void ApplyTPTApproach(ModelBuilder builder)
    {
        builder.Entity<Admin>().ToTable("Admins");
        builder.Entity<Seller>().ToTable("Sellers");
        builder.Entity<Customer>().ToTable("Customers");
    }

}
