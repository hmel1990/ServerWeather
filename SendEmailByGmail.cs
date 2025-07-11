using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerWeather
{
    internal class SendEmailByGmail
    {
        private static string senderEmail = "hmelforitstep@gmail.com"; // отправитель
        private static string senderPassword = "cbwx jctt omrh pubx"; // пароль приложения https://github.com/sunmeat/secret_things/blob/main/app_code.txt
        public string receiverEmail = "hmel408757595@gmail.com"; // получатель

        // SMTP-клиент для Gmail

        private SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true
        };

        private string GetFileExtension(string mime)
        {
            return mime switch
            {
                "image/jpeg" => "jpg",
                "image/png" => "png",
                _ => "bin" // по умолчанию, если MIME не известен
            };
        }
        public async Task <string> SendEmail (FlatData myData)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(new MailAddress(receiverEmail));
                mail.Subject = "Тестовое письмо";

                string textMessage = "";

                if (!string.IsNullOrEmpty(myData.Adress))
                {
                     textMessage += $@"Адрес: {myData.Adress}" + "\n" +
                                         $"Жилая площадь: {myData.LivingArea}\n" +
                                         $"Общая площадь: {myData.TotalArea}\n" +
                                         $"Количество комнат: {myData.RoomsNumber}\n" +
                                         $"Этаж: {myData.Floor}\n" +
                                         $"Этажность: {myData.FloorNumber}\n";
                }



                if (!string.IsNullOrEmpty(myData.Question))
                {
                    textMessage += $"Вопрос: {myData.Question}\n";
                }
               

                mail.Body = $@"{textMessage}";

                int i = 1;
                foreach (var photo in myData.Photo)
                {
                    var match = Regex.Match(photo, @"data:(.+?);base64,(.+)");
                    if (!match.Success) continue;

                    string mime = match.Groups[1].Value;
                    string base64 = match.Groups[2].Value;

                    byte[] imageBytes = Convert.FromBase64String(base64);

                    var stream = new MemoryStream(imageBytes);
                    var attachment = new Attachment(stream, $"photo{i}.{GetFileExtension(mime)}", mime);
                    mail.Attachments.Add(attachment);
                    i++;
                }

                    mail.IsBodyHtml = false; // Установите true, если хотите отправить HTML-сообщение

                await smtpClient.SendMailAsync(mail);

                return "Письмо успешно отправлено!";

            }
            catch (Exception ex)
            {
                return $"Ошибка при отправке письма: {ex.Message}";
            }

        }

    }
}
