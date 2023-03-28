﻿namespace Project.Authorization;

public enum Places : short
{
    NoPlace,
    Movie,
    MovieCategory,
    Identity,
    Role,
    Permission,
    // Max 8191 options = 13 bits
}

public enum Actions : short
{
    NoAction,
    Create,
    Read,   // Read individual details
    Update,
    Delete,
    List,   // List page
    // Max 8 options = 3 bits
}

public enum DefaultRoles
{
    User,
    Staff,
    Admin,
}