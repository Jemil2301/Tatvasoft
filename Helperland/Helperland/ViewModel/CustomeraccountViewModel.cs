using Helperland.Models;
using Helperland.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModel
{
    public class CustomeraccountViewModel : LoginForgot
    {
        public User user { get; set; }
        public IEnumerable<UserAddress> userAddresses { get; set; }
        
        public int deleteadd { get; set;}
        public string fname { get; set; }
        public string lname { get; set; }
        public string phone { get; set; }
        public string day { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string lang { get; set; }



        [Required(ErrorMessage = "Please enter Streat name")]
        public string AddressLine1 { get; set; }
        [Required(ErrorMessage = "Please enter House number")]
        public string AddressLine2 { get; set; }
        [Required(ErrorMessage = "Please enter City")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter PostalCode")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Please enter Mobile number")]
        public string Mobile { get; set; }
        public int addid { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public String oldpass { get; set;}
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{8,10}$", ErrorMessage = "8-10 characters,1 Alphabet, 1 Number, 1 Special Character")]
        public String Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [DataType(DataType.Password)]
        public String Confirmpwd { get; set; }
        
    }
}
