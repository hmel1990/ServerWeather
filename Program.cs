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
            string url = $"http://+:{port}/send/";
            // Для локального тестирования можно использовать "http://localhost:5000/send/"
            //string url = "http://localhost:5000/send/";


            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

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

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Methods", "POST, OPTIONS");
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                response.OutputStream.Close();
                return;
            }



            response.AddHeader("Access-Control-Allow-Origin", "*");



            if (request.HttpMethod != "POST")
                {
                    response.StatusCode = 405;
                    await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("Только POST-запросы"));
                    response.OutputStream.Close();
                    return;
                }

            try
            {
                using var reader = new StreamReader(request.InputStream, Encoding.UTF8);
                string data = await reader.ReadToEndAsync();

                // Десериализация в объект
                var myData = JsonSerializer.Deserialize<FlatData>(data);

                string emailText = $"Адрес: {myData.Adress}\n" +
                                        $"Жилая площадь: {myData.LivingArea}\n" +
                                        $"Общая площадь: {myData.TotalArea}";

                var emailSender = new SendEmailByGmail();

                var responseText = await emailSender.SendEmail(emailText);


                byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                string responseText = "Произошла ошибка: "+ ex.Message;
                byte[] buffer = Encoding.UTF8.GetBytes(responseText);

                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            finally { 
                response.OutputStream.Close();            
            }


        }
    }
}

