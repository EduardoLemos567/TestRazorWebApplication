﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public abstract class Account : IdentityUser
{
    [Required]
    public string? RealName { get; set; }
}