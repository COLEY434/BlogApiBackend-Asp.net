﻿using blogApi.DAL.Login.LoginRepository;
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


        public RepositoriesUnitOfWork(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }


        public IUsersRepository user { 
            get {
                if(_user == null)
                {
                    _user = new UsersRepository(RepositoryContext);
                }

                return _user;
            } }

        public async Task save()
        {
            await RepositoryContext.SaveChangesAsync();

        }
    }
}
