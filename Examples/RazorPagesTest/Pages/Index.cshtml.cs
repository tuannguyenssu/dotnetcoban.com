using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorPagesTest.Models;

namespace RazorPagesTest.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<Product> Products { get; set; } = new List<Product>();
        public Product FeaturedProduct { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Products = FakeBakeryDbContext.Products;
            FeaturedProduct = Products.ElementAt(new Random().Next(Products.Count));
        }
    }
}
