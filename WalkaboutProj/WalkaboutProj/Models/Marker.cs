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

        [ForeignKey("WandererId")]
        public string WandererId { get; set; }

        [ForeignKey("RouteId")]
        public string RouteId { get; set; }

        [Required(ErrorMessage = "Please enter a name for your marker")]
        [Display(Name = "Name")]
        public string MarkerName { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public string MarkerCategory { get; set; }

        [Display(Name = "Image")]
        public string PictureId { get; set; }

        [Display(Name = "Description")]
        public string MarkerDescription { get; set; }

        [Display(Name = "Favorite")]
        public bool IsFavorite { get; set; }
        
        public double LocationLat { get; set; }
        public double LocationLong { get; set; }
    }
}
