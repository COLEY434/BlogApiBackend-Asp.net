using blogApi.DTOS.ReadDTO;
using blogApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.Interfaces
{
    public interface IPostRepository : IBaseRepository<posts>
    {
        Task<List<GetPostReadDTO>> GetPostsAsync();
        Task<posts> GetPostById(int Id);
        Task<GetPostReadDTO> GetPostsByIdSingle(int Id);
        Task<List<GetPostReadDTO>> GetCommentsAsync(int postId);
        
    }
}
