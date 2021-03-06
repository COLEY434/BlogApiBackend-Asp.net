﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.Entities
{
    public class posts
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int user_id { get; set; }
         
        [Required]
        public string message { get; set; }

        [Required]
        public DateTime created_at { get; set; }

        public DateTime? updated_at { get; set; }
    }
}
