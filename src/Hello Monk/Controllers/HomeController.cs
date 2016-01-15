using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Monk.Domain.Entities;
using Monk.Domain.Enums;
using Monk.Log;
using Monk.Web.Models;
using Ninject;

namespace Monk.Web.Controllers
{
    public class HomeController : BaseController
    {
        [Inject]
        public new ILog<HomeController> Log { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMail(MessageModel messageModel)
        {
            if (ModelState.IsValid)
            {
                //If message contains not nullable fields as null value
                //invalidate message and do not continue
                if (string.IsNullOrEmpty(messageModel.Name) || string.IsNullOrEmpty(messageModel.MailAddress) || string.IsNullOrEmpty(messageModel.Message))
                    return RedirectToActionPermanent("Index");

                Log.Debug("A sent contact message received");

                //Setup smtp
                var smtpClient = new SmtpClient(Host, Port);

                //Create new message record
                var message = new Message()
                {
                    Name = messageModel.Name,
                    Mail = messageModel.MailAddress,
                    PhoneNumber = messageModel.MailAddress,
                    Content = messageModel.Message,
                    Time = DateTime.Now
                };

                //login mail
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(MailAddress, Password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = SslEnabled;

                //Setting From , To and CC
                var mail = new MailMessage();
                mail.From = new MailAddress(MailAddress);
                mail.To.Add(new MailAddress(MailAddress));
                mail.Subject = string.Format("New message from {0}", messageModel.Name);
                mail.SubjectEncoding = Encoding.UTF8;
                var msg = string.Format("Mail from {0} <{1}>: \n\n{2}", messageModel.Name, messageModel.MailAddress, messageModel.Message);
                mail.Body = msg;
                mail.BodyEncoding = Encoding.UTF8;

                try
                {
                    await smtpClient.SendMailAsync(mail);
                    message.Status = MessageStatus.Success;
                    
                }
                catch (Exception ex)
                {
                    //If cannot send mail due to exception, log it and store 
                    //the message as fail in db
                    Log.Error(ex,"Error while sending mail");
                    message.Status = MessageStatus.Fail;
                }

                MessageManager.Insert(message);
                Log.Info(string.Format("The contact request is saved with id:{0} and status:{1}",message.Id, message.Status));
            }
            return RedirectToActionPermanent("Index");
        }
    }
}
