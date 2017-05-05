using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieProject.Models
{
    public class Movie
    {
        //id of Movie
        public int ID { get; set; }

        //MovieName-required,length less than 50 chars,display to user as Movie Name
        [Required]
        [StringLength(50)]
        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        //brief-display to user as brief
        [Display(Name = "brief")]
        public string brief { get; set; }

        //videoLink-display to user as Video URL
        [Display(Name = "Video URL")]
        public string videoLink { get; set; }

        //writer-display to user as Writer
        [Display(Name = "Writer")]
        public string writer { get; set; }

        //stars-display to user as stars
        [Display(Name = "stars")]
        public string stars { get; set; }

        //trailer id
        public int TrailerID { get; set; }

        //foreign key-disc
        public virtual Trailer Trailer { get; set; }
        
    }
}