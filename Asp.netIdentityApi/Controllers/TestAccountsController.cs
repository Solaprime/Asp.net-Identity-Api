using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EmailMustStartWithTest")]
    public class TestAccountsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test to see if u can acces this acount");
        }
    }
}
