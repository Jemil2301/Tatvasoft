using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    public class AddAddressModel
    {
        [Required(ErrorMessage = "Please enter Streat name")]
        public string AddressLine1 { get; set; }
        [Required(ErrorMessage = "Please enter House number")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Please enter City")]
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Mobile { get; set; }
        
    }
}
