﻿using Microsoft.EntityFrameworkCore;

namespace Project.Data;

public class SauceContext : DbContext
{
    public SauceContext(DbContextOptions<SauceContext> options, ILogger<SauceContext> a)
        : base(options)
    { }
    public DbSet<Models.Movie> Movies { get; set; } = default!;
    public DbSet<Models.MovieCategory> MovieCategories { get; set; } = default!;
    public DbSet<Models.UserAccount> UserAccounts { get; set; } = default!;
    public DbSet<Models.StaffAccount> StaffAccounts { get; set; } = default!;
    public DbSet<Models.PermissionsPackage> PermissionsPackages { get; set; } = default!;
    protected override void ConfigureConventions(ModelConfigurationBuilder builder) => 
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
}
