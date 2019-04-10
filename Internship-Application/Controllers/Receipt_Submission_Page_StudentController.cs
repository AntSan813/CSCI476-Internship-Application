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
    public class Receipt_Submission_Page_StudentController : Controller
    {
        private readonly DataContext _context;

        public Receipt_Submission_Page_StudentController(DataContext context)
        {
            _context = context;
        }

       // [Authorize(Roles = "Student")]
        public IActionResult Index()
        {
            sendEmailtoAdmin();
            sendEmailtoEmployer();
            sendEmailtoStudent();
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
            var employerEmail = "";
            var studentName = "";

            foreach (var record in form)
            {
                employerEmail = record.EmployerEmail;
                studentName = record.StudentName;
            }

            string mailbody = studentName + "( " + User.Identity.Name + ") sent his/her form to the employer (" + employerEmail + ").";
            message.Subject = "Student Application Sent to Employer";
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
        public void sendEmailtoEmployer()
        {
            var form = _context.Forms.ToList<Forms>();
            var employerEmail = "";
            var studentName = "";

            foreach (var record in form)
            {
                employerEmail = record.EmployerEmail;
                studentName = record.StudentName;
            }

            string to = employerEmail; //To address    
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to); 

            string mailbody = "Your intern, " + studentName + "( " + User.Identity.Name + ") has listed you as his/her employer for an internship. <br>" +
                "To move forward with this process, please complete the steps below.<br> " +
                "1. Follow the link listed here: https://localhost:5001/Identity/Account/Register <br> " +
                "2. Register with the email address this message was sent to and create your own password. It is important to use this email address or you won't be able to register. <br> " +
                "3. Login with your new acount. <br>" +
                "4. Click on View Form <br>" +
                "5. Fill our your portion of the form." + "<br>" +
                "6. Click submit. \n NOTE: Once you click submit, the form will be sent to the Director of External Relations for further review.<br>";
         

            message.Subject = "Student Application Sent to Employer";
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
        public void sendEmailtoStudent()
        {
            var form = _context.Forms.ToList<Forms>();
            var employerEmail = "";
            var studentName = "";

            foreach (var record in form)
            {
                employerEmail = record.EmployerEmail;
                studentName = record.StudentName;
            }

            string to = User.Identity.Name; //To address    
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Thank you for completing your portion of the CBA Internship Agreement. It has been sent to your employer, " + employerEmail +". If the employer email is incorrect, you must contact the Director of External Relations for assistance as your form will not be completed until this is fixed."
                ;


            message.Subject = "Student Application Sent to Employer";
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
