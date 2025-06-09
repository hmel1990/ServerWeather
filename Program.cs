using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerWeather
{
    internal class Program
    {




        
          static async Task Main()
        {
            string port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
            string url = $"http://+:{port}/send/"; // Для локального тестирования можно использовать "http://localhost:5000/send/"
            //string url = "http://localhost:5000/send/";


            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine($"Сервер запущен на {url}");

            while (true)
            {
                var context = await listener.GetContextAsync();
                _ = Task.Run(() => ProcessRequest(context));
            }
        }

        static async Task ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            if (request.HttpMethod != "POST")
            {
                response.StatusCode = 405;
                await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("Только POST-запросы"));
                response.OutputStream.Close();
                return;
            }

            using var reader = new StreamReader(request.InputStream, Encoding.UTF8);
            string numberStr = await reader.ReadToEndAsync();

            if (int.TryParse(numberStr, out int number))
            {
                if (number == 100500)
                {
                    var resultWeather = new WeatherInfo("Одесса");
                    var resultFromJSON = await resultWeather.DeserializeJsonAsync();

                    var weatherData = new
                    {
                        temperature = resultFromJSON?.Main?.Temp,
                        windSpeed = resultFromJSON?.Wind?.Speed,
                        description = resultFromJSON?.Weather?.FirstOrDefault()?.Description
                    };

                    string json = JsonSerializer.Serialize(weatherData);
                    byte[] buffer = Encoding.UTF8.GetBytes(json);

                    response.ContentType = "application/json";
                    response.ContentLength64 = buffer.Length;
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            else
            {
                response.StatusCode = 400;
                await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("Ошибка: ожидалось число."));
            }

            response.OutputStream.Close();
        }
         
        

    }

}

