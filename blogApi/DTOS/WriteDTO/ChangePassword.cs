using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DTOS.WriteDTO
{
    public class ChangePassword
    {
        [Required]
        public int OldPassword { get; set; }

        [Required]
        public int NewPassword { get; set; }
    }
}
