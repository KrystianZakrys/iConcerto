using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iConcerto.Models;
using Microsoft.AspNet.Identity;
using iConcerto.Helpers;
using iConcerto.Repository;
using Microsoft.AspNet.Identity.Owin;

namespace iConcerto.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly ILocationsRepository _locationsRepository;
        private readonly IUserDatasRepository _userDatasRepository;

        public EventsController()
        {
            _locationsRepository = new LocationsRepository();
            _userDatasRepository = new UserDatasRepository();
        }

        public EventsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            ILocationsRepository LocationsRepository, IUserDatasRepository userDatasRepository)
        {
           
            _locationsRepository = LocationsRepository;
            _userDatasRepository = userDatasRepository;
        }

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: EventsForUser
        [Authorize]
        public ActionResult EventsForUser()
        {
            UserData userData = null;
            var loggedUserId = User.Identity.GetUserId();
            var users = db.Users.Where(u => u.ApplicationUserId == loggedUserId);
            if (users.Any())
                 userData = users.First();
            var userEvents = userData != null ? userData.Events: new List<Events>();
            return View(userEvents);
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // GET: Events/Create
        [Authorize]
        public ActionResult Create()
        {
            var factorOptions = _locationsRepository.GetLocations().Select(location => new SelectListItem { Text = location.Name, Value = location.ID.ToString() }).ToList();
            return View(new EventsViewModel() { Locations = factorOptions });
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(EventsViewModel model)
        {
            Int32.TryParse(model.SelectedLocations, out int LocationId);
            string imageUrl = ConcertoHelper.UploadFileToAzureStorage(model.Files);
            Events events = new Events()
            {
                Name = model.Name,
                Description = model.Description,
                Date = model.Date,
                LocationId = LocationId,
                ImageURL = imageUrl
            };
            if (ModelState.IsValid)
            {
                db.Events.Add(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(events);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Date")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(events);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            db.Events.Remove(events);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
