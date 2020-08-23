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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Security.Cryptography.X509Certificates;

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
            wandererView.MyRoutes = _context.Routes.Where(r => r.WandererId == wandererView.Wanderer.WandererId).ToList();
            wandererView.MyTotalDistanceKM = 0;
            wandererView.MyTotalDistanceMI = 0;
            wandererView.MyTotalPoints = 0;
            wandererView.MyTotalWalkCount = wandererView.MyRoutes.Count;
            if (wandererView.MyRoutes.Count != 0)
            {
                wandererView.HighScore = wandererView.MyRoutes.OrderByDescending(r => r.TotalPoints).First();

                foreach (Route route in wandererView.MyRoutes)
                {

                        wandererView.MyTotalDistanceMI += route.TotalDistanceMiles;
                        wandererView.MyTotalDistanceKM += route.TotalDistanceKilometers;
                    wandererView.MyTotalPoints += route.TotalPoints;
                }
            }
           
            wandererView = Geocode(wandererView);
            return View(wandererView);
        }
        [HttpPost]
        public ActionResult Index(WandererIndexViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewModel.Wanderer = _context.Wanderers.Where(s => s.IdentityUserId == userId).FirstOrDefault();
            //function to get highest scoring route
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
        [HttpGet]
        public IActionResult Getmarker(int MarkerId)
        {
            // Retrieve movie by id from db logic
            // return Ok(movie);
            var currentMarker = _context.Markers.Where(s => s.MarkerId == MarkerId).SingleOrDefault();
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
        public ActionResult PutMarker([FromBody] Marker updatedMarker)
        {
            var markerToUpdate = _context.Markers.Where(s => s.MarkerId == updatedMarker.MarkerId).FirstOrDefault();
            if (markerToUpdate == null)
            {
                return NotFound();
            }
            markerToUpdate.MarkerName = updatedMarker.MarkerName;
            markerToUpdate.MarkerDescription = updatedMarker.MarkerDescription;
            markerToUpdate.MarkerCategory = updatedMarker.MarkerCategory;
            markerToUpdate.PicturePath = updatedMarker.PicturePath;
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
            return Json(new { markerToDelete.PointValue });
        }

        [HttpPost]
        public ActionResult PostRoute([FromBody] Route route)
        {
            _context.Add(route);
            _context.SaveChanges();
            return Json(new { route.RouteId });
        }

        // GET specific marker
        [HttpGet]
        public IActionResult GetRoute(int RouteId)
        {

            var currentRoute = _context.Routes.Where(s => s.RouteId == RouteId).SingleOrDefault();
            if (currentRoute == null)
            {
                return NotFound();
            }
            return Ok(currentRoute);
        }


        [HttpPut]
        public ActionResult PutRoute([FromBody] Route updatedRoute)
        {
            var routeToUpdate = _context.Routes.Where(s => s.RouteId == updatedRoute.RouteId).FirstOrDefault();
            if (routeToUpdate == null)
            {
                return NotFound();
            }
            
            routeToUpdate.RouteRating = updatedRoute.RouteRating;
            routeToUpdate.RouteName = updatedRoute.RouteName;
            routeToUpdate.RouteDescription = updatedRoute.RouteDescription;
            routeToUpdate.TotalDistanceMiles = updatedRoute.TotalDistanceMiles;
            routeToUpdate.TotalDistanceKilometers = updatedRoute.TotalDistanceKilometers;
            routeToUpdate.TotalTimeMinutes = updatedRoute.TotalTimeMinutes;
            routeToUpdate.TotalPoints = updatedRoute.TotalPoints;
            _context.Update(routeToUpdate);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteRoute(int RouteId)
        {
            // Delete movie from db logic
            var routeToDelete = _context.Routes.Where(s => s.RouteId == RouteId).FirstOrDefault();
            if (routeToDelete == null)
            {
                return NotFound();
            }
            _context.Remove(routeToDelete);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult PrepareRouteWaypoints(int RouteId)
        {
            Marker routeStart = _context.Markers.Where(s => s.RouteId == RouteId && s.MarkerCategory == "StartPoint").FirstOrDefault();
            Marker routeEnd = _context.Markers.Where(s => s.RouteId == RouteId && s.MarkerCategory == "EndPoint").FirstOrDefault();
            IList<Marker> routeMarkers = _context.Markers.Where(s => s.RouteId == RouteId && (s.MarkerCategory == "Wildlife" || s.MarkerCategory == "Landmark" || s.MarkerCategory == "Highlight")).ToList();
            routeMarkers.Insert(0, routeStart);
            routeMarkers.Add(routeEnd);
            return Ok(routeMarkers);

        }

        public string UploadImage(IFormFile files)
        {
            var myFileName = "";
            if (files != null)
            {

                if (files.Length > 0)
                {
                    var fileName = Path.GetFileName(files.FileName);
                    myFileName = Convert.ToString(Guid.NewGuid());
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = string.Concat(myFileName, fileExtension);
                    var filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img")).Root + $@"\{newFileName}";

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        files.CopyTo(fs);
                        fs.Flush();
                    }
                    return newFileName;
                }

            }
            return myFileName;
        }
    }
}
