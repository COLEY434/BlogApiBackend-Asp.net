using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.Entities
{
    public class Followers
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public int user_Id { get; set; }

        [Required]
        public int follower_Id { get; set; }

        public bool isFollowing { get; set; }
    }
}
