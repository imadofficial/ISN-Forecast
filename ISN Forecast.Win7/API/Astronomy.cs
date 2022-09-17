using ISN_Forecast.Win7.FirstSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISN_Forecast.Win7.API
{
    internal class Astronomy
    {
        public static void GetAstro()
        {
            var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.FullText);

            NewWeather.Instance.Sunrise.Text = WeatherData["forecast"]["forecastday"][0]["astro"]["sunrise"];
            NewWeather.Instance.Sunset.Text = WeatherData["forecast"]["forecastday"][0]["astro"]["sunset"];
        }
    }
}
