
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieDatabase.Models
{
    public class OMDB_Context : DbContext
    {
        public OMDB_Context(DbContextOptions<OMDB_Context> DbContextOptions) : base(DbContextOptions)
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MovieGenres> MoviesGenres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
