using MovieProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MovieProject.DAL
{
    //connection to the database
    public class MovieContext:DbContext
    {
        public MovieContext():base("MovieContext")
        {
        }

        //set of Actors
        public DbSet<Actor> Actor { get; set; }
        //set of trailers
        public DbSet<Trailer> Trailer { get; set; }
        //set of Movie
        public DbSet<Movie> Movie { get; set; }
        //set of Premieres
        public DbSet<Premiere> Premiere { get; set; }

        //build the model
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}