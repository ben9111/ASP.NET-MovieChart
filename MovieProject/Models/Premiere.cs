using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieProject.Models
{
    public class Premiere
    {
        //id of Premiere
        public int ID { get; set; }

        //Premierename-reqiured,lenght than 50 chars,display to user as Premiere Name
        [Required]
        [StringLength(50)]
        [Display(Name = "Premiere Name")]
        public string PremiereName { get; set; }

        //Premiere date-reqired, diaplay to user as Premiere Date
        [Required]
        [Display(Name = "Premiere Date")]
        public DateTime PremiereDate { get; set; }

        //locationName-reqired,diaplay to user as Location
        [Required]
        [Display(Name = "Location")]
        public string locationName { get; set; }

        //Latitude and longitude
        public float locationX { get; set; }
        public float locationY { get; set; }

        //participants-diaplay to user as Participants
        [Display(Name = "Participants")]
        public string participants { get; set; }
    }
}