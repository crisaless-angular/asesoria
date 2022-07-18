using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BA002.Web.EnviarSmtp
{
    public class Smtp
    {
        public void EnvioSmtp()
        {
            string Email = "crisalesscrisaless1@gmail.com";
            string EmailDes = "carlos@crisaless.es";
            string conra = "Cris18208.";

            MailMessage d = new MailMessage(Email, EmailDes, "Asunto", "Mensaje");

            d.IsBodyHtml = true;

            SmtpClient f = new SmtpClient("smtp.gmail.com");
            f.EnableSsl = true;
            f.UseDefaultCredentials = false;
            f.Port = 587;
            f.Credentials = new System.Net.NetworkCredential(Email, conra);
            f.Send(d);
            f.Dispose();
        }
    }
}
