using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkaboutProj.Models
{
    public class RouteViewModel
    {
        public Route Route { get; set; }
        public Wanderer Wanderer { get; set; }
        public List<Marker> RouteMarkers { get; set; }
    }
}
