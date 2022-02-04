using Helperland.Models.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    [NotMapped]
    public class ContactusModel:ContactU

    {
        public int ContactUsId { get; set; }

        [Required(ErrorMessage = "Please enter Firstname")]
        public String FirstName { get; set; }


        [Required(ErrorMessage = "Please enter Lastname")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter Mobile no.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter Message")]
        public string Message { get; set; }

        public IFormFile attachment { get; set; }
        public bool privacy { get; set; }

    }
}
