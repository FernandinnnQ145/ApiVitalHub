
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WebAPI.Utils.Mail
{
    public class EmailService : IEmailService
    {
        //variavel que armazena as configs de EmailSettings
        private readonly EmailSettings emailSettings;


        //Construtor que recebe a dependence injection de EmailSettings
        public EmailService(IOptions<EmailSettings> options) {
            emailSettings = options.Value;


        }



    
        //metodo para o envio de email
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                //objeto que representa o email
                var email = new MimeMessage();

                //define o remetente do email
                email.Sender = MailboxAddress.Parse(emailSettings.Email);

                //define o destinatario do email
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

                //define o assunto do email
                email.Subject = mailRequest.Subject;

                //cria o corpo do email
                var builder = new BodyBuilder();


                //define o corpo do email como HTML
                builder.HtmlBody = mailRequest.Body;

                //define o corpo do email no obj MimeMessage
                email.Body = builder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    //conecta-se ao servidor SMTP usando os dados de emailSettings
                    smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);

                    //autentica-se no servidor usando os dados de emailSettings
                    smtp.Authenticate(emailSettings.Email, emailSettings.Password);

                    //envia o email
                    await smtp.SendAsync(email);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
