using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkaboutProj.Models
{
    public class WandererIndexViewModel
    {
        public Wanderer Wanderer { get; set; }
        public List<Route> MyRoutes { get; set; }
        public Route HighScore { get; set; }
        public double MyTotalPoints { get; set; }
        public double MyTotalDistanceKM { get; set; }
        public double MyTotalDistanceMI { get; set; }
        public int MyTotalWalkCount { get; set; }
        public double WandererLat { get; set; }
        public double WandererLong { get; set; }
    }
}
