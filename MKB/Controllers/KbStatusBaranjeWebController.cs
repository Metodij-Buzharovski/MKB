using Microsoft.AspNetCore.Mvc;
using MKB.Data;

namespace MKB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KbStatusBaranjeWebController : Controller
    {
        private readonly ApplicationDbContext _db;

        public KbStatusBaranjeWebController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = " KbStatusBaranjeWeb")]
        public IActionResult Index()
        {
            return Json(_db.KbStatusBaranjeWebs.ToList());
        }
    }
}
