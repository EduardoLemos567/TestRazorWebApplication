﻿using Microsoft.AspNetCore.Identity;

namespace Project.Models;

public class StaffRole : IdentityRole<int>
{
    public StaffRole() { }
    public StaffRole(string roleName) : base(roleName) { }
}
