using blogApi.DTOS.ReadDTO;
using blogApi.Entities;
using blogApi.Interfaces.LoginManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DAL.Login.LoginRepository
{
    public class UsersRepository : BaseRepository<users>, IUsersRepository
    {

        public UsersRepository(RepositoryContext _context):base(_context)
        {

        }

       

        public async Task<IEnumerable<users>> GetAllUsersAsync()
        {
            var result = await FindAll().ToListAsync();

            return result;
        }

        public async Task<UserReadDTO> GetUserById(int id)
        {
            var result = await FindByCondition(x => x.userId == id)
                .Select(x => new UserReadDTO
                {
                    userId = x.userId,
                    surname = x.surname,
                    firstname = x.firstname,
                    username = x.username,
                    gender = x.gender,
                    state = x.state,
                    country = x.country,
                    date_joined = x.created_at,
                    age = x.age,
                    email = x.email,
                    img_url = x.img_url
                }).SingleOrDefaultAsync();
            return result;
        }


        public async Task<users> GetUserByIdT(int id)
        {
            var result = await FindByConditionWithTracking(x => x.userId == id).SingleOrDefaultAsync();

            return result;
        }

      
    }
}
