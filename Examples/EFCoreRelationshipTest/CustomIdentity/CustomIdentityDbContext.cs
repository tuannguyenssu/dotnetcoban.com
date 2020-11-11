using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EFCoreRelationshipTest.CustomIdentity
{
    //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model
    public class CustomIdentityUser : IdentityUser
    {
        public virtual ICollection<CustomIdentityUserClaim> Claims { get; set; }
        public virtual ICollection<CustomIdentityUserLogin> Logins { get; set; }
        public virtual ICollection<CustomIdentityUserToken> Tokens { get; set; }
        public virtual ICollection<CustomIdentityUserRole> UserRoles { get; set; }
    }

    public class CustomIdentityRole : IdentityRole
    {
        public virtual ICollection<CustomIdentityUserRole> UserRoles { get; set; }
        public virtual ICollection<CustomIdentityRoleClaim> RoleClaims { get; set; }
    }

    public class CustomIdentityUserRole : IdentityUserRole<string>
    {
        public virtual CustomIdentityUser User { get; set; }
        public virtual CustomIdentityRole Role { get; set; }
    }

    public class CustomIdentityUserClaim : IdentityUserClaim<string>
    {
        public virtual CustomIdentityUser User { get; set; }
    }

    public class CustomIdentityUserLogin : IdentityUserLogin<string>
    {
        public virtual CustomIdentityUser User { get; set; }
    }

    public class CustomIdentityRoleClaim : IdentityRoleClaim<string>
    {
        public virtual CustomIdentityRole Role { get; set; }
    }

    public class CustomIdentityUserToken : IdentityUserToken<string>
    {
        public virtual CustomIdentityUser User { get; set; }
    }

    public class CustomIdentityDbContext : DbContext
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<CustomIdentityRole> Roles { get; set; }
        public DbSet<CustomIdentityRoleClaim> RoleClaims { get; set; }

        public DbSet<CustomIdentityUser> Users { get; set; }
        public DbSet<CustomIdentityUserToken> UserTokens { get; set; }
        public DbSet<CustomIdentityUserClaim> UserClaims { get; set; }
        public DbSet<CustomIdentityUserLogin> UserLogins { get; set; }

        public DbSet<CustomIdentityUserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CustomIdentityUser>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
                b.ToTable("Users");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
                //b.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                //b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                //b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                //b.HasMany(u => u.Tokens).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<CustomIdentityRole>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex");
                b.ToTable("Roles");
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

            });

            builder.Entity<CustomIdentityUserClaim>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("UserClaims");
            });

            builder.Entity<CustomIdentityRoleClaim>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("RoleClaims");
            });

            builder.Entity<CustomIdentityUserRole>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId });
                b.ToTable("UserRoles");
            });

            builder.Entity<CustomIdentityUserLogin>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
                b.ToTable("UserLogins");
            });

            builder.Entity<CustomIdentityUserToken>(b =>
            {
                b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });
                b.ToTable("UserTokens");
            });
        }
    }
}
