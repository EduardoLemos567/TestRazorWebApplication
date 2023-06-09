﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;
using System.ComponentModel.DataAnnotations;

namespace Project.Pages.Admin.Identity;

[RequirePermission(Places.Identity, Actions.Update)]
public class EditModel : CrudPageModel
{
    public EditModel(AdminRules rules, UserManager<Models.Identity> users) : base(rules, users) { }
    [BindProperty]
    public Models.SummaryIdentity Identity { get; set; } = default!;
    [BindProperty, DataType(DataType.Password), Display(Name = "New password")]
    public string? NewPassword { get; set; }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var user = await rules.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        Identity = Models.SummaryIdentity.FromIdentity(user);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        // Find user
        var user = await rules.TryFindUserAsync(Identity.Id);
        if (user is null) { return NotFound(); }
        // Change password
        if (!string.IsNullOrEmpty(NewPassword))
        {
            var token = await users.GeneratePasswordResetTokenAsync(user);
            var resetResult = await users.ResetPasswordAsync(user, token, NewPassword);
            if (!resetResult.Succeeded)
            {
                if (this.CheckPasswordErrors(resetResult, nameof(NewPassword))) { return Page(); }
                return Content("User update failed");
            }
        }
        // Update other details
        Identity.Update(user);
        var updateResult = await users.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Content("User update failed");
        }
        return RedirectToPage("./Index");
    }
}
