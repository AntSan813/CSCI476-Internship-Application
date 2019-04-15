using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Internship_Application.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Internship_Application.Controllers
{
    [Authorize(Roles = "Admin")]//only the admin is allowed to access this page
    public class Admin_Query_PageController : Controller
    {
        private readonly DataContext _context;

        public Admin_Query_PageController(DataContext context)
        {
            _context = context;
        }

        // GET: Templates/Details/5
        [HttpGet]
        public IActionResult DisplayForm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = _context.Forms
                .First(m => m.Id == id);

            if (form == null)
            {
                return NotFound();
            }
            
            //if form exists, then get corresponding template
            var template = _context.Templates
                 .First(m => m.Id == form.TemplateId);

            //generate form view model from form
            var formViewModel = new FormViewModel
            {
                Id = form.Id,
                StudentName = form.StudentName,
                UpdatedAt = form.UpdatedAt,
                StudentEmail = form.StudentEmail,
                EmployerEmail = form.EmployerEmail,
                FacultyEmail = form.FacultyEmail,
                StatusCodesViewModel = new Models.StatusCodes
                {
                    Id = form.StatusCodeId,
                    StatusCode = _context.StatusCodes.FirstOrDefault(s => s.Id == form.StatusCodeId).StatusCode,
                    Details = _context.StatusCodes.FirstOrDefault(s => s.Id == form.StatusCodeId).Details
                }
            };
            List<Answers> answers = JsonConvert.DeserializeObject<List<Answers>>(form.Answers);
            List<Questions> questions = JsonConvert.DeserializeObject<List<Questions>>(template.Questions);
            List<InputViewModel> inputs = new List<InputViewModel>();

            //go through list of questions and answers and generate a list of input view models
            foreach (var q in questions)
            {
                foreach (var a in answers)
                {
                    //once we find the answer object that cooresponds to questions object, we want to 
                    //process it into the input view model
                    if (q.Order == a.Order)
                    {
                        //isRequired by role?
                        bool isReq = false;
                        bool isDisabled = true;
                        isDisabled = false;
                        isReq = q.Required;
                        inputs.Add(new InputViewModel
                        {
                            DisplayOrder = Convert.ToInt32(q.Order),
                            Order = Convert.ToInt32(q.Order),
                            Prompt = q.Prompt,
                            InputType = q.InputType,
                            HelperText = q.HelperText,
                            Options = q.Options,
                            Signed = q.Signed,
                            Required = q.Required,
                            ProcessQuestion = q.ProcessQuestion,
                            Role = q.Role,
                            Value = a.Value,
                            DateSigned = a.DateSigned,
                            isRequired = isReq,
                            isDisabled = isDisabled
                        });
                    }
                }
            }

            QuestionsAndAnswers qaList = new QuestionsAndAnswers
            {
                //FormDetails = formViewModel,
                TemplateDetails = template,
                QuestionList = questions,
                AnswerList = answers,
                InputList = inputs
            };
            return View(qaList);
        }


        // GET: /<controller>/ and forms c:
        //Main index
        public IActionResult Index()
        {
            // forms is a list of all forms
            var forms = _context.Forms.ToList<Forms>();

            if (forms == null)
            {
                //TODO: move this to a function
                return View();
            }

            List<int> queriedFormIds = new List<int>();
            List<FormViewModel> serializedFormList = new List<FormViewModel>();

            //convert list of forms to list of form view models
            foreach (var form in forms)
            {
                Templates template = _context.Templates.Find(form.TemplateId);
                serializedFormList.Add(new FormViewModel
                {
                    Id = form.Id,
                    StudentName = form.StudentName,
                    UpdatedAt = form.UpdatedAt,
                    StatusCodesViewModel = _context.StatusCodes.Find(form.StatusCodeId),

                });
                queriedFormIds.Add(form.Id);
            }
            List<Companies> companies = _context.Companies.ToList();
            List<Models.StatusCodes> status_codes = _context.StatusCodes.ToList();
            
            //select list options
            ViewBag.Companies = companies;
            ViewBag.Forms = serializedFormList;
            ViewBag.StatusCodes = status_codes;

            //default query values
            ViewBag.QueryByCompanyName = 0;
            ViewBag.QueryByYear = 0;
            ViewBag.QueryBySemester = 0;
            ViewBag.QueryByCompanyLocation = 0;
            ViewBag.QueryByPaid = 0;
            ViewBag.QueryByUnpaid = 0;
            ViewBag.QueryByStatusCode = 0;

            //default query page view model
            QueryPageViewModel queryPageModel = new QueryPageViewModel
            {
                PreviousCompanyLocationQuery = 0,
                PreviousCompanyNameQuery = 0,
                PreviousYearQuery = 0,
                PreviousPaidQuery = 0,
                PreviousUnpaidQuery = 0,
                PreviousSemesterQuery = 0,
                PreviousStatusCodeQuery = 0,
                QueriedFormIds = queriedFormIds
            };

            return View(queryPageModel);
        }


        //Helper function that assigns semesters to each form
        private Dictionary<string, List<int>> GetSemesters(List<Forms> forms)
        {
            //first, create list of semesters
            var semesters = new Dictionary<string, List<int>>()
            {
                { "Fall", new List<int> () },
                { "Summer", new List<int> () },
                { "Spring", new List<int> () },
            };
            //then, go through each form and calculate its month
            foreach (var form in forms)
            {
                if ((form.UpdatedAt.Month < 5) && (form.UpdatedAt.Month > 0))
                {
                    //if the form is from the Spring semester
                    semesters["Spring"].Add(form.Id);
                }
                if ((form.UpdatedAt.Month > 5) && (form.UpdatedAt.Month < 9))
                {
                    //if the form is from the Spring semester
                    semesters["Summer"].Add(form.Id);
                }
                if ((form.UpdatedAt.Month > 9) && (form.UpdatedAt.Month < 13))
                {
                    //if the form is from the Spring semester
                    semesters["Fall"].Add(form.Id);
                }
            }
            return semesters;
        }

        //*******START FILTER FUNCTIONS**************//
        public QueryPageViewModel Filter(IQueryable<Forms> forms)
        {
            List<FormViewModel> serializedFormList = new List<FormViewModel>();
            List<int> queriedFormIds = new List<int>();
            foreach (var form in forms)
            {
                //for each for we query, we want to parse its attributes and add it to an object
                Templates template = _context.Templates.Find(form.TemplateId);
                serializedFormList.Add(new FormViewModel
                {
                    Id = form.Id,
                    StudentName = form.StudentName,
                    UpdatedAt = form.UpdatedAt,
                    StatusCodesViewModel = _context.StatusCodes.Find(form.StatusCodeId),
                });
                queriedFormIds.Add(form.Id);
            }
            //get all companies from db
            List<Companies> companies = _context.Companies.ToList();
            //get all status codes from db
            List<Models.StatusCodes> status_codes = _context.StatusCodes.ToList();

            //we always need these in viewbag
            ViewBag.Companies = companies;
            ViewBag.Forms = serializedFormList;
            ViewBag.StatusCodes = status_codes;

            //set everything else to default
            ViewBag.QueryByCompanyName = 0;
            ViewBag.QueryByYear = 0;
            ViewBag.QueryBySemester = 0;
            ViewBag.QueryByCompanyLocation = 0;
            ViewBag.QueryByPaid = 0;
            ViewBag.QueryByUnpaid = 0;
            ViewBag.QueryByStatusCode = 0;

            //create our default view model 
            QueryPageViewModel queryPageModel = new QueryPageViewModel
            {
                PreviousCompanyLocationQuery = 0,
                PreviousCompanyNameQuery = 0,
                PreviousYearQuery = 0,
                PreviousPaidQuery = 0,
                PreviousUnpaidQuery = 0,
                PreviousSemesterQuery = 0,
                PreviousStatusCodeQuery = 0,
                QueriedFormIds = queriedFormIds
            };

            return queryPageModel;
        }
//********START SEARCH FILTER FUNCTIONS*******
        [HttpPost]
        public IActionResult FilterEmail(string searchString)
        {
            //get list of forms from db
            var forms = from m in _context.Forms
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                //if the search string isnt null or empty, then seach the all student emails
                forms = forms.Where(s => s.StudentEmail.Contains(searchString));
            }

            if (!forms.Any())
            {
                //if no student email exists, check employer email...
                forms = _context.Forms.Where(s => s.EmployerEmail.Contains(searchString));
                if(!forms.Any())
                {
                    //if no student email or employer email exists, search by faculty email...
                    forms = _context.Forms.Where(s => s.FacultyEmail.Contains(searchString));
                }
            }

            var queryPageModel = Filter(forms);
            return View("Index", queryPageModel);
        }

        [HttpPost]
        public IActionResult FilterStudentName(string searchString)
        {
            // forms is a list of all forms
            // var forms = _context.Forms.ToList<Forms>();
            var forms = from m in _context.Forms
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                forms = forms.Where(s => s.StudentName.Contains(searchString));
            }

            var queryPageModel = Filter(forms);
            return View("Index", queryPageModel);
        }



        [HttpPost]
        public IActionResult FilterWinthropId(string searchString)
        {
            // forms is a list of all forms
            // var forms = _context.Forms.ToList<Forms>();
            var forms = from m in _context.Forms
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                forms = forms.Where(s => s.WuId.Contains(searchString));
            }

            var queryPageModel = Filter(forms);
            return View("Index", queryPageModel);
        }
//********START SEARCH FILTER FUNCTIONS*******

//*****START HELPER FUNCTIONS TO DETERMINE WHAT TO QUERY*******
        private Boolean QueryByCompanyName(IFormCollection formcollection, QueryPageViewModel queryPageModel)
        {
            if ((formcollection.ContainsKey("CompanyName") && (formcollection["CompanyName"] != "")) || (queryPageModel.PreviousCompanyNameQuery != 0))
            {
                return true;
            }
            return false;
        }

        private Boolean QueryByCompanyLocation(IFormCollection formcollection, QueryPageViewModel queryPageModel)
        {
            if ((formcollection.ContainsKey("CompanyLocation") && (formcollection["CompanyLocation"] != "")) || (queryPageModel.PreviousCompanyLocationQuery != 0))
            {
                return true;
            }
            return false;
        }

        private Boolean QueryByYear(IFormCollection formcollection, QueryPageViewModel queryPageModel)
        {
            if ((formcollection.ContainsKey("Year") && (formcollection["Year"] != "")) || (queryPageModel.PreviousYearQuery != 0))
            {
                return true;
            }
            return false;
        }
        private Boolean QueryBySemester(IFormCollection formcollection, QueryPageViewModel queryPageModel)
        {
            if ((formcollection.ContainsKey("Semester") && (formcollection["Semester"] != "")) || (queryPageModel.PreviousSemesterQuery != 0))
            {
                return true;
            }
            return false;
        }
        private Boolean QueryByStatusCode(IFormCollection formcollection, QueryPageViewModel queryPageModel)
        {
            if ((formcollection.ContainsKey("StatusCode") && (formcollection["StatusCode"] != "")) || (queryPageModel.PreviousStatusCodeQuery != 0))
            {
                return true;
            }
            return false;
        }
        private Boolean QueryByPaid(IFormCollection formcollection, QueryPageViewModel queryPageModel)
        {
            if ((formcollection.ContainsKey("Paid") && (formcollection["Paid"].Count > 1) &&!String.IsNullOrEmpty(formcollection["Paid"][1])) || (queryPageModel.PreviousPaidQuery != 0))
            {
                return true;
            }
            return false;
        }
        private Boolean QueryByUnpaid(IFormCollection formcollection, QueryPageViewModel queryPageModel)
        {
            if ((formcollection.ContainsKey("Unpaid") && (formcollection["Unpaid"].Count > 1) && !String.IsNullOrEmpty(formcollection["Unpaid"][1])) || (queryPageModel.PreviousUnpaidQuery != 0))
            {
                return true;
            }
            return false;
        }
//*****END HELPER FUNCTIONS TO DETERMINE WHAT TO QUERY*******


        [HttpPost]
        [ValidateAntiForgeryToken]
        //TODO: refactor following function to call other functions that process each type of query
        //This function queries everything the user specifies
        public IActionResult Index(IFormCollection formcollection, QueryPageViewModel queryPageModel)
        {
            IQueryable<Forms> query = _context.Forms;
            List<FormViewModel> serializedFormList = new List<FormViewModel>();
           
            //QUERY BY YEAR
            if (QueryByYear(formcollection, queryPageModel))
            {
                //if user just specified to query this
                if (formcollection.ContainsKey("Year") && (formcollection["Year"] != ""))
                {
                    //if user selected All
                    if (formcollection["YearId"] == "")
                    { 
                        queryPageModel.PreviousYearQuery = 0;
                    }
                    //Else user was more specific
                    else
                    {
                        queryPageModel.PreviousYearQuery = Int32.Parse(formcollection["YearId"]);
                    }
                }
                //if we want this queried, then search db
                if (queryPageModel.PreviousYearQuery != 0)
                {
                    query = query.Where(m => m.CreatedAt.Year == queryPageModel.PreviousYearQuery);
                }
                ViewBag.QueryByYear = queryPageModel.PreviousYearQuery;
            }

            //*******QUERY BY COMPANY NAME*******
            if (QueryByCompanyName(formcollection, queryPageModel))
            {
                //if user just specified to query this
                if (formcollection.ContainsKey("CompanyName") && (formcollection["CompanyName"] != ""))
                {
                    //if user selected All
                    if (formcollection["CompanyNameId"] == "")
                    {
                        queryPageModel.PreviousCompanyNameQuery = 0;
                    }
                    //Else user was more specific
                    else
                    {
                        queryPageModel.PreviousCompanyNameQuery = Int32.Parse(formcollection["CompanyNameId"]);
                    }
                }
                //if we want this queried, then search db
                if (queryPageModel.PreviousCompanyNameQuery != 0)
                {
                    query = query.Where(m => m.CompanyId == queryPageModel.PreviousCompanyNameQuery);
                }
                ViewBag.QueryByCompanyName = queryPageModel.PreviousCompanyNameQuery;
            }


            //*********QUERY BY COMPANY LOCATION***********
            if (QueryByCompanyLocation(formcollection, queryPageModel))
            {
                //if user just specified to query this
                if (formcollection.ContainsKey("CompanyLocation") && (formcollection["CompanyLocation"] != ""))
                {
                    //if user selected All
                    if (formcollection["CompanyLocationId"] == "")
                    {
                        queryPageModel.PreviousCompanyLocationQuery = 0;
                    }
                    //Else user was more specific
                    else
                    {
                        queryPageModel.PreviousCompanyLocationQuery = Int32.Parse(formcollection["CompanyLocationId"]);
                    }
                }
                //if we want this queried, then search db
                if (queryPageModel.PreviousCompanyLocationQuery != 0)
                {
                    query = query.Where(m => m.CompanyId == queryPageModel.PreviousCompanyLocationQuery);
                }
                ViewBag.QueryByCompanyLocation = queryPageModel.PreviousCompanyLocationQuery;
            }

            //***********QUERY BY STATUS CODE**************
            if (QueryByStatusCode(formcollection, queryPageModel))
            {
                //if user just specified to query this
                if (formcollection.ContainsKey("StatusCode") && (formcollection["StatusCode"] != ""))
                {
                    //if user selected All
                    if (formcollection["StatusCodeId"] == "")
                    {
                        queryPageModel.PreviousStatusCodeQuery = 0;
                    }
                    //else user was more specific
                    else
                    {
                        queryPageModel.PreviousStatusCodeQuery = Int32.Parse(formcollection["StatusCodeId"]);
                    }
                }
                //if we want this queried, then search db
                if (queryPageModel.PreviousStatusCodeQuery != 0)
                {
                    query = query.Where(m => m.StatusCodeId == queryPageModel.PreviousStatusCodeQuery);
                }
                ViewBag.QueryByStatusCode = queryPageModel.PreviousStatusCodeQuery;
            }


            //***************QUERY BY PAID*****************
            if (QueryByPaid(formcollection, queryPageModel))
            {
                //if user just specified to query this
                if (formcollection.ContainsKey("Paid") && (formcollection["Paid"].Count > 1) && !String.IsNullOrEmpty(formcollection["Paid"][1]))
                {
                    //if the unpaid is checked to false, then it is now checked to true
                    if (formcollection["Paid"][1] == "false")
                    {
                        queryPageModel.PreviousPaidQuery = 1;
                    }
                    //else unpaid is checked to true, then is is now checked to false
                    else
                    {
                        queryPageModel.PreviousPaidQuery = 0;
                    }
                }
                else if (formcollection.ContainsKey("Paid") && formcollection["Paid"] == "false")
                {
                    queryPageModel.PreviousPaidQuery = 0;
                }

                //if we want this queried, then search db
                //note- if user specified this to be true before, itll still be true here. all previous if else statments
                //would have been skipped
                if (queryPageModel.PreviousPaidQuery != 0)
                {
                    query = query.Where(m => m.Paid == 1);
                }

                ViewBag.QueryByPaid = queryPageModel.PreviousPaidQuery;
            }


            //**********QUERY BY UNPAID***********
            if (QueryByUnpaid(formcollection, queryPageModel))
            {
               //if user just specified to query this
                if (formcollection.ContainsKey("Unpaid") && (formcollection["Unpaid"].Count > 1) && !String.IsNullOrEmpty(formcollection["Unpaid"][1]))
                {
                    //if the unpaid is checked to false, then it is now checked to true
                    if (formcollection["Unpaid"][1] == "false")
                    {
                        queryPageModel.PreviousUnpaidQuery = 1;
                    }
                    //else unpaid is checked to true, then is is now checked to false
                    else
                    {
                        queryPageModel.PreviousUnpaidQuery = 0;
                    }
                }
                else if (formcollection.ContainsKey("Unpaid") && formcollection["Unpaid"] == "false")
                {
                    queryPageModel.PreviousUnpaidQuery = 0;
                }

                //if we want this queried, then search db
                //note- if user specified this to be true before, itll still be true here. all previous if else statments
                //would have been skipped
                if (queryPageModel.PreviousUnpaidQuery != 0)
                {
                    query = query.Where(m => m.Paid == 0);
                }
                ViewBag.QueryByUnpaid = queryPageModel.PreviousUnpaidQuery;
            }

            
            List<Forms> forms = query.ToList<Forms>();

            //**********QUERY BY SEMESTER***************
            if (QueryBySemester(formcollection, queryPageModel))
            {
                //if user just said to query by this
                if (formcollection.ContainsKey("Semester") && (formcollection["Semester"] != ""))
                {
                    //if they selected All
                    if (formcollection["SemesterId"] == "")
                    {
                        queryPageModel.PreviousSemesterQuery = 0;
                    }
                    //else they selected something specific
                    else
                    {
                        queryPageModel.PreviousSemesterQuery = Int32.Parse(formcollection["SemesterId"]);
                    }
                }
                //if the user has something specific quried, then get it from db
                if (queryPageModel.PreviousSemesterQuery != 0)
                {
                    var semesters = GetSemesters(forms);
                    List<Forms> newFormList = new List<Forms>();

                    //go through each form in database and if its fall, summer, or spring (depending on what user specified)
                    //then add it to the list of quried forms
                    foreach(var form in forms)
                    {
                        if ((queryPageModel.PreviousSemesterQuery == 1) && (semesters["Fall"].IndexOf(form.Id) != -1))
                        {
                            newFormList.Add(form);
                        }
                        else if ((queryPageModel.PreviousSemesterQuery == 2) && (semesters["Summer"].IndexOf(form.Id) != -1))
                        {
                            newFormList.Add(form);
                        }
                        else if ((queryPageModel.PreviousSemesterQuery == 3) && (semesters["Spring"].IndexOf(form.Id) != -1))
                        {
                            newFormList.Add(form);
                        }
                    }
                    forms = newFormList; 
                }
                ViewBag.QueryBySemester = queryPageModel.PreviousSemesterQuery;
            }

            //go through queryable forms variable and add them to the list of form view models object
            foreach (var form in forms)
            {
                Templates template = _context.Templates.Find(form.TemplateId);
                serializedFormList.Add(new FormViewModel
                {
                    Id = form.Id,
                    StudentName = form.StudentName,
                    UpdatedAt = form.UpdatedAt,
                    StatusCodesViewModel = _context.StatusCodes.Find(form.StatusCodeId),
                });

                queryPageModel.QueriedFormIds.Add(form.Id);
            }

            //get list options
            ViewBag.StatusCodes = _context.StatusCodes.ToList<Models.StatusCodes>();
            ViewBag.Companies = _context.Companies.ToList<Companies>();
            ViewBag.Forms = serializedFormList;

            return View(queryPageModel);
        }

        //This function is called by a button on the front end that exports everything from db to a csv file
        public void ExportCSV([Bind("QueriedFormIds")] QueryPageViewModel queryPageModel)
        {
            var sb = new StringBuilder();
            IQueryable<Forms> query = _context.Forms;
            
            //get all forms that are currently queried
            query = query.Where(m => queryPageModel.QueriedFormIds.Contains(m.Id));
            
            var forms = query.ToList<Forms>();

            //header row
            sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", "Created At", "Updated At","Winthrop Id", "Student Email"
                ,"Faculty Email","Employer Email","Student Name","Company Name", "Company Location","Status Code","Status Code Details"
                ,"Template Name", "Is Template Retired", "Is Template Active", Environment.NewLine);
            sb.AppendFormat(Environment.NewLine);
            
            //go through each row and append it to the string builder
            foreach (var item in forms)
            {
                var company = _context.Companies.First(m => m.Id == item.CompanyId);
                var template = _context.Templates.First(m => m.Id == item.TemplateId);
                var statusCode = _context.StatusCodes.First(m => m.Id == item.StatusCodeId);
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", RefactorCsvField(item.CreatedAt.ToString("yyyy/MM/dd")),
                    RefactorCsvField(item.UpdatedAt.ToString("yyyy/MM/dd")), RefactorCsvField(item.WuId), RefactorCsvField(item.StudentEmail),
                    RefactorCsvField(item.FacultyEmail), RefactorCsvField(item.StudentName), RefactorCsvField(company.CompanyName),
                    RefactorCsvField(company.CompanyLocation), RefactorCsvField(statusCode.StatusCode),
                    RefactorCsvField(statusCode.Details), RefactorCsvField(template.TemplateName), template.IsRetired,template.IsActive, Environment.NewLine);
            }

            //download as Report.csv
            HttpContext.Response.Clear();
            HttpContext.Response.Headers.Clear();
            HttpContext.Response.Headers.Add("content-disposition", "attachment;filename=Report.CSV ");
            HttpContext.Response.ContentType = "text/plain";
            HttpContext.Response.WriteAsync(sb.ToString());
        }
        private string RefactorCsvField(string data)
        {
            // CSV rules: http://en.wikipedia.org/wiki/Comma-separated_values#Basic_rules
            // From the rules:
            // 1. if the data has quote, escape the quote in the data
            // 2. if the data contains the delimiter (in our case ','), double-quote it
            // 3. if the data contains the new-line, double-quote it.

            if (data.Contains("\""))
            {
                data = data.Replace("\"", "\"\"");
            }

            if (data.Contains(","))
            {
                data = String.Format("\"{0}\"", data);
            }

            if (data.Contains(System.Environment.NewLine))
            {
                data = String.Format("\"{0}\"", data);
            }

            return data;
        }
    }
}
