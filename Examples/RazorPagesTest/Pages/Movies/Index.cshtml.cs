using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesTest.Models;

namespace RazorPagesTest.Pages.Movies
{
    public class IndexModel : PageModel
    {
        public IList<Movie> Movie { get; set; }
        public void OnGet()
        {
            Movie = FakeMovieDbContext.Movies.ToList();
        }
    }
}