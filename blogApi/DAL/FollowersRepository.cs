using blogApi.DTOS.ReadDTO;
using blogApi.Entities;
using blogApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DAL
{
    public class FollowersRepository : BaseRepository<Followers>, IFollowersRepository
    {
        private readonly RepositoryContext context;
        public FollowersRepository(RepositoryContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<Followers> CheckIfFollowerExistAsync(int userToFollowOrUnfollowId, int userThatWantToFollowORUnfollowId)
        {
            var result = await FindByConditionWithTracking(x => x.user_Id == userToFollowOrUnfollowId && x.follower_Id == userThatWantToFollowORUnfollowId).SingleOrDefaultAsync();
            return result;

        }

        public async Task<bool> GetFollowingStatusAsync(int userId, int followersUserId)
        {
            var result = await FindByCondition(x => x.user_Id == userId && x.follower_Id == followersUserId).SingleOrDefaultAsync();

            if(result == null)
            {
                return false;
            }
            else
            {
                if(result.isFollowing == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
    
        public async Task<List<FollowersAndFollowingDTO>> GetFollowersAsync(int userId)
        {
            var result = await context.FollowersAndFollowings.FromSqlRaw("select * from get_followers({0})", userId).ToListAsync();

            return result;
        }

        public async Task<List<FollowersAndFollowingDTO>> GetFollowingsAsync(int userId)
        {
            var result = await context.FollowersAndFollowings.FromSqlRaw("select * from get_followings({0})", userId).ToListAsync();

            return result;
        }
    }
}
