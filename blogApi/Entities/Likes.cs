using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.Entities
{
    public class Likes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int user_Id { get; set; }

        [Required]
        public int post_Id { get; set; }

        public bool liked { get; set; }

    }
}
