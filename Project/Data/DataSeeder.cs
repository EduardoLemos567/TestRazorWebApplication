﻿using Microsoft.AspNetCore.Identity;
using Project.Models;

namespace Project.Data;

public class DataSeeder
{
    private readonly SauceContext db;
    private readonly Random rng;
    public DataSeeder(SauceContext db)
    {
        this.db = db;
        this.rng = new Random(1234);
    }
    public void SeedAllModels()
    {
        this.SeedMovieCategories();
        this.SeedMovies();
        this.SeedUserAccounts();
        this.SeedStaffRoles();
        this.SeedStaffAccounts();
    }
    public void SeedMovieCategories()
    {
        if (this.db.MovieCategories.Any()) { return; }
        foreach (var i in Enumerable.Range(0, 10))
        {
            _ = this.db.Add(new MovieCategory()
            {
                Name = $"MovieCategory {i + 1}"
            });
        }
        _ = this.db.SaveChanges();
    }
    public void SeedMovies()
    {
        if (this.db.Movies.Any()) { return; }
        var categories = this.db.MovieCategories.ToList();
        foreach (var i in Enumerable.Range(0, 20))
        {
            _ = this.db.Add(new Movie()
            {
                Name = $"Movie {i + 1}",
                ReleaseDate = new(this.rng.Next(1900, 2024), this.rng.Next(1, 13), this.rng.Next(1, 31)),
                Category = categories[this.rng.Next(categories.Count)],
            });
        }
        _ = this.db.SaveChanges();
    }
    public void SeedUserAccounts()
    {
        if (this.db.UserAccounts.Any()) { return; }
        foreach (var i in Enumerable.Range(0, 20))
        {
            var user = new UserAccount();
            SeedAccount(user, i);
            this.db.Add(user);
        }
        this.db.SaveChanges();
    }
    public void SeedStaffRoles()
    {
        if (this.db.PermissionsPackages.Any()) { return; }
        foreach (var i in Enumerable.Range(0, 10))
        {
            _ = this.db.Add(new StaffRole()
            {
                Name = $"StaffRole {i + 1}",
                Powers = (uint)this.rng.Next(0, 1000)
            });
        }
        _ = this.db.SaveChanges();
    }
    public void SeedStaffAccounts()
    {
        if (this.db.StaffAccounts.Any()) { return; }
        var staffRoles = this.db.PermissionsPackages.ToList();
        foreach (var i in Enumerable.Range(0, 20))
        {
            var staff = new StaffAccount();
            SeedAccount(staff, i);
            staff.Permissions = staffRoles[this.rng.Next(staffRoles.Count)];
            this.db.Add(staff);
        }
        this.db.SaveChanges();
    }
    private static void SeedAccount(in Account account, int index)
    {
        account.UserName = $"UserName {index + 1}";
        account.RealName = $"RealName {index + 1}";
        account.Email = $"email{index + 1}@email.com";
    }
}