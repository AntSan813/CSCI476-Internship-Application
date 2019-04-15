using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Internship_Application.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace Internship_Application.Controllers
{

    [Authorize(Roles = "Student, StudentServices, Admin, Employer, FacultyOfRec")]//all of the roles can access this page
    public class Receipt_Submission_PageController : Controller
    {
        private readonly DataContext _context;

        public Receipt_Submission_PageController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var form = _context.Forms
                .FirstOrDefault(m => m.Id == id);
    //do certain functions based on what user is accessing this page
            if (User.IsInRole("Student"))
            {
                sendEmailtoSelf(); //confirmation email
                sendEmailtoEmployer(form.StudentName, form.StudentEmail, form.EmployerEmail);
                sendEmailtoAdmin(form.StudentName, form.StudentEmail, form.StatusCodeId);
            }
            if(User.IsInRole("Admin") && form.StatusCodeId == 4)//if role is admin and code is pending student services approval
            {
                sendEmailtoSelf();
                sendEmailtoStudent(form.StudentName, form.StudentEmail, form.StatusCodeId);
                sendEmailtoStudentServices(form.StudentName, form.StudentEmail, form.StatusCodeId);
            }
            if(User.IsInRole("Admin") && form.StatusCodeId == 6)//if user is admin and code is pending DER approval
            {
                sendEmailtoSelf();
                sendEmailtoStudent(form.StudentName, form.StudentEmail, form.StatusCodeId);
                sendEmailtoFaculty(form.StudentName, form.StudentEmail, form.FacultyEmail, form.StatusCodeId);
            }
            if(User.IsInRole("Admin") && (form.StatusCodeId == 8 || form.StatusCodeId == 9 || form.StatusCodeId == 10))//if role is admin and code is approved, declined, or withdrawn
            {
                sendEmailtoSelf();
                sendFinalEmail(form.StudentEmail, form.StatusCodeId);
            }
            if (User.IsInRole("Employer") || User.IsInRole("StudentServices") || User.IsInRole("FacultyOfRec"))//if the role is employer, SS or faculty
            {
                sendEmailtoSelf();
                sendEmailtoStudent(form.StudentName, form.StudentEmail, form.StatusCodeId);
                sendEmailtoAdmin(form.StudentName, form.StudentEmail, form.StatusCodeId);
            }

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public void sendEmailtoAdmin(string studentName, string studentEmail, int statusCode)
        {
            string to = "sadakc2@winthrop.edu";//hardcoded to the ADMIN EMAIL --- To address    
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to); var form = _context.Forms.ToList<Forms>();
            var employerEmail = "";

            foreach (var record in form)
            {
                employerEmail = record.EmployerEmail;//get the employer email and student name
                studentName = record.StudentName;
            }

            string mailbody = studentName + "'s ( " + studentEmail + ") form has moved to the next stage in the pipeline. Now at step " + statusCode + ". Login to view more information about this application."; //the body of the email
            message.Subject = "CBA Internship Application Status Update";//the subject of the email
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
                client.Send(message);//send the email
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public void sendEmailtoStudentServices(string studentName, string studentEmail, int statusCode)
        {
            string to = "oliverj14@winthrop.edu";//CHANGE THIS TO STUDENT SERVICES WHEN READY   
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to); var form = _context.Forms.ToList<Forms>();
            var employerEmail = "";

            foreach (var record in form)
            {
                employerEmail = record.EmployerEmail;//get the employer email and student name
                studentName = record.StudentName;
            }

            string mailbody = studentName + "'s ( " + User.Identity.Name + ") form requires the attention of Student Services."; //the body of the email
            message.Subject = "CBA Internship Agreement Requires Your Attention";//the subject of the email
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
                client.Send(message);//send the email
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void sendEmailtoEmployer(string studentName, string studentEmail, string employerEmail)
        {
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
                "6. Click submit. \n NOTE: Once you click submit, the form will be sent to the Director of External Relations for further review.<br>" +
                "If you feel that you have been contacted in error, please contact the Director of External Relations at Winthrop University a tillerc@winthrop.edu.";
         

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

        public void sendEmailtoStudent(string studentName, string studentEmail, int statusCode)
        {

            string to = studentEmail; //To address    
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to);

            //the students body of the email....tell them the employer they sent their application to. If this email is incorrect it is shown here
            string mailbody = "Your CBA Internship Application form has moved to the next stage in the pipeline. Now at step " + statusCode + ".";


            message.Subject = "CBA Internship Application Status Update";//the subject of the email
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
                client.Send(message);//send the email
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void sendEmailtoFaculty(string studentName, string studentEmail, string facultyEmail, int statusCodeId)
        {

            //string to = facultyEmail; //To address
            string to = facultyEmail;
            //string to = "christinasadak@gmail.com";
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "You have been listed as the Faculty of Record for "+ studentName + "'s (" + studentEmail + ") internship. You may now view and complete your portion of the internship agreement. If you are new to this process, please register using this email and your choice of password. If you have done this before, please login to view forms that require your attention.";

            message.Subject = "CBA Internship Agreement Requires Your Attention";
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

            string mailbody = "Thank you for completing your portion of the CBA Internship Agreement.";//tell them thank you for submitting their part of the application


            message.Subject = "Internship Agreement Confirmation Email";//subject of the email
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

        public void sendFinalEmail(string studentEmail, int statusCode)
        {
            string to = studentEmail; //To address    
            string from = "smtps19@winthrop.edu"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Your CBA Internship Application Form has been finalized. Please log into your account to view the final status of your application.";


            message.Subject = "CBA Internship Application Has Been Finalized";//subject of the email
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
