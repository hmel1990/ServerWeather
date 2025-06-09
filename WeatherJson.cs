using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerWeather
{
    public class WeatherJson
    {
        [JsonPropertyName("weather")]
        public List<Weather>? Weather { get; set; }

        [JsonPropertyName("main")]
        public Main? Main { get; set; }

        [JsonPropertyName("wind")]
        public Wind? Wind { get; set; }

    }
    public class Weather
    {
        [JsonPropertyName("main")]
        public string? Main { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    public class Main
    {
        [JsonPropertyName("temp")]
        public double? Temp { get; set; }
    }

    public class Wind
    {
        [JsonPropertyName("speed")]
        public double? Speed { get; set; }
    }
}
