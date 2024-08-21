using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        //Најкористени** подмодули ** - најмногу записи имаат во база во WebKorisnikAktivnost
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

        //- Трошење на пари и поени по компанија ? sto e ind plateno
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

        //- Компании за кој е баран Х тип на извештај; Х - Сопствен, блокада или корпоративен  ?nema tip izvestaj tabela
        [HttpGet("TipIzvestaj/{id}")]
        public IActionResult TipIzvestaj(int id)
        {
            var query = (from iw in _db.KbIzvestaiWeb
                         join pl in _db.KbWebPravniLica
                         on iw.LegalEntityId equals pl.LegalEntityId
                         where iw.TipIzvestaj == id
                         select new
                         {
                             pl.CompanyName,
                             iw.TipIzvestaj
                         }).Distinct();
            return Ok(query);
        }

        //- Најисплатливи** подмодули ** (најмногу денари се потрошени) - за активности со поени да помножи со цена 
        //на поен(Цена на пакет / вкупно поени во пакет), но да се вратат и поените.Да се игнорираат бесплатните подмодули. ? sto e ind plateno
        [HttpGet("NajisplativiPodmoduli")]
        public IActionResult NajisplativiPodmoduli()
        {
            var query = (from wm in _db.KbWebModuli
                         join wka in _db.KbWebKorisnikAktivnosti
                         on new { ModulId = wm.ModulId, PodModulId = wm.PodModulId } equals new { ModulId = wka.ModulId ?? 0, PodModulId = wka.PodModulId ?? 0 }
                         join wp in _db.KbWebPaketiM
                         on wka.PaketId equals wp.PaketId into wpJoin
                         from wp in wpJoin.DefaultIfEmpty() // Left join
                         where wm.Cena > 0 && wm.Poeni > 0
                         group new { wka, wp } by new { wm.ModulId, wm.NazivModul, wm.PodModulId, wm.NazivPodModul } into g
                         select new
                         {
                             ModulId = g.Key.ModulId,
                             NazivModul = g.Key.NazivModul.Trim(),
                             PodModulId = g.Key.PodModulId,
                             NazivPodModul = g.Key.NazivPodModul.Trim(),
                             Vkupno_Denari = g.Sum(x => x.wka.Cena + (x.wka.Poeni > 0 ? x.wka.Poeni * (x.wp != null ? x.wp.CenaPoen : 0) : 0)),
                             Vkupno_Poeni = g.Sum(x => x.wka.Poeni)
                         });

            return Ok(query);
        }


        //- Начин на плаќање за сите записи во база (вкупно уплати со поени, ПП30 и картичка) ? ind plateno, nema karticka
        [HttpGet("NacinPlakjanje")]
        public IActionResult NacinPlakjanje()
        {
            var query = (from wka in _db.KbWebKorisnikAktivnosti
                         join np in _db.KbNacinPlakanje
                         on wka.NacinPlakanje equals np.NacinPlakanje
                         where np.OpisNacinPlakanje == "поени" || np.OpisNacinPlakanje == "ПП30"
                         group np by np.OpisNacinPlakanje into g
                         select new
                         {
                             OpisNacinPlakanje = g.Key,
                             Vkupno_Uplati = g.Count()
                         });

            return Ok(query);
        }


        //- Начин на плаќање по тип услуга ? ind plateno
        [HttpGet("NacinPlakjanjePoTipUsluga")]
        public IActionResult NacinPlakjanjePoTipUsluga()
        {
            var query = (from wka in _db.KbWebKorisnikAktivnosti
                         join wtu in _db.KbWebTipUslugi
                         on wka.TipUsluga equals wtu.TipUsluga
                         join np in _db.KbNacinPlakanje
                         on wka.NacinPlakanje equals np.NacinPlakanje
                         select new
                         {
                             KorisnikWebID = wka.KorisnikWebId,
                             OpisTipUsluga = wtu.OpisTipUsluga,
                             OpisNacinPlakanje = np.OpisNacinPlakanje
                         })
                      .OrderBy(x => x.OpisTipUsluga);

            return Ok(query);
        }









        //-------------------------------------------------------------------------------------------------------------

        // Трошење на поени по компанија по модул.
        // Endpoint кој ќе прима ЕМБС на компанија и ќе враќа колку поени потрошиле по модул

        [HttpGet("company/points")]
        public IActionResult PointsPerCompanyForModule([FromQuery(Name = "id")] string embs)
        {
            var query = from a in _db.AspNetUsers
                        join kbw in _db.KbWebPravniLica on a.LegalEntityId equals kbw.LegalEntityId
                        join kwk in _db.KbWebKorisnikAktivnosti on a.UserWebId equals kwk.KorisnikWebId
                        join kwm in _db.KbWebModuli on new { ModulId = kwk.ModulId.GetValueOrDefault(), PodmoduliId = kwk.PodModulId.GetValueOrDefault() } equals new { kwm.ModulId, PodmoduliId = kwm.PodModulId }
                        where a.LegalEntity != null
                              && kbw.Embs == embs
                              && kwk.Poeni != 0
                              && kwk.Cena == 0
                        group kwk by kwm.NazivPodModul into grouped
                        orderby grouped.Count() ascending
                        select new
                        {
                            NazivPodModul = grouped.Key,
                            PoeniTroseni = grouped.Count()
                        };
            var result = query.ToList();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(result);
        }


        // Корисници со непотврдена Email адреса

        [HttpGet("email/unconfirmed")]
        public IActionResult UnconfirmedEmails()
        {
            var query = _db.AspNetUsers
                .Where(z => z.EmailConfirmed == false)
                .Count();
            return Ok(query);
        }


        // Компании кои имаат активни пакети по пакети 
        // (број на компании претплатени на Стандард, Бизнис, Премиум и Персонализиран)
        // (сите персонализирани заедно во еден податок)

        [HttpGet("companies/packets")]
        public IActionResult NumberActivePacketsPerCompany()
        {
            var query = from a in _db.AspNetUsers
                        join kwp in _db.KbWebPravniLica on a.LegalEntityId equals kwp.LegalEntityId
                        join kwk in _db.KbWebKorisnikAktivnosti on a.UserWebId equals kwk.KorisnikWebId
                        join kwm in _db.KbWebPaketiM on kwk.PaketId equals kwm.PaketId
                        where a.LegalEntityId != null
                        group kwm by kwm.NazivPaket into grouped
                        select new
                        {
                            NazivPaket = grouped.Key,
                            Firmi = grouped.Count()
                        };
            return Ok(query);
        }

        // Купени извештаи по начин плаќање
        [HttpGet("payment/usluga")]
        public IActionResult NacinPlakanjeNaTipUsluga()
        {
            var query = from a in _db.AspNetUsers
                        join kwk in _db.KbWebKorisnikAktivnosti on a.UserWebId equals kwk.KorisnikWebId
                        join kwm in _db.KbWebModuli on new { ModulId = kwk.ModulId.GetValueOrDefault(), PodmoduliId = kwk.PodModulId.GetValueOrDefault() } equals new { kwm.ModulId, PodmoduliId = kwm.PodModulId }
                        join kwt in _db.KbWebTipUslugi on kwm.TipUsluga equals kwt.TipUsluga
                        join kwn in _db.KbNacinPlakanje on kwk.NacinPlakanje equals kwn.NacinPlakanje
                        group new { kwt.TipUsluga, kwn.OpisNacinPlakanje } by new { kwt.TipUsluga, kwn.NacinPlakanje } into g
                        select new
                        {
                            g.Key.TipUsluga,
                            g.Key.NacinPlakanje,
                            BrojNaPlakanja = g.Count()
                        } into tmp
                        join kwt in _db.KbWebTipUslugi on tmp.TipUsluga equals kwt.TipUsluga
                        join kwn in _db.KbNacinPlakanje on tmp.NacinPlakanje equals kwn.NacinPlakanje
                        select new
                        {
                            kwn.OpisNacinPlakanje,
                            tmp.BrojNaPlakanja,
                            kwt.OpisTipUsluga
                        };
            return Ok(query);
        }

        // Корисници кои го деактивирале/избришале профилот во истиот месец кога и го направиле
        // (DateIns и DateUpd се во ист месец и UserStatus e 0)

        [HttpGet("user/deactivatedmonth")]
        public IActionResult UserDeactivatedInTheSameMonth()
        {
            var query = _db.AspNetUsers
                .Where(z => z.DateIns.Substring(5, 2).Equals(z.DateUpd.Substring(5, 2)) && z.UserStatus == null)
                .Select(z => new
                {
                    z.UserWebId,
                    z.JobTitle,
                    z.LegalEntity.City,
                    z.LegalEntity.CompanyName,
                    z.LegalEntity.CompanyAddress,
                    z.DateIns,
                    z.DateUpd
                });
            return Ok(query);
        }

        // Корисници кои го деактивирале/избришале профилот и корисници кои креирале профил по ден за даден месец
        // месецот да е влезен параметар во API-то


        [HttpGet("user/activated_deactived")]
        public IActionResult UsersActivatedAndDeactivatedInAMonth([FromQuery(Name = "month")] string month)
        {
            var result = _db.AspNetUsers
                .Where(z => z.DateIns.Substring(4, 2).Contains(month))
                .OrderBy(z => z.DateIns)
                .Select(z => new
                {
                    z.UserWebId,
                    z.UserStatus,
                    z.DateIns,
                    z.DateUpd,
                    z.TimeUpd
                });
            return Ok(result);
        }
    }
}

