using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WalkaboutProj.Models
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        [ForeignKey("WandererId")]
        public int WandererId { get; set; }

        [Required(ErrorMessage = "Please name your route")]
        [Display(Name = "Route Name")]
        public string RouteName { get; set; }

        [Display(Name = "Route Description")]
        public string RouteDescription { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Total Distance")]
        public double TotalDistance { get; set; }

        [Display(Name = "Total Points")]
        public double TotalPoints { get; set; }

        [Display(Name = "Start Latitude")]
        public double LocationLat { get; set; }

        [Display(Name = "Start Longitude")]
        public double LocationLong { get; set; }
    }
}
