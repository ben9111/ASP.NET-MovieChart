using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieProject.Models
{
    public class Actor
    {
        //id of Actor
        public int ID { get; set; }

        //ActorName-required,length less than 50 chars,display to user as Actor Name
        [Required]
        [StringLength(50)]
        [Display(Name = "Actor Name")]
        public string ActorName { get; set; }

        //gender-required,length less than 20 chars,display to user as Gender
        [Required]
        [StringLength(20)]
        [Display(Name = "Gender")]
        public string gender { get; set; }

        //ActorImage-required,length less than 50 chars,display to user as URL Image
        [Required]
        [StringLength(100)]
        [Display(Name = "URL Image")]
        public string ActorImage { get; set; }

        //catagory-required,length less than 50 chars,display to user as Category
        [Required]
        [StringLength(50)]
        [Display(Name = "Category")]
        public string catagory { get; set; }

        //io collection of trailers
        public virtual ICollection<Trailer> Trailers { get; set; }
    }
}