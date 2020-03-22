using blogApi.DTOS.ReadDTO;
using blogApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogApi.DAL
{  
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options){

        }

        public DbSet<users> users { get; set; }
        public DbSet<posts> posts { get; set; }

        public DbSet<Likes> likes { get; set; }

        public DbSet<Followers> followers { get; set; }
        public DbSet<Replies> Replies { get; set; }

        public DbSet<GetPostReadDTO> GetPostReadDTO { get; set; }

        public DbSet<FollowersAndFollowingDTO> FollowersAndFollowings { get; set; }

        //public DbSet<GetPostReadDTO> GetPostReadDto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GetPostReadDTO>(options => 
                {
                    options.HasNoKey();
                    options.ToView("get_posts");
                });
            modelBuilder
                .Entity<GetPostReadDTOs>(options =>
                {
                    options.HasNoKey();
                    options.ToTable("get_posts_single");


                });
            modelBuilder
                .Entity<FollowersAndFollowingDTO>(options =>
                {
                    options.HasNoKey();
                    options.ToTable("get_followers");
                });
        }
    }
}

 