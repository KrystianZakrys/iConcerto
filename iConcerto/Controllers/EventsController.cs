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
            var userEvents = userData != null ? userData.EventToUser.Select(eu => eu.Events).ToList(): new List<Events>();
            return View(userEvents);
        }

        // GET: Events/Details/5
        [Authorize]
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
            //check if assigned
            var loggedUserId = User.Identity.GetUserId();
            UserData userData = db.Users.Where(ud => ud.ApplicationUserId == loggedUserId).First();
            var isAssigned = userData.EventToUser.Select(eu => eu.Events).Where(ue => ue.EventId == events.EventId).Any();
            ViewBag.isAssigned = isAssigned;
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
        [Authorize]
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
        public ActionResult Edit([Bind(Include = "EventId,Name,Description,Date")] Events events)
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
        [Authorize]
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

        //GET: Events/AssignToUser/5
        [Authorize]
        public ActionResult AssignToUser(int id)
        {
            var loggedUserId = User.Identity.GetUserId();
            UserData userData = db.Users.Where(ud => ud.ApplicationUserId == loggedUserId).First();
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

        // POST: Events/AssignToUser/5
        [HttpPost, ActionName("AssignToUser")]
        [Authorize]
        public ActionResult AssignToUserConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            var loggedUserId = User.Identity.GetUserId();

            UserData userData = db.Users.Where(ud => ud.ApplicationUserId == loggedUserId).First();

            EventToUser eventToUser = new EventToUser() { Events = events, Notified = false, UserData = userData };
            userData.EventToUser.Add(eventToUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Events/UnassignFromUser/5
        [Authorize]
        public ActionResult UnassignFromUser(int id)
        {
            var loggedUserId = User.Identity.GetUserId();
            UserData userData = db.Users.Where(ud => ud.ApplicationUserId == loggedUserId).First();
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

        // POST: Events/UnassignFromUser/5
        [HttpPost, ActionName("UnassignFromUser")]
        [Authorize]
        public ActionResult UnassignFromUserConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            var loggedUserId = User.Identity.GetUserId();

            UserData userData = db.Users.Where(ud => ud.ApplicationUserId == loggedUserId).First();
            EventToUser eventToUser = db.EventToUser.Where(ue => ue.UserData.UserDataId == userData.UserDataId && ue.Events.EventId == events.EventId).SingleOrDefault();
            userData.EventToUser.Remove(eventToUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ManageEventUsers(int id)
        {
            List<UserData> userData = db.EventToUser.Where(ue => ue.Events.EventId == id).Select(ue => ue.UserData).ToList();
            ViewBag.eventId = id;
            return View(userData);
        }

        [Authorize]
        public ActionResult UnassignUserByAdmin(int id, int userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventToUser eventToUser = db.EventToUser.Where(eu => eu.Events.EventId == id && eu.UserData.UserDataId == userId).SingleOrDefault();
            if (eventToUser == null)
            {
                return HttpNotFound();
            }
            return View(eventToUser);
        }


        [Authorize]
        [HttpPost, ActionName("UnassignUserByAdmin")]
        public ActionResult UnassignUserByAdminConfirmed(int id, int userId)
        {
            EventToUser eventToUser = db.EventToUser.Where(eu => eu.Events.EventId == id && eu.UserData.UserDataId == userId).SingleOrDefault();
            db.EventToUser.Remove(eventToUser);
            db.SaveChanges();

            return RedirectToAction("ManageEventUsers","Events",new { id });
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
