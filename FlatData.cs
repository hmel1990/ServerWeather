using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerWeather
{
    internal class FlatData
    {
        [JsonPropertyName("adress")]
        public string? Adress { get; set; }

        [JsonPropertyName("livingArea")]
        public string? LivingArea { get; set; }

        [JsonPropertyName("totalArea")]
        public string? TotalArea { get; set; }

        [JsonPropertyName("roomsNumber")]
        public string? RoomsNumber { get; set; }

        [JsonPropertyName("floor")]
        public string? Floor { get; set; }

        [JsonPropertyName("floorNumber")]
        public string? FloorNumber { get; set; }

        [JsonPropertyName("question")]
        public string? Question { get; set; }

        [JsonPropertyName("photo")]
        public List <string>? Photo { get; set; }
    }
}
