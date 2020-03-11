using blogApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.Interfaces
{
    public interface IReplyPostRepository : IBaseRepository<Replies>
    {
        Task<IEnumerable<Replies>> GetPostComments(int postId);
    }
}
