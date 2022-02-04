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
    public partial class LoginModel:User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(Name ="Remember me")]
        public bool Rememberme { get; set; }

        
    }
}
