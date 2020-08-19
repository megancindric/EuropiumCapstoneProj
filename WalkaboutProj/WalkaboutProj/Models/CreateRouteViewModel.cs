using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkaboutProj.Models
{
    public class CreateRouteViewModel
    {
        public Route Route { get; set; }
        public Wanderer Wanderer { get; set; }
        public Marker Origin { get; set; }
        public Marker Destination { get; set; }
        public List<Marker> WayPoints {get;set;}
    }
}
