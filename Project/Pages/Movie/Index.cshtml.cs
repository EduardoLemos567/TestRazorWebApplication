using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Movie;

public class IndexModel : PageModel
{
    private readonly Data.DataDbContext db;
    public IndexModel(Data.DataDbContext context) => this.db = context;
    public IList<Models.Movie> Movies { get; set; } = default!;
    public async Task OnGetAsync() => this.Movies = await this.db.Movies.Include(movie => movie.Category).ToListAsync();
}
