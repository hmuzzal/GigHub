using System.Linq;
using System.Web.Http;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.API
{
    [Authorize]
    public class AttendanceController : ApiController
    {
        private readonly ApplicationDbContext _context;
        public AttendanceController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var attendeeId = User.Identity.GetUserId();
            var existedAttendance = _context.Attendance.Any(a => a.AttendeeId == attendeeId && a.GigId == dto.GigId);
            if (existedAttendance)
                return BadRequest("The attendance already exists");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = attendeeId
            };

            _context.Attendance.Add(attendance);
            _context.SaveChanges();
            return Ok();
        }
    }
}
