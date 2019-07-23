using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookSys.BLL.Contacts;
using BookSys.BLL.Services;
using BookSys.VeiwModel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService<UserVM, string> userService;
        public UserController(UserService _userService)
        {
            userService = _userService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseVM>> Register ([FromBody] UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var res = await userService.Register(userVM);
                return Ok(res);
            }
            else
                return BadRequest("Something went wrong");
        }
    }
}