using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Internship_Application.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Internship_Application.Controllers
{
    public class Receipt_Submission_Page_EmployerSSFoRController : Controller
    {
        private readonly DataContext _context;

        public Receipt_Submission_Page_EmployerSSFoRController(DataContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Employer,StudentServices,FacultyOfRec")]
        public IActionResult Index()
        {
            sendEmailtoAdmin();
            sendEmailtoSelf();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void sendEmailtoAdmin()
        {
            string to = "sadakc2@winthrop.edu";//hardcoded to the administrator email. sorry. --- To address    
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to); var form = _context.Forms.ToList<Forms>();
            var studentName = "";

            foreach (var record in form)
            {
                studentName = record.StudentName;
            }

            string mailbody = User.Identity.Name + "completed their portion of the form for " + studentName;
            message.Subject = "Student Application Status Change";
            message.Body = mailbody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.office365.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("smtps19@winthrop.edu", "SpringSnow2019!");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void sendEmailtoSelf()
        {
            string to = User.Identity.Name; //To address    
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Thank you for completing your portion of the CBA Internship Agreement. A confirmation email will be sent to " + User.Identity.Name;


            message.Subject = "Internship Agreement Confirmation Email";
            message.Body = mailbody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.office365.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("smtps19@winthrop.edu", "SpringSnow2019!");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
