﻿using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.Movie;

[RequirePermission(Places.Movie, Actions.Read)]
public class DetailsModel : CrudPageModel
{
    public DetailsModel(DataDbContext db) : base(db) { }
    public Models.Movie Movie { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var movie = await this.TryFindMovieAsync(id);
        if (movie is null) { return NotFound(); }
        Movie = movie;
        return Page();
    }
}
