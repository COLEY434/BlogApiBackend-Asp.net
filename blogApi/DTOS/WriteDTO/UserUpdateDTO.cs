using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DTOS.WriteDTO
{
    public class UserUpdateDTO
    {
       
        public IFormFile Photo { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Country { get; set; }



    }
}
