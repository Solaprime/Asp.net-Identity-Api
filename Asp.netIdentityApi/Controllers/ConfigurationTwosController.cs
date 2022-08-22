using Asp.netIdentityApi.ConfigurationObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationTwosController : ControllerBase
    {
        private readonly HomePageConfiguration _homepageConfig;
        private readonly int minToCache;
        private readonly int minToCacheTwo;
        public ConfigurationTwosController(
            IOptionsSnapshot<HomePageConfiguration> options,
           IOptionsMonitor<ExternalServicesConfig> serviceconfig )
        {
            _homepageConfig = options.Value;
            // _serviceConfig = serviceconfig.CurrentValue;
            minToCache = serviceconfig.Get("ProductsApi").MinsToCache;
            minToCacheTwo = serviceconfig.Get("WeatherApi").MinsToCache;

        }
        [HttpGet]
        public ActionResult  Get()
        {
            return Ok(JsonConvert.SerializeObject(_homepageConfig));
        }
        [HttpGet("NamedOption")]
        public ActionResult GetWithNamed()
        {
            return Ok($"First oen {minToCache} and the second oen {minToCacheTwo}");
        }
    }
}
// We can use the IoptionsTobind instead of Bimding to aclass directly
// Ioptions provide some Juicy stuff far beeter than We binding directly e.g
//  IOptionsSnapshot<HomePageConfiguration> options, IoptionsSnapshot
// provides the ability of our configure service tonreflect immediately if changed 
// real life application in some games or app, where certain part might be UNAccessible
// due to bug, den when the bug/Functionality is fixed, when the user refresh the page
// everythong will be back to noraml instead of recompiling and redeploying+
// This relaofing feature provided by Ioptionsnapshpt can be costly at times
// not all features and functionality should be done this way to
// Prevent adverse effect

// we have IOptions<T>, IOptionsSnapshot<T>, IOptionsMonitor<T>,
// IOptionsMonitor<T>, has more functionality than Ioptionsnapshot, and does some Event, Signal R flow
 