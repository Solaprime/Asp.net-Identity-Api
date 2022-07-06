using Asp.netIdentityApi.Services;
using Asp.netShared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private UserManager<IdentityUser> _userManager;
        public AuthController(IUserService userService, UserManager<IdentityUser> userManager )
        {
            _userService = userService;
            _userManager = userManager;

        }
        // /api/Auth/
        [HttpPost("Register")]
        public async  Task<IActionResult> RegisterAsync([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var emailExist = new IdentityUser();
                var emailExist = await _userManager.FindByEmailAsync(model.Email);
                if (emailExist != null)
                {
                    return BadRequest("User already Exist");
                }
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Some properrtied are InvALID");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // we login withn the Model
                var result = await _userService.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Some properrtied are not  InvALID");
        }
    }
}
