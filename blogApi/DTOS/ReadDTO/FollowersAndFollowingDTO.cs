using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DTOS.ReadDTO
{
    public class FollowersAndFollowingDTO
    {
        public int UserId { get; set; }

        public string Surname { get; set; }

        public string Firstname { get; set; }

        public string Username { get; set; }

        public string ImgUrl { get; set; }

        public DateTime date_joined { get; set; }
    }
}
