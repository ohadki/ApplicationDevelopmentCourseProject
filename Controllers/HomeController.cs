using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ApplicationDevelopmentCourseProject.Controllers.BranchesController;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDevelopmentCourseProjectContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDevelopmentCourseProjectContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            var contactViewModel = new ContactViewModel
            {
                Contacts =  _context.Contact.ToList(),
                Branches =  _context.Branch.ToList()
            };
            return View(contactViewModel);
        }

        [HttpPost]
        public ActionResult Contact(Contact e)
        {
            if (ModelState.IsValid)
            {

                //prepare email
                var toAddress = "talofirohad@yahoo.com";
                var fromAddress = e.Email.ToString();
                var subject = "Test enquiry from " + e.Name;
                var message = new StringBuilder();
                message.Append("Name: " + e.Name + "\n");
                message.Append("Email: " + e.Email + "\n");
                message.Append("Telephone: " + e.Telephone + "\n\n");
                message.Append(e.Message);

                //start email Thread
                var tEmail = new Thread(() =>
               SendEmail(toAddress, fromAddress, subject, message.ToString()));
                tEmail.Start();
            }
            return View();
        }

        public void SendEmail(string toAddress, string fromAddress,
                      string subject, string message)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    const string email = "talofirohad@yahoo.com";
                    const string password = "Aa123456789!";

                    var loginInfo = new NetworkCredential(email, password);


                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(new MailAddress(toAddress));
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    try
                    {
                        using (var smtpClient = new SmtpClient(
                                                         "smtp.mail.yahoo.com", 465))
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }

                    }

                    finally
                    {
                        //dispose the client
                        mail.Dispose();
                    }

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        //Response.Write("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        //Response.Write("Failed to deliver message to {0}",
                                          //t.FailedRecipient);
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                //Response.Write(Se.ToString());
            }

            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
