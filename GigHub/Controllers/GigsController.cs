using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }


        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g =>
                            g.ArtistId == userId &&
                           !g.IsCanceled)
                .Include(a => a.Artist)
                .Include(a => a.Genre);
            //var gigs = _context.Gigs
            //               .Where(g =>
            //                           g.ArtistId == userId &&
            //                           g.DateTime > DateTime.Now &&
            //                           !g.IsCanceled)
            //               .Include(a => a.Artist)
            //               .Include(a => a.Genre);


            return View(gigs);
        }


        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendance
                           .Where(g => g.AttendeeId == userId)
                           .Select(a => a.Gig)
                           .Include(a => a.Artist)
                           .Include(a => a.Genre)
                           .ToList();

            var gigsViewModel = new GigsViewModel
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"

            };

            return View("Gigs", gigsViewModel);
        }


        [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();
            var followees = _context.Users.Include(u => u.Followees).Where(u => u.Id == userId);
            return View(followees);
        }


        [Authorize]
        public ActionResult Update(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigViewModel()
            {
                Id = gig.Id,
                Venue = gig.Venue,
                Genre = gig.GenreId,
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Heading = "Update Gig"
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigViewModel()
            {
                Genres = _context.Genres.ToList(),
                Heading = "Add a new gig"
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };
            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }

    }
}