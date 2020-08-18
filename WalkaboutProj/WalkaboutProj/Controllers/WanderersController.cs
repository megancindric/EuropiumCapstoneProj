using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WalkaboutProj.Data;
using WalkaboutProj.Models;
using GoogleMaps.LocationServices;


namespace WalkaboutProj.Controllers
{
    public class WanderersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WanderersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wanderers
        public IActionResult Index()
        {
            WandererIndexViewModel wandererView = new WandererIndexViewModel();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            wandererView.Wanderer = _context.Wanderers.Where(w => w.IdentityUserId == userId).FirstOrDefault();
            
            wandererView = Geocode(wandererView);
            //wandererView.MyRoutes = _context.Routes.Where(r => r.WandererId == wandererView.Wanderer.WandererId).ToList();
            return View(wandererView);
        }
        [HttpPost]
        public ActionResult Index(WandererIndexViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewModel.Wanderer = _context.Wanderers.Where(s => s.IdentityUserId == userId).FirstOrDefault();
            //viewModel.MyRoutes = _context.Routes.Where(r => r.WandererId == viewModel.Wanderer.WandererId).ToList();
            viewModel = Geocode(viewModel);

            return View(viewModel);
        }

        // GET: Wanderers/Details/5
        public IActionResult Details(int? id)
        {
            var wanderer = _context.Wanderers.Where(m => m.WandererId == id).FirstOrDefault();
            if (wanderer == null)
            {
                return NotFound();
            }

            return View(wanderer);
        }

        // GET: Wanderers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wanderers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("WandererId,IdentityUserId,Username,FirstName,LastName,PhoneNumber,ZipCode,UnitPreference,WeeklyPoints,WeeklyDistance")] Wanderer wanderer)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            wanderer.IdentityUserId = userId;
                _context.Add(wanderer);
            _context.SaveChanges();
                return RedirectToAction(nameof(Index));
        }

        // GET: Wanderers/Edit/5
        public IActionResult Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wanderer = _context.Wanderers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            return View(wanderer);
        }

        // POST: Wanderers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("WandererId,IdentityUserId,Username,FirstName,LastName,PhoneNumber,ZipCode,UnitPreference")] Wanderer wanderer)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wandererToUpdate = _context.Wanderers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
           try
            {
                wandererToUpdate.FirstName = wanderer.FirstName;
                wandererToUpdate.LastName = wanderer.LastName;
                wandererToUpdate.PhoneNumber = wanderer.PhoneNumber;
                wandererToUpdate.ZipCode = wanderer.ZipCode;
                wandererToUpdate.UnitPreference = wanderer.UnitPreference;

                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WandererExists(wandererToUpdate.WandererId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Wanderers/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wanderer = _context.Wanderers
                .Include(w => w.IdentityUser)
                .FirstOrDefaultAsync(m => m.WandererId == id);
            if (wanderer == null)
            {
                return NotFound();
            }

            return View(wanderer);
        }

        // POST: Wanderers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var wanderer = _context.Wanderers.FirstOrDefault(s => s.WandererId == id);
            _context.Wanderers.Remove(wanderer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateRoute()
        {
            return View();
        }

        // POST: Listing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRoute([Bind("RouteId,WandererId,RouteName,RouteDescription,StartTime,EndTime,TotalDistance,TotalPoints,LocationLat,LocationLong")] Route route)
        {
           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentWanderer = _context.Wanderers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            route.WandererId = currentWanderer.WandererId;
            _context.Add(route);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditRoute(int id)
        {
            //When we create button for this, pass in ID of current listing as param
            var listing = _context.Routes.Where(c => c.RouteId == id).SingleOrDefault();
            return View(listing);
        }

        // POST: Listing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoute(int id, [Bind("RouteId,WandererId,RouteName,RouteDescription,StartTime,EndTime,TotalDistance,TotalPoints,LocationLat,LocationLong")] Route route)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentWanderer = _context.Wanderers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var routeToUpdate = _context.Routes.Where(c => c.RouteId == id).SingleOrDefault();
            try
            {
                routeToUpdate.RouteName = route.RouteName;
                routeToUpdate.RouteDescription = route.RouteDescription;
                //Should not be able to change other parts of route
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteExists(route.RouteId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Traders/Delete/5
        public IActionResult DeleteRoute(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = _context.Routes
                .Include(c => c.RouteId)
                .FirstOrDefault(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Traders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRouteConfirmed(int id)
        {
            var route = _context.Routes.FirstOrDefault(m => m.RouteId == id);
            _context.Routes.Remove(route);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RouteDetails(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentWanderer = _context.Wanderers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            var route = _context.Routes.Where(m => m.RouteId == id).FirstOrDefault();
            var routeMarkers = _context.Markers.Where(r => r.RouteId == route.RouteId).ToList();
            if (route == null)
            {
                return NotFound();
            }
            else
            {
                RouteViewModel viewModel = new RouteViewModel();
                viewModel.Wanderer = currentWanderer;
                viewModel.RouteMarkers = routeMarkers;
                viewModel.Route = route;

                return View(viewModel);
            }
        }

        private bool WandererExists(int id)
        {
            return _context.Wanderers.Any(e => e.WandererId == id);
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }

        private bool MarkerExists(int id)
        {
            return _context.Markers.Any(e => e.MarkerId == id);
        }

        public WandererIndexViewModel Geocode(WandererIndexViewModel indexView)
        {
            AddressData address = new AddressData
            {
                Zip = indexView.Wanderer.ZipCode
            };
            var geocodeRequest = new GoogleLocationService(APIKeys.APIKEY);
            var latlong = geocodeRequest.GetLatLongFromAddress(address);
            indexView.WandererLat = latlong.Latitude;
            indexView.WandererLong = latlong.Longitude;
            return indexView;
        }
        public TakeAWalkViewModel GeocodeRoute(TakeAWalkViewModel viewModel)
        {
            AddressData address = new AddressData
            {
                Zip = viewModel.Wanderer.ZipCode
            };
            var geocodeRequest = new GoogleLocationService(APIKeys.APIKEY);
            var latlong = geocodeRequest.GetLatLongFromAddress(address);
            viewModel.WandererLat = latlong.Latitude;
            viewModel.WandererLong = latlong.Longitude;
            return viewModel;
        }
        public IActionResult TakeAWalk()
        {
            TakeAWalkViewModel viewModel = new TakeAWalkViewModel();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewModel.Wanderer = _context.Wanderers.Where(s => s.IdentityUserId == userId).FirstOrDefault();
            //viewModel.MyRoutes = _context.Routes.Where(r => r.WandererId == viewModel.Wanderer.WandererId).ToList();
            viewModel = GeocodeRoute(viewModel);

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult TakeAWalk(TakeAWalkViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewModel.Wanderer = _context.Wanderers.Where(s => s.IdentityUserId == userId).FirstOrDefault();
            //viewModel.MyRoutes = _context.Routes.Where(r => r.WandererId == viewModel.Wanderer.WandererId).ToList();
            viewModel = GeocodeRoute(viewModel);
            return View(viewModel);
        }
        // GET a marker
        [HttpGet]
        public IActionResult GetAllMarkers()
        {
            //get entire list of markers in DB
            IList<Marker> markers = _context.Markers.ToList();
            return Ok(markers);
        }

        [HttpGet]
        public IActionResult GetRouteMarkers(int RouteId)
        {
            IList<Marker> routeMarkers = _context.Markers.Where(s => s.RouteId == RouteId).ToList();
            return Ok(routeMarkers);

        }
        // GET specific marker
        [HttpGet("{id}")]
        public IActionResult Getmarker(int id)
        {
            // Retrieve movie by id from db logic
            // return Ok(movie);
            var currentMarker = _context.Markers.Where(s => s.MarkerId == id).SingleOrDefault();
            if (currentMarker == null)
            {
                return NotFound();
            }
            return Ok(currentMarker);
        }
        // POST marker
        [HttpPost]
        public ActionResult PostMarker([FromBody] Marker marker)
        {
            _context.Add(marker);
            _context.SaveChanges();
            return Ok();
        }
        // PUT marker
        [HttpPut]
        public IActionResult PutMarker([FromBody] Marker marker)
        {
            var markerToUpdate = _context.Markers.Where(s => s.MarkerId == marker.MarkerId).SingleOrDefault();
            if (markerToUpdate == null)
            {
                return NotFound();
            }
            markerToUpdate.MarkerName = marker.MarkerName;
            markerToUpdate.MarkerDescription = marker.MarkerDescription;
            markerToUpdate.MarkerCategory = marker.MarkerCategory;
            markerToUpdate.PicturePath = marker.PicturePath;
            _context.Update(markerToUpdate);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteMarker(int MarkerId)
        {
            // Delete movie from db logic
            var markerToDelete = _context.Markers.Where(s => s.MarkerId == MarkerId).FirstOrDefault();
            if (markerToDelete == null)
            {
                return NotFound();
            }
            _context.Remove(markerToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}
