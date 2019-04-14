using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Internship_Application.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

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

        // GET: /<controller>/ and forms c:
        public IActionResult Index()
        {
            // forms is a list of all forms
            var forms = _context.Forms.ToList<Forms>();

            if (forms == null)
            {
                //TODO: move this to a function
                return View();
            }

            
            List<FormViewModel> serializedFormList = new List<FormViewModel>();
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
            }
            List<Companies> companies = _context.Companies.ToList();
            List<Models.StatusCodes> status_codes = _context.StatusCodes.ToList();
            
            ViewBag.Companies = companies;
            ViewBag.Forms = serializedFormList;
            ViewBag.StatusCodes = status_codes;

            ViewBag.QueryByCompanyName = 0;
            ViewBag.QueryByYear = 0;
            ViewBag.QueryBySemester = 0;
            ViewBag.QueryByCompanyLocation = 0;
            ViewBag.QueryByPaid = 0;
            ViewBag.QueryByUnpaid = 0;
            ViewBag.QueryByStatusCode = 0;


            QueryPageViewModel queryPageModel = new QueryPageViewModel
            {
                PreviousCompanyLocationQuery = 0,
                PreviousCompanyNameQuery = 0,
                PreviousYearQuery = 0,
                PreviousPaidQuery = 0,
                PreviousUnpaidQuery = 0,
                PreviousSemesterQuery = 0,
                PreviousStatusCodeQuery = 0
            };

            return View(queryPageModel);
        }
        
        private Dictionary<string, List<int>> GetSemesters(List<Forms> forms)
        {
            var semesters = new Dictionary<string, List<int>>()
            {
                { "Fall", new List<int> () },
                { "Summer", new List<int> () },
                { "Spring", new List<int> () },
            };
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult QueryByCompanyName(IFormCollection formcollection)
        //TODO: refactor following function to call other functions that process each type of query
        public IActionResult Index(IFormCollection formcollection, [Bind("PreviousCompanyNameQuery,PreviousCompanyLocationQuery,PreviousYearQuery,PreviousSemesterQuery,PreviousPaidQuery,PreviousUnpaidQuery,PreviousStatusCodeQuery")] QueryPageViewModel queryPageModel)
        {
            IQueryable<Forms> query = _context.Forms;
            List<FormViewModel> serializedFormList = new List<FormViewModel>();
           
            //QUERY BY YEAR
            if (QueryByYear(formcollection, queryPageModel))
            {
                if (formcollection.ContainsKey("Year") && (formcollection["Year"] != ""))
                {
                    if(formcollection["YearId"] == "")
                    { 
                        queryPageModel.PreviousYearQuery = 0;
                    }
                    else
                    {
                        queryPageModel.PreviousYearQuery = Int32.Parse(formcollection["YearId"]);
                    }
                }
                if (queryPageModel.PreviousYearQuery != 0)
                {
                    query = query.Where(m => m.CreatedAt.Year == queryPageModel.PreviousYearQuery);
                }
                ViewBag.QueryByYear = queryPageModel.PreviousYearQuery;
            }

            //QUERY BY COMPANY NAME
            if (QueryByCompanyName(formcollection, queryPageModel))
            {
                if (formcollection.ContainsKey("CompanyName") && (formcollection["CompanyName"] != ""))
                {
                    if (formcollection["CompanyNameId"] == "")
                    {
                        queryPageModel.PreviousCompanyNameQuery = 0;
                    }
                    else
                    {
                        queryPageModel.PreviousCompanyNameQuery = Int32.Parse(formcollection["CompanyNameId"]);
                    }
                }
                if (queryPageModel.PreviousCompanyNameQuery != 0)
                {
                    query = query.Where(m => m.CompanyId == queryPageModel.PreviousCompanyNameQuery);
                }
                ViewBag.QueryByCompanyName = queryPageModel.PreviousCompanyNameQuery;
            }


            //QUERY BY COMPANY LOCATION
            if (QueryByCompanyLocation(formcollection, queryPageModel))
            {
                if (formcollection.ContainsKey("CompanyLocation") && (formcollection["CompanyLocation"] != ""))
                {
                    if (formcollection["CompanyLocationId"] == "")
                    {
                        queryPageModel.PreviousCompanyLocationQuery = 0;
                    }
                    else
                    {
                        queryPageModel.PreviousCompanyLocationQuery = Int32.Parse(formcollection["CompanyLocationId"]);
                    }
                }
                if (queryPageModel.PreviousCompanyLocationQuery != 0)
                {
                    query = query.Where(m => m.CompanyId == queryPageModel.PreviousCompanyLocationQuery);
                }
                ViewBag.QueryByCompanyLocation = queryPageModel.PreviousCompanyLocationQuery;
            }

            //QUERY BY STATUS CODE
            if (QueryByStatusCode(formcollection, queryPageModel))
            {
                if (formcollection.ContainsKey("StatusCode") && (formcollection["StatusCode"] != ""))
                {
                    if (formcollection["StatusCodeId"] == "")
                    {
                        queryPageModel.PreviousStatusCodeQuery = 0;
                    }
                    else
                    {
                        queryPageModel.PreviousStatusCodeQuery = Int32.Parse(formcollection["StatusCodeId"]);
                    }
                }
                if (queryPageModel.PreviousStatusCodeQuery != 0)
                {
                    query = query.Where(m => m.StatusCodeId == queryPageModel.PreviousStatusCodeQuery);
                }
                ViewBag.QueryByStatusCode = queryPageModel.PreviousStatusCodeQuery;
            }

            //QUERY BY PAID
            if (QueryByPaid(formcollection, queryPageModel))
            {
                if (formcollection.ContainsKey("Paid") && (formcollection["Paid"].Count > 1) && !String.IsNullOrEmpty(formcollection["Paid"][1]))
                {
                    if (formcollection["Paid"][1] == "false")
                    {
                        queryPageModel.PreviousPaidQuery = 1;
                    }
                    else
                    {
                        queryPageModel.PreviousPaidQuery = 0;
                    }
                }
                else if (formcollection.ContainsKey("Paid") && formcollection["Paid"] == "false")
                {
                    queryPageModel.PreviousPaidQuery = 0;
                }

                if (queryPageModel.PreviousPaidQuery != 0)
                {
                    query = query.Where(m => m.Paid == 1);
                }

                ViewBag.QueryByPaid = queryPageModel.PreviousPaidQuery;
            }

            //QUERY BY UNPAID
            if (QueryByUnpaid(formcollection, queryPageModel))
            {
                if (formcollection.ContainsKey("Unpaid") && (formcollection["Unpaid"].Count > 1) && !String.IsNullOrEmpty(formcollection["Unpaid"][1]))
                {
                    if (formcollection["Unpaid"][1] == "false")
                    {
                        queryPageModel.PreviousUnpaidQuery = 1;
                    }
                    else
                    {
                        queryPageModel.PreviousUnpaidQuery = 0;
                    }
                }
                else if (formcollection.ContainsKey("Unpaid") && formcollection["Unpaid"] == "false")
                {
                    queryPageModel.PreviousUnpaidQuery = 0;
                }

                if (queryPageModel.PreviousUnpaidQuery != 0)
                {
                    query = query.Where(m => m.Paid == 0);
                }
                ViewBag.QueryByUnpaid = queryPageModel.PreviousUnpaidQuery;
            }

            
            List<Forms> forms = query.ToList<Forms>();
            //QUERY BY SEMESTER
            if (QueryBySemester(formcollection, queryPageModel))
            {
                if (formcollection.ContainsKey("Semester") && (formcollection["Semester"] != ""))
                {
                    if (formcollection["SemesterId"] == "")
                    {
                        queryPageModel.PreviousSemesterQuery = 0;
                    }
                    else
                    {
                        queryPageModel.PreviousSemesterQuery = Int32.Parse(formcollection["SemesterId"]);
                    }
                }
                if (queryPageModel.PreviousSemesterQuery != 0)
                {
                    var semesters = GetSemesters(forms);
                    List<Forms> newFormList = new List<Forms>();

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

            }

            ViewBag.StatusCodes = _context.StatusCodes.ToList<Models.StatusCodes>();
            ViewBag.Companies = _context.Companies.ToList<Companies>();
            ViewBag.Forms = serializedFormList;

            return View(queryPageModel);
        }
    }
}
