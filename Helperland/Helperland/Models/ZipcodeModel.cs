using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    [NotMapped]
    public partial class ZipcodeModel
    {
        [Required(ErrorMessage = "Please enter ZipCode")]
        public String zipcode { get; set; }
    }
}
