using Helperland.Models;
using Helperland.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModel
{
    public class BookServiceModel:LoginForgot
    {
       
        public ZipcodeModel zipcodeModel { get; set; }
        public ScheduleandplanModel scheduleandplanModel { get; set; }
        public IEnumerable<UserAddress> userAddress { get; set; }
        public AddAddressModel addAddressModel { get; set; }
        public String selectedaddress { get; set; }

        [Required]
        public bool privacy { get; set; }

    }
}
