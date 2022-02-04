using Helperland.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    [NotMapped]
    public partial class UpdatepasswordModel:User
    {
        public int UserId { get; set; }

        public String Email { get; set; }

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
