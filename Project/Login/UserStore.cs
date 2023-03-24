﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Login;

public class UserStore<TUser>
    : UserStore<TUser, StaffRole, DataContext, int>
    where TUser : AAccount
{
    public UserStore(DataContext context,
                     IdentityErrorDescriber? describer = null)
        : base(context, describer) { }
}
