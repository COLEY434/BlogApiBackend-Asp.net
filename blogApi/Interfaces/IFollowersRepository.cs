using blogApi.DTOS.ReadDTO;
using blogApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.Interfaces
{
    public interface IFollowersRepository : IBaseRepository<Followers>
    {
        Task<Followers> CheckIfFollowerExistAsync(int userToFollowOrUnfollowId, int userThatWantToFollowORUnfollowId);

        Task<bool> GetFollowingStatusAsync(int userId, int followersUserId);

        Task<List<FollowersAndFollowingDTO>> GetFollowersAsync(int userId);
        Task<List<FollowersAndFollowingDTO>> GetFollowingsAsync(int userId);
    }
}
