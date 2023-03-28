﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.Role;

public class EditModel : PageModel
{
    private readonly DataDbContext _context;

    public EditModel(DataDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Models.Role Role { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Roles == null)
        {
            return NotFound();
        }

        var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
        if (role == null)
        {
            return NotFound();
        }
        Role = role;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Role).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoleExists(Role.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool RoleExists(int id)
    {
        return (_context.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
