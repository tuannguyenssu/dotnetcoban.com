using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesTest.Models;

namespace RazorPagesTest.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = FakeMovieDbContext.Movies.FirstOrDefault(m => m.Guid == id);
            if (Movie == null)
            {
                return NotFound();
            }
            await Task.CompletedTask;
            return Page();
        }
    }
}