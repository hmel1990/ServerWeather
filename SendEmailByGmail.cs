using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServerWeather
{
    internal class SendEmailByGmail
    {
        private static string senderEmail = "hmelforitstep@gmail.com"; // отправитель
        private static string senderPassword = "zgzc amfi nmur npps"; // пароль приложения https://github.com/sunmeat/secret_things/blob/main/app_code.txt
        public string receiverEmail = "hmel408757595@gmail.com"; // получатель

        // SMTP-клиент для Gmail

        private SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true
        };

        public async Task <string> SendEmail (string textMessage)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(new MailAddress(receiverEmail));
                mail.Subject = "Тестовое письмо";
                mail.Body = $@"{textMessage}";

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
