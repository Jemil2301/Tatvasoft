    using Helperland.Models;
    using Helperland.Models.Data;
using Helperland.ViewModel;
using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

namespace Helperland.Controllers
{
    public class SharedController : Controller
    {
        private readonly ILogger<SharedController> _logger;
        private readonly HelperlandContext _db;
        public SharedController(ILogger<SharedController> logger, HelperlandContext db)
        {
            _logger = logger;
            _db = db;
        }
       
        [HttpPost]
        public ActionResult Login(LoginForgot reg)
        {
            
           
            var details = (from userlist in _db.Users
                           where userlist.Email == reg.LoginModel.Email && userlist.Password == reg.LoginModel.Password
                           select new
                           {
                               userlist.UserId,
                               userlist.FirstName,
                               userlist.Email,
                               userlist.Password,
                               userlist.UserTypeId,
                               userlist.IsApproved
                            

                           }).ToList();
            if (details.FirstOrDefault() != null)
            {
                /*if (reg.LoginModel.Rememberme == true) 
                {
                    string key = "EmailCookie";
                    string value = reg.LoginModel.Email.ToString();
                    string key1 = "PasswordCookie";
                    string value1 = reg.LoginModel.Password.ToString();
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Append(key, value, options);
                    Response.Cookies.Append(key1, value1, options);
                    ViewBag.Rememberme = String.Format("Save");
                }*/
                HttpContext.Session.SetInt32("UserTypeId", details.FirstOrDefault().UserTypeId);
                if (details.FirstOrDefault().UserTypeId == 1)
                {
                    HttpContext.Session.SetString("FirstName", details.FirstOrDefault().FirstName);
                    HttpContext.Session.SetInt32("UserId", details.FirstOrDefault().UserId);
                    
                    if (reg.chceckbookorlogin != null)
                    {
                        return RedirectToAction("bookservice", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
                else 
                {
                    if (reg.chceckbookorlogin != null) 
                    {
                        ModelState.AddModelError("Invalid", "Only Customer can access this part. ");
                        ViewBag.Message = String.Format("Invalid Login");
                        return View("~/Views/Home/Index.cshtml");
                    }

                    if (details.FirstOrDefault().IsApproved) 
                    {
                      
                        HttpContext.Session.SetInt32("UserId", details.FirstOrDefault().UserId);
                        HttpContext.Session.SetString("FirstName", details.FirstOrDefault().FirstName);
                        return RedirectToAction("spnewservicerequests", "Home");
                    
                    }
                    else {
                        ModelState.AddModelError("Invalid", "Your Account is not Approved yet");
                        ViewBag.Message = String.Format("Invalid Login");
                        return View("~/Views/Home/Index.cshtml");
                    }
                    
                }
                
                
            }
            else
            {
                if (reg.chceckbookorlogin != null)
                {
                    ModelState.AddModelError("Invalid", "Invalid login");
                    ViewBag.Message1 = String.Format("Invalid Login");
                    return View("~/Views/Home/Index.cshtml");
                }
                else 
                {
                    ModelState.AddModelError("Invalid", "Invalid login");
                    ViewBag.Message = String.Format("Invalid Login");
                    return View("~/Views/Home/Index.cshtml");
                }

                
            }



        }
        [HttpPost]
        public IActionResult forgot(LoginForgot reg)
        {
            var details = IsEmailExists(reg.ForgotModel.Email);
            if (details)
            {
                ViewBag.emailupdatepass = reg.ForgotModel.Email.ToString();
                return View("~/Views/Home/forgotpassword.cshtml");
            }
            else 
            {
                ModelState.AddModelError("invalidemail", "Invalid Email");
                ViewBag.forgot = String.Format("Invalid Email");
                return View("~/Views/Home/Index.cshtml");

            }

        }
        public bool IsEmailExists(string eMail)
        {
            var IsCheck = _db.Users.Where(email => email.Email == eMail).FirstOrDefault();
            return IsCheck != null;
        }

        


    }
}
