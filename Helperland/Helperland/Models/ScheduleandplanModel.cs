using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    public partial class ScheduleandplanModel
    {
        public string date { get;set;}
        public string time { get; set; }
        public string serviceHrs { get; set; }
        public string extraHrs { get; set; }
        public string subtotal { get; set; }
        public string totalcost { get; set; }
        public string comments { get; set; }
        public bool haspets { get; set; }
        
        public bool one { get; set; }
        public bool two { get; set; }
        public bool three { get; set; }
        public bool four { get; set; }
        public bool five { get; set; }
    }
}
