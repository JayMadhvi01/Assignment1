using EventSignup.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventSignup.Controllers
{
    public class EventsController : Controller
    {
        // static so attendees survive between requests (controllers are recreated every request)
        private static readonly List<Event> _events = new()
        {
            new Event { Id = 1, Title = "Career Fair", Date = new DateTime(2026, 2, 1),  Location = "Gym" },
            new Event { Id = 2, Title = "Tech Talk",   Date = new DateTime(2026, 2, 8),  Location = "Auditorium" },
            new Event { Id = 3, Title = "Hack Night",  Date = new DateTime(2026, 2, 15), Location = "Library" }
        };

        // GET: /Events
        public IActionResult Index()
        {
            ViewData["Title"] = "Event Manager";
            ViewData["Subtitle"] = "Select an event to manage attendees.";
            return View(_events);
        }

        // GET: /Events/Manage/1
        [HttpGet]
        public IActionResult Manage(int id)
        {
            var ev = _events.FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();

            ViewData["Title"] = $"Manage Attendees – {ev.Title}";
            ViewData["Message"] = TempData["Message"];
            return View(ev);
        }

        // POST: /Events/Manage/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Manage(int id, Attendee attendee)
        {
            var ev = _events.FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = $"Manage Attendees – {ev.Title}";
                return View(ev);
            }

            ev.Attendees.Add(attendee);
            TempData["Message"] = "Attendee registered!";
            return RedirectToAction(nameof(Manage), new { id });
        }
    }
}