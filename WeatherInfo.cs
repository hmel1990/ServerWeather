using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

namespace ServerWeather
{
    public class WeatherInfo
    {
        //https://api.openweathermap.org/data/2.5/weather?q=Москва&appid=f71929617bd3e217acbd51006be1e75c&units=metric&lang=ru
        public string Url { get; set; }

        public WeatherInfo(string cityName)
        {
            Url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid=f71929617bd3e217acbd51006be1e75c&units=metric&lang=ru";
        }

        //Делаем запрос к API и получаем JSON-ответ
        private async Task<string> ReadJsonFileAsync()
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(Url);
                    response.EnsureSuccessStatusCode();
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    return jsonContent;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении данных: {ex.Message}\n{ex.StackTrace}");
                return "";
            }
        }

        //Десериализация JSON 
        public async Task<WeatherJson?> DeserializeJsonAsync()
        {
            try
            {
                var jsonContent = await ReadJsonFileAsync();

                WeatherJson? weatherDataFromJSON = JsonSerializer.Deserialize<WeatherJson>(jsonContent);
                return weatherDataFromJSON;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при десериализации JSON: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }



    }
}
