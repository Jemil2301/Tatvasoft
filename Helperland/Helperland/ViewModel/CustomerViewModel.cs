using Helperland.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModel
{
    public class CustomerViewModel:LoginForgot
    {
        public IEnumerable<ServiceRequest> serviceRequests { get; set; }
        public IEnumerable<ServiceRequest> sr {get; set;}
        public IEnumerable<ServiceRequestAddress> serviceRequestAddresses { get; set;}
        public IEnumerable<ServiceRequestExtra> serviceRequestExtras { get; set; }
        public IEnumerable<User> users { get; set; }
        public int Cancelrequestid { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public BookServiceModel bookServiceModel { get; set; }
        public Rating rate { get; set; }
        public IEnumerable<Rating> ratings { get; set; }
        public String ontime { get; set; }
        public String friendly { get; set; }
        public String quality { get; set; }
        public IEnumerable<FavoriteAndBlocked> favoriteAndBlockeds { get; set; }
        public int userid { get; set; }
        public int spid { get; set; }

    }
}
