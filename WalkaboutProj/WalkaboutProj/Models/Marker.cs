using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalkaboutProj.Models
{
    public class Marker
    {
        [Key]
        public int MarkerId { get; set; }

        [ForeignKey("RouteId")]
        public int RouteId { get; set; }

        [Display(Name = "Name")]
        public string MarkerName { get; set; }

        [Display(Name = "Category")]
        public string MarkerCategory { get; set; }

        [Display(Name = "Image")]
        public string PicturePath { get; set; }

        [Display(Name = "Description")]
        public string MarkerDescription { get; set; }

        [Display(Name = "Favorite")]
        public bool IsFavorite { get; set; }
        public double PointValue { get; set; }

        public double MarkerLat { get; set; }
        public double MarkerLong { get; set; }
    }
}
