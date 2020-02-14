using blogApi.DTOS.ReadDTO;
using blogApi.Entities;
using blogApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DAL.Post
{
    public class PostRepository : BaseRepository<posts>, IPostRepository
    {
        private readonly RepositoryContext context;
        public PostRepository(RepositoryContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<List<GetPostReadDTO>> GetPostsAsync()
        {
            var result = await context.GetPostReadDTO.FromSqlRaw("select * from dbo.get_posts").ToListAsync();

            return result;
        }

        public async Task<List<GetPostReadDTO>> GetCommentsAsync(int postId)
        {
            var result = await context.GetPostReadDTO.FromSqlRaw("select * from dbo.get_comments({0})", postId).ToListAsync();
            return result;
        }
        public async Task<GetPostReadDTO> GetPostsByIdSingle(int Id)
        {
            var postId = Id;

            var result = await context.GetPostReadDTO.FromSqlRaw("select * from dbo.get_posts_single({0})", postId).SingleOrDefaultAsync();
            return result;
        }

        public async Task<posts> GetPostById(int Id)
        {
            var result = await FindByConditionWithTracking(x => x.Id == Id).SingleOrDefaultAsync();

            return result;
        }


    }
}
