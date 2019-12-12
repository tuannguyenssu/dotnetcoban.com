using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorPagesTest.Models;

namespace RazorPagesTest
{
    public class FakeMovieDbContext
    {
        public static List<Movie> Movies = new List<Movie>()
        {
            new Movie
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "When Harry Met Sally",
                ReleaseDate = DateTime.Parse("1989-2-12"),
                Genre = "Romantic Comedy",
                Price = 7.99M
            },

            new Movie
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "Ghostbusters ",
                ReleaseDate = DateTime.Parse("1984-3-13"),
                Genre = "Comedy",
                Price = 8.99M
            },

            new Movie
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "Ghostbusters 2",
                ReleaseDate = DateTime.Parse("1986-2-23"),
                Genre = "Comedy",
                Price = 9.99M
            },

            new Movie
            {
                Guid = Guid.NewGuid().ToString(),
                Title = "Rio Bravo",
                ReleaseDate = DateTime.Parse("1959-4-15"),
                Genre = "Western",
                Price = 3.99M
            }
        };
    }
}
