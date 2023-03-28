﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class IndexModel : PageModel
{
    private readonly DataDbContext _context;

    public IndexModel(DataDbContext context)
    {
        _context = context;
    }

    public IList<Models.Movie> Movies { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Movies != null)
        {
            Movies = await _context.Movies.ToListAsync();
        }
    }
}
