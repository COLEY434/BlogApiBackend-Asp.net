﻿using blogApi.Entities;
using blogApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DAL.Post
{
    public class ReplyPostRepository : BaseRepository<Replies>, IReplyPostRepository
    {
        private readonly RepositoryContext _context;
        public ReplyPostRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
