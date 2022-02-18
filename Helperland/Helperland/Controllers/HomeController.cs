
using Helperland.Models;
using Helperland.Models.Data;
using Helperland.ViewModel;
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
                    string uploadsFloder = Path.Combine(webHostEnvironment.WebRootPath, "uploadfile");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.attachment.FileName;
                    filePath = Path.Combine(uploadsFloder, uniqueFileName);
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
        public IActionResult bookservice() {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult bookservice1(BookServiceModel model) 
        {
            var rand = new Random();
            var uid = rand.Next(1000, 9999);

            ServiceRequest serviceRequest = new ServiceRequest();
            serviceRequest.UserId=(int)HttpContext.Session.GetInt32("UserId");
            serviceRequest.ServiceId = uid;
            serviceRequest.ServiceStartDate= DateTime.Parse(model.scheduleandplanModel.date + " " + model.scheduleandplanModel.time);
            serviceRequest.ZipCode = model.zipcodeModel.zipcode.ToString();
            serviceRequest.ServiceHourlyRate = 18;
            serviceRequest.ServiceHours =float.Parse( model.scheduleandplanModel.serviceHrs);
            if (model.scheduleandplanModel.extraHrs != null) 
            {
                serviceRequest.ExtraHours = float.Parse(model.scheduleandplanModel.extraHrs);
            }
            serviceRequest.SubTotal = int.Parse(model.scheduleandplanModel.subtotal);
            serviceRequest.Discount = 20;
            serviceRequest.TotalCost = int.Parse(model.scheduleandplanModel.totalcost);
            if (model.scheduleandplanModel.comments != null) 
            {
                serviceRequest.Comments = model.scheduleandplanModel.comments.ToString();
            }
            serviceRequest.PaymentDue = false;
            serviceRequest.HasPets = model.scheduleandplanModel.haspets;
            serviceRequest.CreatedDate = DateTime.Now;
            serviceRequest.ModifiedDate = DateTime.Now;
            serviceRequest.Distance = 0;
            _db.ServiceRequests.Add(serviceRequest);
            _db.SaveChanges();
            var oldadd = (from userlist in _db.UserAddresses
                          where userlist.AddressId == int.Parse(model.selectedaddress)
                          select new
                          {
                              userlist.AddressLine1,
                              userlist.AddressLine2,
                              userlist.City,
                              userlist.State,
                              userlist.PostalCode,
                              userlist.Mobile,
                              userlist.Email
                          }).ToList();
            var srviceid = (from userlist in _db.ServiceRequests
                            where userlist.ServiceId == uid
                            select new
                            {
                                userlist.ServiceRequestId
                            }).ToList();
            if (oldadd.FirstOrDefault() != null) 
            {
               
                ServiceRequestAddress serviceRequestAddress = new ServiceRequestAddress();
                serviceRequestAddress.ServiceRequestId = srviceid.FirstOrDefault().ServiceRequestId;
                serviceRequestAddress.AddressLine1 = oldadd.FirstOrDefault().AddressLine1;
                serviceRequestAddress.AddressLine2 = oldadd.FirstOrDefault().AddressLine2;
                serviceRequestAddress.City = oldadd.FirstOrDefault().City;
                serviceRequestAddress.State = oldadd.FirstOrDefault().State;
                serviceRequestAddress.PostalCode = oldadd.FirstOrDefault().PostalCode;
                serviceRequestAddress.Mobile = oldadd.FirstOrDefault().Mobile;
                serviceRequestAddress.Email = oldadd.FirstOrDefault().Email;
                _db.ServiceRequestAddresses.Add(serviceRequestAddress);
                _db.SaveChanges();


            }
            else 
            {
                    ServiceRequestAddress serviceRequestAddress = new ServiceRequestAddress();
                    serviceRequestAddress.ServiceRequestId = srviceid.FirstOrDefault().ServiceRequestId;
                    serviceRequestAddress.AddressLine1 = model.addAddressModel.AddressLine1.ToString();
                    serviceRequestAddress.AddressLine2 = model.addAddressModel.AddressLine2.ToString();
                    serviceRequestAddress.City = model.addAddressModel.City.ToString();
                    serviceRequestAddress.PostalCode = model.addAddressModel.PostalCode.ToString();
                    if (model.addAddressModel.Mobile != null)
                    {
                        serviceRequestAddress.Mobile = model.addAddressModel.Mobile.ToString();
                    }
                    _db.ServiceRequestAddresses.Add(serviceRequestAddress);
                    _db.SaveChanges();
                    UserAddress userAddress = new UserAddress();
                    userAddress.UserId= (int)HttpContext.Session.GetInt32("UserId");
                    userAddress.AddressLine1 = model.addAddressModel.AddressLine1.ToString();
                    userAddress.AddressLine2 = model.addAddressModel.AddressLine2.ToString();
                    userAddress.City = model.addAddressModel.City.ToString();
                    userAddress.PostalCode = model.addAddressModel.PostalCode.ToString();
                    userAddress.IsDefault = false;
                    userAddress.IsDeleted = false;
                    if (model.addAddressModel.Mobile != null)
                    {
                        userAddress.Mobile = model.addAddressModel.Mobile.ToString();
                    }
                    _db.UserAddresses.Add(userAddress);
                    _db.SaveChanges();

            }
            
            if (model.scheduleandplanModel.one == true) 
            {
                ServiceRequestExtra serviceRequestExtra = new ServiceRequestExtra();
                serviceRequestExtra.ServiceExtraId = 1;
                serviceRequestExtra.ServiceRequestId = srviceid.FirstOrDefault().ServiceRequestId;
                _db.ServiceRequestExtras.Add(serviceRequestExtra);
                _db.SaveChanges();
            }
            if (model.scheduleandplanModel.two == true)
            {
                ServiceRequestExtra serviceRequestExtra = new ServiceRequestExtra();
                serviceRequestExtra.ServiceExtraId = 2;
                serviceRequestExtra.ServiceRequestId = srviceid.FirstOrDefault().ServiceRequestId;
                _db.ServiceRequestExtras.Add(serviceRequestExtra);
                _db.SaveChanges();
            }
            if (model.scheduleandplanModel.three == true)
            {
                ServiceRequestExtra serviceRequestExtra = new ServiceRequestExtra();
                serviceRequestExtra.ServiceExtraId = 3;
                serviceRequestExtra.ServiceRequestId = srviceid.FirstOrDefault().ServiceRequestId;
                _db.ServiceRequestExtras.Add(serviceRequestExtra);
                _db.SaveChanges();
            }
            if (model.scheduleandplanModel.four == true)
            {
                ServiceRequestExtra serviceRequestExtra = new ServiceRequestExtra();
                serviceRequestExtra.ServiceExtraId = 4;
                serviceRequestExtra.ServiceRequestId = srviceid.FirstOrDefault().ServiceRequestId;
                _db.ServiceRequestExtras.Add(serviceRequestExtra);
                _db.SaveChanges();
            }
            if (model.scheduleandplanModel.five == true)
            {
                ServiceRequestExtra serviceRequestExtra = new ServiceRequestExtra();
                serviceRequestExtra.ServiceExtraId = 5;
                serviceRequestExtra.ServiceRequestId = srviceid.FirstOrDefault().ServiceRequestId;
                _db.ServiceRequestExtras.Add(serviceRequestExtra);
                _db.SaveChanges();
            }
            ViewBag.serviceid = uid;
            return View("bookservice");
        }

        [HttpPost]
        public IActionResult checkpostalcode(BookServiceModel bookServiceModel) {
            var details = (from userlist in _db.Users
                           where userlist.UserTypeId == 2 && userlist.ZipCode == bookServiceModel.zipcodeModel.zipcode
                           select new
                           {
                               userlist.UserTypeId,
                               userlist.IsApproved,
                           }).ToList();

            if (details.FirstOrDefault() != null)
            {
                BookServiceModel bs = new BookServiceModel();
                bs.userAddress = from UserAddress in _db.UserAddresses
                                 where UserAddress.UserId == HttpContext.Session.GetInt32("UserId")
                                 select UserAddress;
                var city = (from userlist in _db.UserAddresses
                               where userlist.PostalCode == bookServiceModel.zipcodeModel.zipcode
                               select new
                               {
                                   userlist.City,
                               }).ToList();
                ViewBag.zipcodematch = String.Format("sucess");
                ViewBag.zipcodepass = bookServiceModel.zipcodeModel.zipcode;
                ViewBag.city = city.FirstOrDefault().City;
                ViewBag.add1 = "hii";
                return View("bookservice",bs);

            }
            else
            {

                ViewBag.zipcodeunmatch = String.Format("sucess");
                /*return Content("false");*/
                return View("bookservice");

            }


        }
        /* [HttpPost]
         public IActionResult schedule(BookServiceModel model)
         {
             var x = DateTime.Parse(model.scheduleandplanModel.date + " " + model.scheduleandplanModel.time);
             ViewBag.extra = DateTime.Parse(model.scheduleandplanModel.date + " " + model.scheduleandplanModel.time);
             ViewBag.extra1 = x.GetType();


             return View();
         }*/
        public IActionResult logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
       
        public bool IsEmailExists(string eMail)
        {
            var IsCheck = _db.Users.Where(email => email.Email == eMail).FirstOrDefault();
            return IsCheck != null;
        }


    }
}
