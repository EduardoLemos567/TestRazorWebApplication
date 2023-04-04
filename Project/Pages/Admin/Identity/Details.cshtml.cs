﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Identity;

[RequirePermission(Places.Identity, Actions.Read)]
public class DetailsModel : CrudPageModel
{
    public DetailsModel(AdminRules rules, UserManager<Models.Identity> users) : base(rules, users) { }
    public Models.SummaryIdentity Identity { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var user = await rules.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        Identity = Models.SummaryIdentity.FromIdentity(user);
        return Page();
    }
}
