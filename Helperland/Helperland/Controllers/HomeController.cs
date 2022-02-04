
using Helperland.Models;
using Helperland.Models.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HelperlandContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomeController(ILogger<HomeController> logger, HelperlandContext db, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _db = db;
            this.webHostEnvironment = webHostEnvironment;

        }

        
     
        public IActionResult Index()
        {
            return View();
        }
      
        public IActionResult Price()
        {
            return View();
        }
      
        public IActionResult Faq() 
        {
            return View();
        }
      
        public IActionResult Aboutus()
        {
            return View();
        }
      
        public IActionResult Contactus()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contactus(ContactusModel model)
        {
            if (ModelState.IsValid) {
                string uniqueFileName = null;
                string filePath = null;
                string filename = null;
                if (model.attachment != null)
                {
                   string uploadsFloder= Path.Combine(webHostEnvironment.WebRootPath,"uploadfile");
                   uniqueFileName= Guid.NewGuid().ToString() + "_" + model.attachment.FileName;
                   filePath= Path.Combine(uploadsFloder, uniqueFileName);
                   model.attachment.CopyTo(new FileStream(filePath, FileMode.Create));
                    filename = model.attachment.FileName.ToString();
                }
                ContactU contactU = new ContactU();
                contactU.Name = model.FirstName.ToString() + " " + model.LastName.ToString();
                contactU.Email = model.Email.ToString();
                contactU.Subject = model.Subject.ToString();
                contactU.PhoneNumber = model.PhoneNumber.ToString();
                contactU.Message = model.Message.ToString();
                contactU.UploadFileName = uniqueFileName;
                contactU.CreatedOn = DateTime.Now;
                contactU.FileName = filename;
               
                _db.ContactUs.Add(contactU);
                _db.SaveChanges();

                ViewBag.Contactussucess = String.Format("Sucess");
                return View("~/Views/Home/Contactus.cshtml");
                

            }
            return View();
        }

        public IActionResult Createaccount() {
            return View();
        }
        [HttpPost]
        public IActionResult Createaccount(RegisterModel reg)
        {
            var details = IsEmailExists(reg.Email);

            if (details)
            {
                
                ModelState.AddModelError("Email", "EmailAddress already in Use");
                return View();
            }
            else 
            {
                User user = new User();
                user.FirstName = reg.FirstName.ToString();
                user.LastName = reg.LastName.ToString();
                user.Password = reg.Password.ToString();
                user.Email = reg.Email.ToString();
                user.Mobile = reg.Mobile.ToString();
                user.UserTypeId = 1;
                user.WorksWithPets = false;
                user.CreatedDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = 1;
                user.IsApproved = true;
                _db.Users.Add(user);
                _db.SaveChanges();

                return RedirectToAction("Index");


            }
            
        }

        public IActionResult Welcome()
        {
            return View();
        }

        [HttpPost]
        public IActionResult forgotpassword(UpdatepasswordModel reg)
        {
            User user = _db.Users.Where(x => x.Email == reg.Email).FirstOrDefault();
            user.Password = reg.Password.ToString();
            _db.Users.Update(user);
            _db.SaveChanges();
            ViewBag.passwordchangesucess = String.Format("Sucess");
            
            return View("~/Views/Home/Index.cshtml");
        }

        public IActionResult BecomeaHelper()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BecomeaHelper(RegisterModel reg)
        {
            var details = IsEmailExists(reg.Email);

            if (details)
            {

                ModelState.AddModelError("Email", "EmailAddress already in Use");
                return View();
            }
            else
            {
                User user = new User();
                user.FirstName = reg.FirstName.ToString();
                user.LastName = reg.LastName.ToString();
                user.Password = reg.Password.ToString();
                user.Email = reg.Email.ToString();
                user.Mobile = reg.Mobile.ToString();
                user.UserTypeId = 2;
                user.WorksWithPets = false;
                user.CreatedDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = 2;
                user.IsApproved = false;
                _db.Users.Add(user);
                _db.SaveChanges();

                return RedirectToAction("Index");


            }

        }



        public bool IsEmailExists(string eMail)
        {
            var IsCheck = _db.Users.Where(email => email.Email == eMail).FirstOrDefault();
            return IsCheck != null;
        }


    }
}
