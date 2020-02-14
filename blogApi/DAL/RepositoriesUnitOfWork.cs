using blogApi.DAL.Login.LoginRepository;
using blogApi.DAL.Post;
using blogApi.Interfaces;
using blogApi.Interfaces.LoginManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DAL
{
    public class RepositoriesUnitOfWork : IRepositoryUnitOfWork
    {
        private readonly RepositoryContext RepositoryContext;
        private IUsersRepository _user;
        private IPostRepository _post;
        private IReplyPostRepository _replies;
        private ILikeRepository _likes;


        public RepositoriesUnitOfWork(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public ILikeRepository Likes
        {
            get
            {
                if(_likes == null)
                {
                    _likes = new LikeRepository(RepositoryContext);
                }
                return _likes;
            }
        }
       public IReplyPostRepository Replies {
            get { 
                if(_replies == null)
                {
                    _replies = new ReplyPostRepository(RepositoryContext);
                }
                return _replies;
            }
        }

        public IUsersRepository User { 
            get {
                if(_user == null)
                {
                    _user = new UsersRepository(RepositoryContext);
                }

                return _user;
            } }

        public IPostRepository Post
        {
            get
            {
                if(_post == null)
                {
                    _post = new PostRepository(RepositoryContext);
                }
                return _post;
            }
        }

       
        public async Task save()
        {
            await RepositoryContext.SaveChangesAsync();

        }
    }
}
