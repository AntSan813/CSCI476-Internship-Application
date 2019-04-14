using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Internship_Application.Models;
using Newtonsoft.Json;
using System.Linq;

namespace Internship_Application.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly DataContext _context;

        public RegisterModel(
            DataContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;

            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        /*Name:OnPostAsync
         * Purpose: The purpose of this function is to allow the user. If the user is able to login, they will be assigned a role here.
         * They will have the role as a student, employer, faculty, or studentservices. The user cannot be assigned the role of the administrator
         * here. 
         */ 

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)//the user is able to register an account
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    
                    //ASSIGN ROLES BASED ON EMAIL
                    string userEmail = Input.Email;//get the user's email
                    string winthrop = "@winthrop.edu";//set the string as @winthrop.edu to do some check later
                    if (userEmail == "studentservices@winthrop.edu")//Person is Student Services, this is hardcoded because this is the only email that student services can be
                    {
                        await _userManager.AddToRoleAsync(user, "StudentServices");//assign this role
                    }
                    else//Person is not an Admin, or Student Services so they must be Student, Employer, or FacultyOfRec
                    {
                        int numLoc = -1;//find the location of the number in the email
                        bool student = false;
                        var length = userEmail.Length;//get the length of the email
                        if(userEmail.Contains(winthrop))//if the email contains @winthrop.edu
                        {
                            numLoc = userEmail.IndexOf('@');//checking char before @. If #, student. If not, faculty
                            numLoc = numLoc - 1;
                            bool result2 = Char.IsDigit(Input.Email, numLoc);
                            if (result2)
                                     student = true;//the user is a student
                            if (student)//if true, set the user as a student
                            {
                                await _userManager.AddToRoleAsync(user, "Student");
                            }
                            else//false, the user is a faculty of record
                            {
                                await _userManager.AddToRoleAsync(user, "FacultyOfRec");
                            }
                        }

                        //check if employer role
                        //look through all entries in employer_email column in forms table and if the email being used to register
                        //is in that table, it is an employer and let them.
                        var form = _context.Forms.ToList<Forms>();

                        foreach (var record in form)
                        {
                            var currEmail = record.EmployerEmail;
                            if (userEmail == currEmail)
                            {
                                await _userManager.AddToRoleAsync(user, "Employer");
                            }
                        }

                    }


                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
