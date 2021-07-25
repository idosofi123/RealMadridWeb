using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RealMadridWebApp.ExternalServices {

    public static class WeatherAPIService {

        private static readonly HttpClient client = new HttpClient();

        public static async Task<float> GetCurrentTemperature(double latitude, double longitude) {

            string response = await client.GetStringAsync($"https://api.tomorrow.io/v4/timelines?location={latitude},{longitude}&fields=temperature&timesteps=current&apikey={Keys.WeatherAPIKey}");
            JObject jsonResult = JObject.Parse(response);
            float temperature = (float)(jsonResult["data"]["timelines"][0]["intervals"][0]["values"]["temperature"]);

            return temperature;
        }
    }
}
