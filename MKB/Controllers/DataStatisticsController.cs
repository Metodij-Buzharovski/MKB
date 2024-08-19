using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MKB.Data;

namespace MKB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataStatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public DataStatisticsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("NajkoristeniPodmoduli")]
        public IActionResult NajkoristeniPodmoduli()
        {
            var query = (from ka in _db.KbWebKorisnikAktivnosti
                         join wm in _db.KbWebModuli
                         on new { ModulId = ka.ModulId ?? 0, PodModulId = ka.PodModulId ?? 0 }
                            equals new { ModulId = wm.ModulId, PodModulId = wm.PodModulId }
                         group wm by new { wm.ModulId, wm.NazivModul, wm.PodModulId, wm.NazivPodModul } into g
                         select new
                         {
                             ModulId = g.Key.ModulId,
                             NazivModul = g.Key.NazivModul.Trim(),
                             PodModulId = g.Key.PodModulId,
                             NazivPodModul = g.Key.NazivPodModul.Trim(),
                             PodModulIdCount = g.Count()
                         })
                      .OrderByDescending(x => x.PodModulIdCount);

            return Ok(query);
        }

        [HttpGet("PotrosheniSredstvaPoKompanija")]
        public IActionResult PotrosheniSredstvaPoKompanija()
        {
            var query = (from pl in _db.KbWebPravniLica
                         join u in _db.AspNetUsers
                         on pl.LegalEntityId equals u.LegalEntityId
                         join wka in _db.KbWebKorisnikAktivnosti
                         on u.UserWebId equals wka.KorisnikWebId
                         group wka by new { pl.Embs, pl.CompanyName } into g
                         select new
                         {
                             EMBS = g.Key.Embs,
                             CompanyName = g.Key.CompanyName,
                             PotrosheniPoeni = g.Sum(x => x.Poeni),
                             PotroshenPari = g.Sum(x => x.Cena)
                         });

            return Ok(query);
        }
    }
}
