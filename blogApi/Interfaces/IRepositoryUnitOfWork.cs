using blogApi.Interfaces.LoginManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.Interfaces
{
    public interface IRepositoryUnitOfWork
    {
        IUsersRepository User { get; }
        IPostRepository Post { get; }

        IReplyPostRepository Replies { get; }

        IFollowersRepository Followers { get; }

        ILikeRepository Likes { get; }
        Task save();
    }
}
