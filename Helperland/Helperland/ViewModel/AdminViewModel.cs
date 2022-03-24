using Helperland.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModel
{
    public class AdminViewModel : LoginForgot
    {
        public IEnumerable<User> user { get; set; }
        public IEnumerable<ServiceRequest> serviceRequests { get; set; }
        public IEnumerable<ServiceRequestAddress> serviceRequestAddresses { get; set; }
        public IEnumerable<Rating> ratings { get; set; }
        public int usrid { get; set; }
        public String date1 { get; set; }
        public String time1 { get; set; }
        [Required(ErrorMessage = "Please enter Street name")]
        public String Add1 { get; set; }
        [Required(ErrorMessage = "Please enter House number")]
        public String Add2 { get; set; }
        [Required(ErrorMessage = "Please enter PostalCode")]
        public String zipcode { get; set; }
        [Required(ErrorMessage = "Please enter City")]
        public String City { get; set; }
        public int Srid { get; set; }
        public int Srid1 { get; set; }
        public string useremail{get;set;}
        public string spemail { get; set; }
        public string username { get; set; }
        public string spname { get; set; }
    }
}
