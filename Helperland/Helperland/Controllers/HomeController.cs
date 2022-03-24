
using Helperland.Models;
using Helperland.Models.Data;
using Helperland.ViewModel;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
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
                                 where UserAddress.UserId == HttpContext.Session.GetInt32("UserId") && UserAddress.IsDeleted !=true && UserAddress.PostalCode== bookServiceModel.zipcodeModel.zipcode
                                 select UserAddress;
                var city = (from userlist in _db.UserAddresses
                               where userlist.PostalCode == bookServiceModel.zipcodeModel.zipcode
                               select new
                               {
                                   userlist.City,
                               }).ToList();
                ViewBag.zipcodematch = String.Format("sucess");
                ViewBag.zipcodepass = bookServiceModel.zipcodeModel.zipcode;
                ViewBag.ct = city.FirstOrDefault();
                if (ViewBag.ct != null) 
                {
                    ViewBag.city = city.FirstOrDefault().City;
                }
                
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
       
        public IActionResult Dashboard() 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                CustomerViewModel bs = new CustomerViewModel();
                bs.serviceRequests = from ServiceRequest in _db.ServiceRequests
                                 where ServiceRequest.UserId == HttpContext.Session.GetInt32("UserId") && ServiceRequest.Status !=1 && ServiceRequest.Status !=2
                                 select ServiceRequest;
                bs.serviceRequestAddresses = from serviceRequestAddresses in _db.ServiceRequestAddresses
                                             select serviceRequestAddresses;
                bs.serviceRequestExtras = from serviceRequestExtras in _db.ServiceRequestExtras
                                          select serviceRequestExtras;
                bs.users = from users in _db.Users select users;
                bs.ratings = from ratings in _db.Ratings select ratings;

                return View("Dashboard",bs);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult cancelrequest(CustomerViewModel customerViewModel) 
        {
            ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == customerViewModel.
            Cancelrequestid).FirstOrDefault();
            /*ServiceRequestAddress serviceRequestAddress = _db.ServiceRequestAddresses.Where(x => x.ServiceRequestId == customerViewModel.
            Cancelrequestid).FirstOrDefault();
            while (_db.ServiceRequestExtras.Where(x => x.ServiceRequestId == customerViewModel.
            Cancelrequestid).FirstOrDefault() != null) 
            {
              ServiceRequestExtra serviceRequestExtra = _db.ServiceRequestExtras.Where(x => x.ServiceRequestId == customerViewModel.
            Cancelrequestid).FirstOrDefault();
                _db.ServiceRequestExtras.Remove(serviceRequestExtra);
                _db.SaveChanges();
            }
            if (serviceRequestAddress != null)
            {
                _db.ServiceRequestAddresses.Remove(serviceRequestAddress);
                _db.SaveChanges();
            }*/
            serviceRequest.Status = 2;
            _db.ServiceRequests.Update(serviceRequest);
            _db.SaveChanges();
           
            return RedirectToAction("Dashboard");

        }
        [HttpPost]
        public IActionResult updaterequest(CustomerViewModel customerViewModel) 
        {
            if (customerViewModel.spid == 0)
            {
                ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == customerViewModel.
            Cancelrequestid).FirstOrDefault();
                serviceRequest.ServiceStartDate = DateTime.Parse(customerViewModel.date + " " + customerViewModel.time);
                _db.ServiceRequests.Update(serviceRequest);
                _db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else 
            {
                var x = from ServiceRequest in _db.ServiceRequests
                        where ServiceRequest.ServiceProviderId == customerViewModel.spid
                        && ServiceRequest.Status == null && ServiceRequest.ServiceRequestId != customerViewModel.Cancelrequestid
                        select ServiceRequest;
                var mstartdate = DateTime.Parse(customerViewModel.date + " " + customerViewModel.time);
                var menddate = mstartdate.AddHours(customerViewModel.srhr);
                var j = 0;
                foreach (var servicedatecheck in x)
                {
                    var endtimefromdb = servicedatecheck.ServiceStartDate.AddHours(servicedatecheck.ServiceHours);
                    if (endtimefromdb >= mstartdate && endtimefromdb < menddate)
                    {
                        j = 1;
                    }

                    else if (menddate >= servicedatecheck.ServiceStartDate && mstartdate < endtimefromdb)
                    {
                        j = 1;
                    }
                    else if (mstartdate >= servicedatecheck.ServiceStartDate && menddate <= endtimefromdb)
                    {
                        j = 1;
                    }
                    else if (servicedatecheck.ServiceStartDate >= mstartdate && endtimefromdb <= menddate)
                    {
                        j = 1;
                    }
                    if (j == 1)
                    {
                        TempData["date"] = servicedatecheck.ServiceStartDate.ToShortDateString();
                        TempData["s"] = servicedatecheck.ServiceStartDate.ToString("HH:mm");
                        TempData["e"] = servicedatecheck.ServiceStartDate.AddHours(servicedatecheck.ServiceHours).ToString("HH:mm");
                        break;
                    }
                }
                if (j == 0)
                {
                    ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == customerViewModel.
             Cancelrequestid).FirstOrDefault();
                    serviceRequest.ServiceStartDate = DateTime.Parse(customerViewModel.date + " " + customerViewModel.time);
                    _db.ServiceRequests.Update(serviceRequest);
                    _db.SaveChanges();
                    return RedirectToAction("Dashboard");

                }
                else if (j == 1)
                {
                    TempData["conflict11"] = "conflict";

                }
                return RedirectToAction("Dashboard");
            }
        }

        public IActionResult Servicehistory() 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                CustomerViewModel bs = new CustomerViewModel();
                bs.serviceRequests = from ServiceRequest in _db.ServiceRequests
                                     where ServiceRequest.UserId == HttpContext.Session.GetInt32("UserId") &&( ServiceRequest.Status == 1 || ServiceRequest.Status == 2)
                                     select ServiceRequest;
                bs.serviceRequestAddresses = from serviceRequestAddresses in _db.ServiceRequestAddresses
                                             select serviceRequestAddresses;
                bs.serviceRequestExtras = from serviceRequestExtras in _db.ServiceRequestExtras
                                          select serviceRequestExtras;

                bs.users = from users in _db.Users select users;
                bs.ratings = from ratings in _db.Ratings select ratings;
                return View("Servicehistory", bs);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult rateservicep(CustomerViewModel customerViewModel) 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                Rating rating = new Rating();
                var rateavg = (decimal.Parse(customerViewModel.friendly) + decimal.Parse(customerViewModel.ontime) + decimal.Parse(customerViewModel.quality)) / 3;
                rating.ServiceRequestId = customerViewModel.rate.ServiceRequestId;
                rating.RatingFrom = customerViewModel.rate.RatingFrom;
                rating.RatingTo = customerViewModel.rate.RatingTo;
                rating.Ratings = rateavg;
                rating.Comments = customerViewModel.rate.Comments;
                rating.RatingDate= DateTime.Now;
                rating.OnTimeArrival = decimal.Parse(customerViewModel.ontime);
                rating.Friendly = decimal.Parse(customerViewModel.friendly);
                rating.QualityOfService =decimal.Parse( customerViewModel.quality);
                _db.Ratings.Add(rating);
                _db.SaveChanges();

               
                return RedirectToAction("Servicehistory");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult customerprofile ()
        {

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                CustomeraccountViewModel ca = new CustomeraccountViewModel();
                var userinfo= (from User in _db.Users
                         where User.UserId== HttpContext.Session.GetInt32("UserId")
                               select new
                               {
                                   User.FirstName,
                                   User.LastName,
                                   User.Mobile,
                                   User.DateOfBirth,
                                   User.Email,
                                   User.LanguageId
                               }).ToList();
                ca.userAddresses = from userAddresses in _db.UserAddresses
                                   where userAddresses.UserId == HttpContext.Session.GetInt32("UserId") 
                                   && userAddresses.IsDeleted != true
                                   select userAddresses;
                if (userinfo.FirstOrDefault() != null)
                {
                    ViewBag.cfName = userinfo.FirstOrDefault().FirstName;
                    ViewBag.clName = userinfo.FirstOrDefault().LastName;
                    ViewBag.cMobile = userinfo.FirstOrDefault().Mobile;
                    ViewBag.cDOB = userinfo.FirstOrDefault().DateOfBirth;
                    ViewBag.cEmail = userinfo.FirstOrDefault().Email;
                    ViewBag.cLanguageid = userinfo.FirstOrDefault().LanguageId;
                    return View(ca);
                }
                    return View(ca);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult updatecprofile(CustomeraccountViewModel model) 
        {
            User u = _db.Users.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            u.FirstName = model.fname;
            u.LastName = model.lname;
            u.Mobile = model.phone;
            u.ModifiedDate= DateTime.Now;
            if (model.day != null && model.month != null && model.year != null)
            {
                u.DateOfBirth = DateTime.Parse(model.day + "-" + model.month + "-" + model.year);
            }

            u.LanguageId = int.Parse(model.lang);
            _db.Users.Update(u);

            _db.SaveChanges();
            HttpContext.Session.SetString("FirstName", model.fname);
            TempData["ups"] = "Bill"; 
            return RedirectToAction("customerprofile");

        }
        [HttpPost]
        public IActionResult DeleteAddress(CustomeraccountViewModel model) 
        {
            UserAddress userAddress=_db.UserAddresses.Where(x => x.AddressId == model.deleteadd).FirstOrDefault();
            userAddress.IsDeleted = true;
            _db.UserAddresses.Update(userAddress);
            _db.SaveChanges();
            TempData["deladd"] = "Bill";
            return RedirectToAction("customerprofile");

        }
        [HttpPost]
        public IActionResult AddCustomeraddress(CustomeraccountViewModel model) 
        {
            UserAddress u = new UserAddress();
            u.UserId = (int)HttpContext.Session.GetInt32("UserId");
            u.AddressLine1 = model.AddressLine1.ToString();
            u.AddressLine2 = model.AddressLine2.ToString();
            u.City = model.City.ToString();
            u.PostalCode = model.PostalCode.ToString();
            u.Mobile = model.Mobile.ToString();
            u.IsDefault = false;
            u.IsDeleted = false;
            _db.UserAddresses.Add(u);
            _db.SaveChanges();
            TempData["addadd"] = "Bill";
            return RedirectToAction("customerprofile");
        }
        [HttpPost]
        public IActionResult UpdateCustomeraddress(CustomeraccountViewModel model) 
        {
            UserAddress userAddress= _db.UserAddresses.Where(x => x.AddressId == model.addid).FirstOrDefault();
            
            userAddress.AddressLine1 = model.AddressLine1.ToString();
            userAddress.AddressLine2 = model.AddressLine2.ToString();
            userAddress.City = model.City.ToString();
            userAddress.PostalCode = model.PostalCode.ToString();
            userAddress.Mobile = model.Mobile.ToString();
            _db.UserAddresses.Update(userAddress);
            _db.SaveChanges();
            TempData["updateadd"] = "Bill";
            return RedirectToAction("customerprofile");
        }
        [HttpPost]
        public IActionResult ChangePassword(CustomeraccountViewModel model) 
        {
            User user= _db.Users.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            if (user.Password == model.oldpass)
            {
                user.Password = model.Password.ToString();
                _db.Users.Update(user);
                _db.SaveChanges();
                TempData["Passwordchangesuccess"] = "Bill";
                return RedirectToAction("customerprofile");
            }
            else 
            {
               
                TempData["Passwordchangesuccess1"] = "Bill";
                return RedirectToAction("customerprofile");
               
            }
        }
        public IActionResult Favouritepros() 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                CustomerViewModel model = new CustomerViewModel();
                model.serviceRequests = from s in _db.ServiceRequests
                                        where s.Status == 1 && s.UserId == HttpContext.Session.GetInt32("UserId")
                                        select s;
                model.users = from s in _db.Users
                              where s.UserTypeId == 2
                              select s;
                model.ratings = from s in _db.Ratings
                                select s;
                model.sr = from s in _db.ServiceRequests
                           where s.Status==1
                           select s;
                model.favoriteAndBlockeds = from favoriteAndBlockeds in _db.FavoriteAndBlockeds
                                            where favoriteAndBlockeds.UserId == HttpContext.Session.GetInt32("UserId")
                                            select favoriteAndBlockeds;
                model.favoriteAndBlockeds1 = from favoriteAndBlockeds in _db.FavoriteAndBlockeds
                                            where favoriteAndBlockeds.TargetUserId == HttpContext.Session.GetInt32("UserId")
                                            select favoriteAndBlockeds;

                return View(model);
            }

            else 
            {
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public IActionResult unfavorite(CustomerViewModel m)
        {
            FavoriteAndBlocked fb = _db.FavoriteAndBlockeds.Where(x => x.UserId == m.userid && x.TargetUserId == m.spid).FirstOrDefault();
            if (fb != null)
            {

                fb.IsFavorite = false;
                fb.IsBlocked = false;
                _db.FavoriteAndBlockeds.Update(fb);
                _db.SaveChanges();
            }
            return RedirectToAction("Favouritepros");
        }
        [HttpPost]
        public IActionResult makeFavorite(CustomerViewModel m)
        {


            FavoriteAndBlocked fb = _db.FavoriteAndBlockeds.Where(x => x.UserId == m.userid && x.TargetUserId == m.spid).FirstOrDefault();
            if (fb != null)
            {

                fb.IsFavorite = true;
                fb.IsBlocked = false;
                _db.FavoriteAndBlockeds.Update(fb);
                _db.SaveChanges();
            }
            else
            {
                FavoriteAndBlocked fb1 = new FavoriteAndBlocked();
                fb1.UserId = m.userid;
                fb1.TargetUserId = m.spid;
                fb1.IsFavorite = true;
                fb1.IsBlocked = false;
                _db.FavoriteAndBlockeds.Add(fb1);
                _db.SaveChanges();
            }

            return RedirectToAction("Favouritepros");
        }
        [HttpPost]
        public IActionResult makeblock(CustomerViewModel m)
        {
            FavoriteAndBlocked fb = _db.FavoriteAndBlockeds.Where(x => x.UserId == m.userid && x.TargetUserId == m.spid).FirstOrDefault();
            if (fb != null)
            {

                fb.IsFavorite = false;
                fb.IsBlocked = true;
                _db.FavoriteAndBlockeds.Update(fb);
                _db.SaveChanges();
            }
            else
            {
                FavoriteAndBlocked fb1 = new FavoriteAndBlocked();
                fb1.UserId = m.userid;
                fb1.TargetUserId = m.spid;
                fb1.IsFavorite = false;
                fb1.IsBlocked = true;
                _db.FavoriteAndBlockeds.Add(fb1);
                _db.SaveChanges();
            }

            return RedirectToAction("Favouritepros");
        }

        public IActionResult spnewservicerequests()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                
                var x = from User in _db.Users
                        where User.UserId == HttpContext.Session.GetInt32("UserId")
                        select User.ZipCode;
                SPViewModel sp = new SPViewModel();
                sp.serviceRequests = from serviceRequests in _db.ServiceRequests
                                     where serviceRequests.ZipCode == x.FirstOrDefault() && serviceRequests.Status == null && serviceRequests.ServiceProviderId == null
                                     && serviceRequests.ServiceStartDate > DateTime.Now
                                     select serviceRequests;
                sp.serviceRequestswithoutpets= from serviceRequests in _db.ServiceRequests
                                               where serviceRequests.ZipCode == x.FirstOrDefault() && serviceRequests.Status==null && serviceRequests.HasPets==false && serviceRequests.ServiceProviderId == null
                                               && serviceRequests.ServiceStartDate > DateTime.Now   
                                               select serviceRequests;
                sp.serviceRequestAddresses= from serviceRequestAddresses in _db.ServiceRequestAddresses
                                            select serviceRequestAddresses;
                sp.serviceRequestExtras = from serviceRequestExtras in _db.ServiceRequestExtras
                                          select serviceRequestExtras;
                sp.favoriteAndBlockeds = from FavoriteAndBlocked in _db.FavoriteAndBlockeds
                                         where FavoriteAndBlocked.TargetUserId== HttpContext.Session.GetInt32("UserId") 
                                         select FavoriteAndBlocked;
                sp.favoriteAndBlockeds1 = from FavoriteAndBlocked in _db.FavoriteAndBlockeds
                                          where FavoriteAndBlocked.UserId == HttpContext.Session.GetInt32("UserId")
                                          select FavoriteAndBlocked;
                sp.users = from User in _db.Users
                           where User.UserTypeId == 1
                           select User;


                return View(sp);
            }
            else 
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult AcceptSr(SPViewModel model) 
        {
            var x = from ServiceRequest in _db.ServiceRequests
                     where ServiceRequest.ServiceProviderId == HttpContext.Session.GetInt32("UserId")
                     && ServiceRequest.Status == null
                     select ServiceRequest;
            
            if (x.ToList() != null)
            {
                var j = 0;
                foreach (var servicedatecheck in x)
                {
                    var endtimefromdb = servicedatecheck.ServiceStartDate.AddHours(servicedatecheck.ServiceHours + 1);
                    if (endtimefromdb >= model.starttime && endtimefromdb < model.endtime)
                    {
                        j = 1;
                    }
                    else if (model.endtime >= servicedatecheck.ServiceStartDate && model.starttime < endtimefromdb)
                    {
                        j = 1;
                    }
                    else if (model.starttime >= servicedatecheck.ServiceStartDate && model.endtime <= endtimefromdb)
                    {
                        j = 1;
                    }
                    else if (servicedatecheck.ServiceStartDate >= model.starttime && endtimefromdb <= model.endtime) 
                    {
                        j = 1;
                    }
                    
                }
                if (j == 0)
                {
                    ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == model.srid).FirstOrDefault();
                    serviceRequest.ServiceProviderId = HttpContext.Session.GetInt32("UserId");
                    serviceRequest.SpacceptedDate = DateTime.Now;
                    _db.ServiceRequests.Update(serviceRequest);
                    _db.SaveChanges();
                    return RedirectToAction("spnewservicerequests");
                }
                else 
                {
                    TempData["conflict"] = "conflict";
                    return RedirectToAction("spnewservicerequests");

                }

            }
            else
            {
                ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == model.srid).FirstOrDefault();
                serviceRequest.ServiceProviderId = HttpContext.Session.GetInt32("UserId");
                serviceRequest.SpacceptedDate = DateTime.Now;
                _db.ServiceRequests.Update(serviceRequest);
                _db.SaveChanges();
                return RedirectToAction("spnewservicerequests");
            }
        }

        public IActionResult spupcomingrequests() 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                var x = from User in _db.Users
                        where User.UserId == HttpContext.Session.GetInt32("UserId")
                        select User.ZipCode;
                SPViewModel sp = new SPViewModel();
                sp.serviceRequests = from serviceRequests in _db.ServiceRequests
                                     where serviceRequests.ZipCode == x.FirstOrDefault() && serviceRequests.Status == null && serviceRequests.ServiceProviderId == HttpContext.Session.GetInt32("UserId")
                                     select serviceRequests;
                sp.serviceRequestAddresses = from serviceRequestAddresses in _db.ServiceRequestAddresses
                                             select serviceRequestAddresses;
                sp.serviceRequestExtras = from serviceRequestExtras in _db.ServiceRequestExtras
                                          select serviceRequestExtras;

                sp.users = from User in _db.Users
                           where User.UserTypeId == 1
                           select User;
                return View(sp);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public IActionResult CancleSr(SPViewModel model) 
        {
            ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == model.srid).FirstOrDefault();
            serviceRequest.ServiceProviderId = null ;
            serviceRequest.SpacceptedDate = null;
            _db.ServiceRequests.Update(serviceRequest);
            _db.SaveChanges();
            return RedirectToAction("spupcomingrequests");

        }
        [HttpPost]
        public IActionResult DoneSr(SPViewModel model)
        {
            ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == model.srid).FirstOrDefault();
            serviceRequest.Status = 1;
            _db.ServiceRequests.Update(serviceRequest);
            _db.SaveChanges();
            return RedirectToAction("spupcomingrequests");

        }
        public IActionResult spservicehistory() 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                SPViewModel sp = new SPViewModel();
                sp.serviceRequests = from serviceRequests in _db.ServiceRequests
                                     where serviceRequests.Status == 1 && serviceRequests.ServiceProviderId == HttpContext.Session.GetInt32("UserId")
                                     select serviceRequests;
                sp.serviceRequestAddresses = from serviceRequestAddresses in _db.ServiceRequestAddresses
                                             select serviceRequestAddresses;
                sp.serviceRequestExtras = from serviceRequestExtras in _db.ServiceRequestExtras
                                          select serviceRequestExtras;

                sp.users = from User in _db.Users
                           where User.UserTypeId == 1
                           select User;
                return View(sp);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult spmyratings() 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                SPViewModel sp = new SPViewModel();
                sp.serviceRequests = from serviceRequests in _db.ServiceRequests
                                     where serviceRequests.Status == 1 && serviceRequests.ServiceProviderId == HttpContext.Session.GetInt32("UserId")
                                     select serviceRequests;
                sp.ratings = from Rating in _db.Ratings
                             where Rating.RatingTo == HttpContext.Session.GetInt32("UserId")
                             select Rating;
                sp.users = from User in _db.Users
                           where User.UserTypeId == 1
                           select User;
                return View(sp);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult spblockcustomer() 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                SPViewModel sp = new SPViewModel();
                sp.serviceRequests = from serviceRequests in _db.ServiceRequests
                                     where serviceRequests.Status == 1 && serviceRequests.ServiceProviderId == HttpContext.Session.GetInt32("UserId")
                                     select serviceRequests;
                
                sp.users = from User in _db.Users
                           where User.UserTypeId == 1
                           select User;
                sp.favoriteAndBlockeds = from favoriteAndBlockeds in _db.FavoriteAndBlockeds
                                            where favoriteAndBlockeds.UserId == HttpContext.Session.GetInt32("UserId")
                                            select favoriteAndBlockeds;
                return View(sp);
            }
            else
            {
                return RedirectToAction("Index");
            }


        }
        [HttpPost]
        public IActionResult blockcust(SPViewModel model) 
        {
            FavoriteAndBlocked fb = _db.FavoriteAndBlockeds.Where(x => x.UserId == model.spid && x.TargetUserId == model.userid).FirstOrDefault();
            if (fb != null)
            {

                fb.IsFavorite = false;
                fb.IsBlocked = true;
                _db.FavoriteAndBlockeds.Update(fb);
                _db.SaveChanges();
            }
            else
            {
                FavoriteAndBlocked fb1 = new FavoriteAndBlocked();
                fb1.UserId = model.spid;
                fb1.TargetUserId = model.userid;
                fb1.IsFavorite = false;
                fb1.IsBlocked = true;
                _db.FavoriteAndBlockeds.Add(fb1);
                _db.SaveChanges();
            }

            return RedirectToAction("spblockcustomer");
        }

        [HttpPost]
        public IActionResult unblockcust(SPViewModel model) 
        {
            FavoriteAndBlocked fb = _db.FavoriteAndBlockeds.Where(x => x.UserId == model.spid && x.TargetUserId == model.userid).FirstOrDefault();
            if (fb != null)
            {

                fb.IsFavorite = false;
                fb.IsBlocked = false;
                _db.FavoriteAndBlockeds.Update(fb);
                _db.SaveChanges();
            }
            return RedirectToAction("spblockcustomer");

        }
        public IActionResult SPprofile()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userinfo = (from User in _db.Users
                                where User.UserId == HttpContext.Session.GetInt32("UserId")
                                select new
                                {
                                    User.FirstName,
                                    User.LastName,
                                    User.Mobile,
                                    User.DateOfBirth,
                                    User.Email,
                                    User.NationalityId,
                                    User.Gender,
                                    User.UserProfilePicture
                                }).ToList();
                var useradd = (from UserAddress in _db.UserAddresses
                               where UserAddress.UserId == HttpContext.Session.GetInt32("UserId")
                               select new
                               {
                                   UserAddress.AddressLine1,
                                   UserAddress.AddressLine2,
                                   UserAddress.City,
                                   UserAddress.PostalCode,
                               }).ToList();
                if (userinfo.FirstOrDefault() != null)
                {
                    ViewBag.sfName = userinfo.FirstOrDefault().FirstName;
                    ViewBag.slName = userinfo.FirstOrDefault().LastName;
                    ViewBag.sMobile = userinfo.FirstOrDefault().Mobile;
                    ViewBag.sDOB = userinfo.FirstOrDefault().DateOfBirth;
                    ViewBag.sEmail = userinfo.FirstOrDefault().Email;
                    ViewBag.sNationalityId = userinfo.FirstOrDefault().NationalityId;
                    ViewBag.sGender = userinfo.FirstOrDefault().Gender;
                    ViewBag.sUserProfilePicture = userinfo.FirstOrDefault().UserProfilePicture;
                    ViewBag.sUserProfilePicture1 = userinfo.FirstOrDefault().UserProfilePicture;
                    if (useradd.FirstOrDefault() != null) 
                    {
                        ViewBag.sAddressLine1 = useradd.FirstOrDefault().AddressLine1;
                        ViewBag.sAddressLine2 = useradd.FirstOrDefault().AddressLine2;
                        ViewBag.sCity = useradd.FirstOrDefault().City;
                        ViewBag.SPostalCode = useradd.FirstOrDefault().PostalCode;
                        return View();
                    }
                    return View();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult SpEditinfo(SPViewModel model) 
        {
            User u = _db.Users.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            u.FirstName = model.fname;
            u.LastName = model.lname;
            u.Mobile = model.phone;
            u.ModifiedDate = DateTime.Now;
            if (model.day != null && model.month != null && model.year != null)
            {
                u.DateOfBirth = DateTime.Parse(model.day + "-" + model.month + "-" + model.year);
            }
            if (model.natid != null) 
            {
                u.NationalityId = int.Parse(model.natid);
            }
            u.UserProfilePicture = model.profiledp;
            u.Gender = model.gender;
            u.ZipCode = model.postal;
            _db.Users.Update(u);
            _db.SaveChanges();
            UserAddress userAddress =_db.UserAddresses.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            if (userAddress != null)
            {
                userAddress.AddressLine1 = model.add1;
                userAddress.AddressLine2 = model.add2;
                userAddress.City = model.city;
                userAddress.PostalCode = model.postal;
                _db.UserAddresses.Update(userAddress);
                _db.SaveChanges();
            }
            else 
            {
                UserAddress userAddress1 = new UserAddress();
                userAddress1.UserId =(int)HttpContext.Session.GetInt32("UserId");
                userAddress1.AddressLine1 = model.add1;
                userAddress1.AddressLine2 = model.add2;
                userAddress1.City = model.city;
                userAddress1.PostalCode = model.postal;
                userAddress1.IsDefault = true;
                userAddress1.IsDeleted = false;
                _db.UserAddresses.Add(userAddress1);
                _db.SaveChanges();
            }

            HttpContext.Session.SetString("FirstName", model.fname);
            TempData["ups"] = "Bill";
            return RedirectToAction("SPprofile");
        }
        [HttpPost]
        public IActionResult ChangePasswordsp(SPViewModel model)
        {
            User user = _db.Users.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            if (user.Password == model.oldpass)
            {
                user.Password = model.Password.ToString();
                _db.Users.Update(user);
                _db.SaveChanges();
                TempData["Passwordchangesuccess"] = "Bill";
                return RedirectToAction("SPprofile");
            }
            else
            {

                TempData["Passwordchangesuccess1"] = "Bill";
                return RedirectToAction("SPprofile");

            }
        }

        public IActionResult Usermanagement()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                AdminViewModel adminView = new AdminViewModel();
                adminView.user = from users in _db.Users
                                 where users.UserTypeId != 3
                                 select users;

                return View(adminView);

            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Activate(AdminViewModel model) 
        {
            User user = _db.Users.Where(x => x.UserId == model.usrid).FirstOrDefault();
            user.IsApproved = true;
            user.ModifiedBy = 3;
            user.ModifiedDate = DateTime.Now;
            _db.Users.Update(user);
            _db.SaveChanges();
            return RedirectToAction("Usermanagement");

        }
        [HttpPost]
        public IActionResult Deactivate(AdminViewModel model)
        {
            User user = _db.Users.Where(x => x.UserId == model.usrid).FirstOrDefault();
            user.IsApproved = false;
            user.ModifiedBy = 3;
            user.ModifiedDate = DateTime.Now;
            _db.Users.Update(user);
            _db.SaveChanges();
            return RedirectToAction("Usermanagement");

        }
        public IActionResult Servicerequests() 
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                AdminViewModel adminView = new AdminViewModel();
                adminView.user = from users in _db.Users
                                 where users.UserTypeId != 3
                                 select users;
                adminView.serviceRequests = from serviceRequests in _db.ServiceRequests
                                            select serviceRequests;
                adminView.serviceRequestAddresses = from serviceRequestAddresses in _db.ServiceRequestAddresses
                                                    select serviceRequestAddresses;
                adminView.ratings = from ratings in _db.Ratings select ratings;


                return View(adminView);

            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult updatesradmin(AdminViewModel model) 
        {
            ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == model.Srid).FirstOrDefault();
            ServiceRequestAddress serviceRequestAddress = _db.ServiceRequestAddresses.Where(x => x.ServiceRequestId == model.Srid).FirstOrDefault();
            serviceRequest.ServiceStartDate = DateTime.Parse(model.date1 + " " + model.time1);
            serviceRequest.ModifiedBy = 3;
            serviceRequest.ModifiedDate = DateTime.Now;
            serviceRequestAddress.AddressLine1 = model.Add1.ToString();
            serviceRequestAddress.AddressLine2 = model.Add2.ToString();
            serviceRequestAddress.City = model.City.ToString();
            serviceRequestAddress.PostalCode = model.zipcode;
            _db.ServiceRequests.Update(serviceRequest);
            _db.ServiceRequestAddresses.Update(serviceRequestAddress);
            _db.SaveChanges();
            if (serviceRequest.ServiceProviderId != null)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Test Project", "demoprojectspm@gmail.com"));
                message.To.Add(new MailboxAddress(model.username, model.useremail));
                message.Subject = "Admin Reschedule Service Request";
                message.Body = new TextPart("plain")
                {
                    Text = "Admin Reschedule Service Request   ServiceID:" + model.Srid1
                };
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("demoprojectspm@gmail.com", "jemil12345");
                    client.Send(message);
                    client.Disconnect(true);
                }
                var message1 = new MimeMessage();
                message1.From.Add(new MailboxAddress("Test Project", "demoprojectspm@gmail.com"));
                message1.To.Add(new MailboxAddress(model.spname, model.spemail));
                message1.Subject = "Admin Reschedule Service Request ";
                message1.Body = new TextPart("plain")
                {
                    Text = "Admin Reschedule Service Request   ServiceID:" + model.Srid1
                };
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("demoprojectspm@gmail.com", "jemil12345");
                    client.Send(message1);
                    client.Disconnect(true);
                }
                return RedirectToAction("Servicerequests");

            }
            else
            {
                var message2 = new MimeMessage();
                message2.From.Add(new MailboxAddress("Test Project", "demoprojectspm@gmail.com"));
                message2.To.Add(new MailboxAddress(model.username, model.useremail));
                message2.Subject = "Admin Reschedule Service Request";
                message2.Body = new TextPart("plain")
                {
                    Text = "Admin Reschedule Service Request   ServiceID:" + model.Srid1
                };
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("demoprojectspm@gmail.com", "jemil12345");
                    client.Send(message2);
                    client.Disconnect(true);
                }
                return RedirectToAction("Servicerequests");
            }

        }
        [HttpPost]
        public IActionResult cancelsrfromadmin(AdminViewModel model) 
        {
            ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == model.Srid).FirstOrDefault();
            serviceRequest.Status = 2;
            serviceRequest.ModifiedBy = 3;
            serviceRequest.ModifiedDate = DateTime.Now;
            _db.ServiceRequests.Update(serviceRequest);
            _db.SaveChanges();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Test Project", "demoprojectspm@gmail.com"));
            message.To.Add(new MailboxAddress(model.username, model.useremail));
            message.Subject = "Your Service Request is Cancelled";
            message.Body = new TextPart("plain")
            {
                Text = "Admin Cancel Your Service Request ServiceID:" + model.Srid1
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("demoprojectspm@gmail.com", "jemil12345");
                client.Send(message);
                client.Disconnect(true);
            }
            return RedirectToAction("Servicerequests");

        }
        [HttpPost]
        public IActionResult cancelspfromadmin(AdminViewModel model)
        {
            ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == model.Srid).FirstOrDefault();
            serviceRequest.ModifiedBy = 3;
            serviceRequest.ModifiedDate = DateTime.Now;
            serviceRequest.ServiceProviderId = null;
            serviceRequest.SpacceptedDate = null;
            _db.ServiceRequests.Update(serviceRequest);
            _db.SaveChanges();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Test Project", "demoprojectspm@gmail.com"));
            message.To.Add(new MailboxAddress(model.username,model.useremail));
            message.Subject = "Admin deallocated Service Provider";
            message.Body = new TextPart("plain")
            {
                Text = "Admin deallocated Service Provider for ServiceRequest ID:" + model.Srid1
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("demoprojectspm@gmail.com", "jemil12345");
                client.Send(message);
                client.Disconnect(true);
            }
            var message1 = new MimeMessage();
            message1.From.Add(new MailboxAddress("Test Project", "demoprojectspm@gmail.com"));
            message1.To.Add(new MailboxAddress(model.spname, model.spemail));
            message1.Subject = "Admin deallocated you for service ";
            message1.Body = new TextPart("plain")
            {
                Text = "Admin deallocated you for ServiceRequest ID:" + model.Srid1
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("demoprojectspm@gmail.com", "jemil12345");
                client.Send(message1);
                client.Disconnect(true);
            }
            return RedirectToAction("Servicerequests");

        }
        public bool IsEmailExists(string eMail)
        {
            var IsCheck = _db.Users.Where(email => email.Email == eMail).FirstOrDefault();
            return IsCheck != null;
        }



     


    }
}
