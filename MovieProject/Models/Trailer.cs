using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieProject.Models
{
    public class Trailer
    {
        //id of trailer
        public int ID { get; set; }

        //trailer name-required,size less than 20,display to user as Trailer Name
        [Required]
        [StringLength(20)]
        [Display(Name = "Trailer Name")]
        public string TrailerName { get; set; }

        //Director name display to user as Director Name
        [Required]
        [StringLength(20)]
        [Display(Name = "Director Name")]
        public string DirectorName { get; set; }

        //image url-display to user as Image URL
        [Display(Name = "Image URL")]
        public string TrailerImage { get; set; }

        //release date-reqired,display to user as Release Date
        [Required]
        [Display(Name = "Release Date")]
        public DateTime releaseDate { get; set; }

        //Actor id
        public int ActorID { get; set; }

        //foreign key-Actor
        public virtual Actor Actor { get; set; }

        //io collection-Actor
        public virtual ICollection<Movie> Movies { get; set; }
    }
}