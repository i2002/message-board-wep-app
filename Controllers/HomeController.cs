using MessageBoard.Data;
using MessageBoard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MessageBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly MessageBoardContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(MessageBoardContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (_context.Announcement == null)
            {
                return Problem("Entity set 'MessageBoardContext.Announcement'  is null.");
            }
            var announcements = _context.Announcement;
            ViewBag.Announcements = announcements;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}