using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesTest.Models;

namespace RazorPagesTest.Pages.Movies
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Movie Movie { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Movie.Guid = Guid.NewGuid().ToString();
            FakeMovieDbContext.Movies.Add(Movie);
            await Task.CompletedTask;
            return RedirectToPage("Index");
        }
    }
}