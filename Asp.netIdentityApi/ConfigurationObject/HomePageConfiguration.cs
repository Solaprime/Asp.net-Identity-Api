using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.ConfigurationObject
{
    public class HomePageConfiguration
    {

        public bool EnableGreeting { get; set; }
        public bool EnableWeatherForecast { get; set; }
        [Required]
        public string ForecastSectionTitle { get; set; }
    }
}
