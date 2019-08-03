using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HangfireServer.Models;
using Hangfire;

namespace HangfireServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBackgroundJobClient _jobClient;

        public HomeController(IBackgroundJobClient jobClient)
        {
            _jobClient = jobClient;
        }
        public IActionResult Index()
        {
            _jobClient.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
