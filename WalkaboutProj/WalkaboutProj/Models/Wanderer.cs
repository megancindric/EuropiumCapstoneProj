using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WalkaboutProj.Models
{
    public class Wanderer
    {
        [Key]
        public int WandererId { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

        [Required(ErrorMessage = "Please create a unique UserName")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your zip code")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please select your preferred unit")]
        [Display(Name = "Preferred Unit")]
        public string UnitPreference { get; set; }

        [Display(Name = "Weekly Points")]
        public double WeeklyPoints { get; set; }

        [Display(Name = "Weekly Distance")]
        public double WeeklyDistance { get; set; }

        [Display(Name = "Friends List")]
        public List<Wanderer> FriendsList { get; set; }
    }
}
