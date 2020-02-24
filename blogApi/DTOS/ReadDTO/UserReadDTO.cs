using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DTOS.ReadDTO
{
    public class UserReadDTO
    {
        public int userId { get; set; }

        public string surname { get; set; }

        public string firstname { get; set; }

        public string username { get; set; }

        public string gender { get; set; }

        public string state { get; set; }

        public DateTime date_joined { get; set; }

        public string country { get; set; }

        public int? age { get; set; }

        public string email { get; set; }

        public string img_url { get; set; }

    }
}
