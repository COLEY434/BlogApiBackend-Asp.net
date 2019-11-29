﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DTOS.WriteDTO
{
    public class UserWriteDTO
    {

        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

    }
}
