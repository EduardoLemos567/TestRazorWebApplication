﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

public class DeleteModel : PageModel
{
    private readonly DataDbContext _context;

    public DeleteModel(DataDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Models.MovieCategory MovieCategory { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.MovieCategories == null)
        {
            return NotFound();
        }

        var moviecategory = await _context.MovieCategories.FirstOrDefaultAsync(m => m.Id == id);

        if (moviecategory == null)
        {
            return NotFound();
        }
        else
        {
            MovieCategory = moviecategory;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.MovieCategories == null)
        {
            return NotFound();
        }
        var moviecategory = await _context.MovieCategories.FindAsync(id);

        if (moviecategory != null)
        {
            MovieCategory = moviecategory;
            _context.MovieCategories.Remove(MovieCategory);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
