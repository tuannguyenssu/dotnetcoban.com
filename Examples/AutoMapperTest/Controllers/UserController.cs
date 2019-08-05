using AutoMapper;
using AutoMapperTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoMapperTest.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var user = new User()
            {
              FirstName = "Tuan",
              LastName = "Nguyen"  
            };

            var model = _mapper.Map<UserViewModel>(user);
            return View(model);
        }
    }
}