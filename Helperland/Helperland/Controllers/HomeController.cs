
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
            ServiceRequest serviceRequest = _db.ServiceRequests.Where(x => x.ServiceRequestId == customerViewModel.
            Cancelrequestid).FirstOrDefault();
            serviceRequest.ServiceStartDate = DateTime.Parse(customerViewModel.date + " " + customerViewModel.time);
            _db.ServiceRequests.Update(serviceRequest);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
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
                           select s;
                model.favoriteAndBlockeds = from favoriteAndBlockeds in _db.FavoriteAndBlockeds
                                            where favoriteAndBlockeds.UserId == HttpContext.Session.GetInt32("UserId")
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




        public bool IsEmailExists(string eMail)
        {
            var IsCheck = _db.Users.Where(email => email.Email == eMail).FirstOrDefault();
            return IsCheck != null;
        }



     


    }
}
