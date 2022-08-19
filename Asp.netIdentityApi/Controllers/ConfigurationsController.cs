using Asp.netIdentityApi.ConfigurationObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Controllers
{

    // Using IConfiguration  on multiple Key is a bad practice
    // beacuse there is repetitve code, Fragile Naming
    // In simple scenario/simple APP using Iconfiguration is good,
    //but in complex App having multiple key, and accessing dem can be quite tedious
    [Route("api/[controller]")]
    [ApiController]
    
    
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ConfigurationsController(IConfiguration configuration)
        {
            _configuration = configuration;  
        }
        [HttpGet]
        public ActionResult Get()
        {
            // THE GET Value ectract the value in the Key and Cast it to a bool
            // note we have a section and subsection so we flattend it with the : keyword
            // if the value of the Key cant be foud, authomatically the default  value
            // of the Key is assumed
            if (_configuration.GetValue<bool>("Features:HomePage:EnableGreeting"))
            {
                return Ok("Your configuration worked Normally");
            }
            return BadRequest("Something went wrong with your configuration");
           
        }

        [HttpGet("Getting")]
        public ActionResult GetWithSection()
        {
            // instead of  retyping the Flattnned section name all the timw,
            // We can improvise
            var homePageFeatures = _configuration.GetSection("Features:HomePage");
            if (homePageFeatures.GetValue<bool>("EnableGreeting"))
            {
                return Ok($"Your configuration worked Normally{homePageFeatures["ForecastSectionTitle"]}");
            }
            return BadRequest("Something went wrong with your configuration");
        }
        [HttpGet("BingConfigObject")]
        public ActionResult BindingConfigToObject()
        {
            // Using config directly is a bad Idea in complex app dat has multiple keys
            // or config properties, just a single rename or name typed wrongly can cause 
            // havoc in our App
            // an alternative way is to use configuratyion binding in our App
            var features = new Features();
            // if these method implements, any keys in our configuration gets
            // mapped to properties in our features object
            // the name of the properties described in the features must be spelled
            // the same way the keys are speeled for the binding to work
            //automatically
            _configuration.Bind("Features:HomePage", features);

            if (features.EnableWeatherForecast)
            {
                return Ok($"Your configuration worked Normall, {features.EnableGreeting}");
            }
            return BadRequest("Something went wrong with your configuration");
        }
    }
}
