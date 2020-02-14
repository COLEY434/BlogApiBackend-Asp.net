using blogApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.Interfaces
{
    public interface ILikeRepository : IBaseRepository<Likes>
    {
        Task<List<Likes>> GetLikesAsync();
        Task<Likes> checkIfLikeExist(int postId, int userId);
    }
}
