using blogApi.Entities;
using blogApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DAL.Post
{
    public class LikeRepository : BaseRepository<Likes>, ILikeRepository
    {
        private readonly RepositoryContext context;
        public LikeRepository(RepositoryContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<List<Likes>> GetLikesAsync()
        {
            var results = await context.likes.ToListAsync();

            return results;
        }

        public async Task<Likes> checkIfLikeExist(int postId, int userId)
        {
            var result = await FindByConditionWithTracking(x => x.post_Id == postId && x.user_Id == userId).SingleOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Likes>> GetPostLikes(int postId)
        {
            var result = await FindByConditionWithTracking(x => x.post_Id == postId).ToListAsync();

            return result;
        }
    }
}
