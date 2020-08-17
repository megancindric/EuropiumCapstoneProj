using GoogleMaps.LocationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkaboutProj.Models
{
    public class TakeAWalkViewModel
    {
        public Wanderer Wanderer { get; set; }
        public double WandererLat { get; set; }
        public double WandererLong { get; set; }
    }
}
