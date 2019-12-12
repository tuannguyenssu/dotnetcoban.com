using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesTest.Models;

namespace RazorPagesTest.Pages
{
    public class OrderModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Product Product { get; set; }

        [BindProperty, EmailAddress, Required, Display(Name = "Your Email Address")]
        public string OrderEmail { get; set; }
        [BindProperty, Required(ErrorMessage = "Please supply a shipping address"), Display(Name = "Shipping Address")]
        public string OrderShipping { get; set; }
        [BindProperty, Display(Name = "Quantity")]
        public int OrderQuantity { get; set; } = 1;
        public void OnGet()
        {
            Product = FakeBakeryDbContext.Products.Find(p => p.Id == Id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Product = FakeBakeryDbContext.Products.Find(p => p.Id == Id);
            await Task.CompletedTask;
            if (ModelState.IsValid)
            {
                return RedirectToPage("OrderSuccess");
            }
            return Page();
        }
    }
}