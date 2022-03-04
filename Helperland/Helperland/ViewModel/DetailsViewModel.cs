using Helperland.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModel
{
    public partial class DetailsViewModel
    {
        public ServiceRequest serviceRequests { get; set; }
        public ServiceRequestAddress serviceRequestAddresses { get; set; }
        public ServiceRequestExtra serviceRequestExtras { get; set; }
    }
}
