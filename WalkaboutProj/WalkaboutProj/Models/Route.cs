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
        public string WandererId { get; set; }

        [Required(ErrorMessage = "Please name your route")]
        [Display(Name = "Route Name")]
        public string RouteName { get; set; }

        [Display(Name = "Route Description")]
        public string RouteDescription { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Route Markers")]
        public List<Marker> RouteMarkers { get; set; }

        [Display(Name = "Total Distance")]
        public double TotalDistance { get; set; }

        [Display(Name = "Total Points")]
        public double TotalPoints { get; set; }
    }
}
