using GigHub.Models;
using GigHub.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }


        public ActionResult Index()
        {
            var upComingGigs = _context.Gigs
                    .Include(g => g.Artist)
                    .Include(g => g.Genre)
                    .Where(g => !g.IsCanceled);
            {

            }
            //var upComingGigs = _context.Gigs
            //    .Include(g => g.Artist)
            //    .Include(g => g.Genre)
            //    .Where(g => g.DateTime > DateTime.Now);

            GigsViewModel model = new GigsViewModel
            {
                UpcomingGigs = upComingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs"
            };
            return View("Gigs", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page...";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}