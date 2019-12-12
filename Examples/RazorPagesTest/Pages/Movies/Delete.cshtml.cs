using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesTest.Models;

namespace RazorPagesTest.Pages.Movies
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Movie Movie { get; set; }

        private static int _selectedIndex;

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

            _selectedIndex = FakeMovieDbContext.Movies.FindIndex(m => m.Guid == id);

            await Task.CompletedTask;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            FakeMovieDbContext.Movies.RemoveAt(_selectedIndex);
            await Task.CompletedTask;

            return RedirectToPage("Index");
        }
    }
}