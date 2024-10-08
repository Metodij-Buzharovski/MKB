﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MKB.Data;
using MKB.Models;
using System.Reflection;

namespace MKB.Controllers
{
    [EnableCors("Policy1")]
    [Route("api/[controller]")]
    [ApiController]
    public class DataStatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public DataStatisticsController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Најкористени** подмодули ** - најмногу записи имаат во база во WebKorisnikAktivnost  !!!!!OK
        //Tabelata KorisnikAktivnost da se dodade modulId i podmodulId koga tip usluga e 7
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

        //- Трошење на пари и поени по компанија !!!!!!OK
        [HttpGet("PotrosheniSredstvaPoKompanija")]
        public IActionResult PotrosheniSredstvaPoKompanija()
        {
            var query = (from pl in _db.KbWebPravniLica
                         join u in _db.AspNetUsers on pl.LegalEntityId equals u.LegalEntityId
                         join wka in _db.KbWebKorisnikAktivnosti on u.UserWebId equals wka.KorisnikWebId
                         group wka by new { pl.LegalEntityId, pl.Embs, pl.CompanyName } into g
                         select new
                         {
                             LegalEntityId = g.Key.LegalEntityId,
                             EMBS = g.Key.Embs,
                             CompanyName = g.Key.CompanyName,
                             PotrosheniPoeni = g.Sum(x => (x.TipUsluga == 1 || x.TipUsluga == 2 || x.TipUsluga == 6) ? x.Poeni : 0),
                             PotroshenoDenari = g.Sum(x => x.IndPlateno == true ? x.Cena : 0)
                         })
                      .OrderByDescending(x => x.PotroshenoDenari)
                      .ThenByDescending(x => x.PotrosheniPoeni)
                      .ToList();

            return Ok(query);
        }

        //- Компании за кој е баран Х тип на извештај; Х - Сопствен, блокада или корпоративен   !!!!OK
        [HttpGet("TipIzvestaj/{id}")]
        public IActionResult TipIzvestaj(int id)
        {
            var query = (from iw in _db.KbIzvestaiWeb
                         join pl in _db.KbWebPravniLica
                         on iw.LegalEntityId equals pl.LegalEntityId
                         join vki in _db.KbVidKorIzvestai
                         on iw.VidKorIzv equals vki.VidKorIzv
                         where new[] { 0, 13, 14 }.Contains(vki.VidKorIzv) && vki.VidKorIzv == id
                         group iw by new { pl.LegalEntityId, pl.CompanyName, vki.VidKorIzv, vki.OpisVidKorIzv } into g
                         select new
                         {
                             g.Key.LegalEntityId,
                             g.Key.CompanyName,
                             g.Key.VidKorIzv,
                             g.Key.OpisVidKorIzv,
                             VkupnoPobaraniIzv = g.Count()
                         }).OrderBy(x => x.LegalEntityId);
            return Ok(query);
        }

        //- Најисплатливи** подмодули ** (најмногу денари се потрошени) - за активности со поени да помножи со цена 
        //на поен(Цена на пакет / вкупно поени во пакет), но да се вратат и поените.Да се игнорираат бесплатните подмодули.
        [HttpGet("NajisplativiPodmoduli")]
        public IActionResult NajisplativiPodmoduli()
        {
            var query = (from wm in _db.KbWebModuli
                         join wka in _db.KbWebKorisnikAktivnosti
                         on new { ModulId = wm.ModulId, PodModulId = wm.PodModulId } equals new { ModulId = wka.ModulId ?? 0, PodModulId = wka.PodModulId ?? 0 }
                         join wp in _db.KbWebPaketiM
                         on wka.PaketId equals wp.PaketId into wpGroup
                         from wp in wpGroup.DefaultIfEmpty() // Left join equivalent
                         where wm.Cena > 0 && wm.Poeni > 0
                         group new { wka, wp } by new { wm.ModulId, wm.NazivModul, wm.PodModulId, wm.NazivPodModul } into g
                         select new
                         {
                             g.Key.ModulId,
                             NazivModul = g.Key.NazivModul.Trim(),
                             g.Key.PodModulId,
                             NazivPodModul = g.Key.NazivPodModul.Trim(),
                             Vkupno_Denari = g.Sum(x => x.wka.Cena + (
                                 x.wka.PaketId != null && x.wka.Poeni > 0 && (x.wka.TipUsluga == 1 || x.wka.TipUsluga == 2 || x.wka.TipUsluga == 6)
                                 ? x.wka.Poeni * (x.wp != null ? x.wp.CenaPoen : 0) : 0)),
                             Vkupno_Poeni = g.Sum(x => x.wka.Poeni > 0 && (x.wka.TipUsluga == 1 || x.wka.TipUsluga == 2 || x.wka.TipUsluga == 6)
                                 ? x.wka.Poeni : 0)
                         })
                         .OrderByDescending(x => x.Vkupno_Denari);

            return Ok(query);
        }


        //- Начин на плаќање за сите записи во база (вкупно уплати со поени, ПП30 и картичка) !!!!!!!OK
        //Koga Tip Usluga e 1 ili 2, i se plakja so poeni, da se dodade NacinPLakjanje da bide 6 i IndPlateno da e 1
        [HttpGet("NacinPlakjanje")]
        public IActionResult NacinPlakjanje()
        {
            var query = (from wka in _db.KbWebKorisnikAktivnosti
                              join np in _db.KbNacinPlakanje
                              on wka.NacinPlakanje equals np.NacinPlakanje into npGroup
                              from tmp in npGroup.DefaultIfEmpty() // Left join equivalent
                              where (tmp.OpisNacinPlakanje == "поени" || tmp.OpisNacinPlakanje == "ПП30" || 
                              tmp.OpisNacinPlakanje == "CPay" || tmp.OpisNacinPlakanje == null)
                                    && ((new[] { 1, 2 }.Contains(wka.TipUsluga) && wka.Poeni > 0) || wka.IndPlateno == true)
                              group wka by new
                              {
                                  NacinPlakanje = (wka.NacinPlakanje == null) ? 6 : wka.NacinPlakanje,
                                  OpisNacinPlakanje = (tmp == null) ? "поени" : tmp.OpisNacinPlakanje
                              } into g
                              select new
                              {
                                  g.Key.NacinPlakanje,
                                  g.Key.OpisNacinPlakanje,
                                  Vkupno_Uplati = g.Count()
                              }).OrderBy(x => x.NacinPlakanje);
            return Ok(query);
        }


        //- Начин на плаќање по тип услуга !!!!!!!OK
        [HttpGet("NacinPlakjanjePoTipUsluga")]
        public IActionResult NacinPlakjanjePoTipUsluga()
        {
            var query = (from wka in _db.KbWebKorisnikAktivnosti
                         join wtu in _db.KbWebTipUslugi on wka.TipUsluga equals wtu.TipUsluga
                         join np in _db.KbNacinPlakanje on wka.NacinPlakanje equals np.NacinPlakanje into npGroup
                         from tmp in npGroup.DefaultIfEmpty()
                         where (new[] { 1, 2 }.Contains(wka.TipUsluga) && wka.Poeni > 0) || wka.IndPlateno == true
                         group wka by new
                         {
                             wtu.TipUsluga,
                             wtu.OpisTipUsluga,
                             NacinPlakanje = (tmp == null) ? 6 : tmp.NacinPlakanje,
                             OpisNacinPlakanje = (tmp == null) ? "поени" : tmp.OpisNacinPlakanje
                         } into g
                         select new
                         {
                             g.Key.TipUsluga,
                             g.Key.OpisTipUsluga,
                             g.Key.NacinPlakanje,
                             g.Key.OpisNacinPlakanje,
                             Vkupno = g.Count()
                         })
                      .OrderBy(x => x.TipUsluga)
                      .ThenBy(x => x.NacinPlakanje);

            return Ok(query);
        }


        //- - Генерирани барања за извештаи по статус на барањето  --Има само за статус барање 3 и 5  !!!!OK
        [HttpGet("GeneriraniBaranjaZaIzvestaiPoStatusBaranje")]
        public IActionResult GeneriraniBaranjaZaIzvestaiPoStatusBaranje()
        {
            var query = (from bw in _db.KbBaranjaWeb
                         join iw in _db.KbIzvestaiWeb
                         on bw.IzvestajWebId equals iw.IzvestajWebId
                         join sbw in _db.KbStatusBaranjeWeb
                         on bw.StatusBaranjeWeb equals sbw.StatusBaranjeWeb
                         join wka in _db.KbWebKorisnikAktivnosti
                         on bw.BaranjeWebId equals wka.BaranjeWebId
                         join spw in _db.KbStatusPretplataWeb
                         on wka.StatusPretplata equals spw.StatusPretplata
                         group iw by new { bw.StatusBaranjeWeb, sbw.OpisStatusBaranjeWeb, spw.StatusPretplata, spw.OpisStatusPretplata } into g
                         orderby g.Key.StatusBaranjeWeb
                         select new
                         {
                             g.Key.StatusBaranjeWeb,
                             g.Key.OpisStatusBaranjeWeb,
                             g.Key.StatusPretplata,
                             g.Key.OpisStatusPretplata,
                             BaranjaIzvestai = g.Count()
                         }).OrderBy(x => x.StatusBaranjeWeb);

            return Ok(query);
        }

        //Активности по статус претплата   !!!!OK
        [HttpGet("AktivnostiPoStatusPretplata")]
        public IActionResult AktivnostiPoStatusPretplata()
        {
            var query = (from wka in _db.KbWebKorisnikAktivnosti
                         join spw in _db.KbStatusPretplataWeb
                         on wka.StatusPretplata equals spw.StatusPretplata
                         group wka by new { spw.StatusPretplata, spw.OpisStatusPretplata } into g
                         select new
                         {
                             g.Key.StatusPretplata,
                             g.Key.OpisStatusPretplata,
                             Br_Aktivnosti = g.Count()
                         }).OrderBy(x => x.StatusPretplata);

            return Ok(query);
        }


        //- Компании кои искористиле промо код при плаќање  !!!!OK
        [HttpGet("KompaniiKoiKoristelePromoKod")]
        public IActionResult KompaniiKoiKoristelePromoKod()
        {
            var query = (from ipk in _db.KbWebLogIskoristeniPromoKodovi
                         join anu in _db.AspNetUsers
                         on ipk.KorisnikWebId equals anu.UserWebId
                         join pl in _db.KbWebPravniLica
                         on anu.LegalEntityId equals pl.LegalEntityId
                         join wka in _db.KbWebKorisnikAktivnosti
                         on ipk.AktivnostId equals wka.Id
                         join wtu in _db.KbWebTipUslugi
                         on wka.TipUsluga equals wtu.TipUsluga
                         group ipk by new { pl.LegalEntityId, pl.CompanyName, wtu.TipUsluga, wtu.OpisTipUsluga } into g
                         select new
                         {
                             g.Key.LegalEntityId,
                             g.Key.CompanyName,
                             g.Key.TipUsluga,
                             g.Key.OpisTipUsluga,
                             IskoristeniPromoKodovi = g.Count()
                         }).OrderBy(x => x.LegalEntityId).ThenByDescending(x => x.IskoristeniPromoKodovi);

            return Ok(query);
        }



        //- Компании кои не се претплатиле на пакет откако им истекол стариот. Да враќа и за тие што повторно СЕ претплатиле.  !!!!OK   
        [HttpGet("KompaniiPretplateniNaPaket")]
        public IActionResult KompaniiPretplateniNaPaket()
        {
            var query = (from wkp in _db.KbWebKorisnikPaketi
                         join wp in _db.KbWebPaketiM 
                         on wkp.PaketId equals wp.PaketId into wpGroup
                         from tmp in wpGroup.DefaultIfEmpty()
                         join anu in _db.AspNetUsers on wkp.KorisnikWebId equals anu.UserWebId
                         join pl in _db.KbWebPravniLica on anu.LegalEntityId equals pl.LegalEntityId
                         select new
                         {
                             LegalEntityID = pl.LegalEntityId,
                             CompanyName = pl.CompanyName,
                             IsPretplatenNaPaket = (wkp.PaketId != null && wkp.DatKrajPaket > DateTime.Now) ? "YES" : "NO",
                             PaketId = wkp.PaketId,
                             NazivPaket = tmp == null ? null : tmp.NazivPaket.Trim(),
                             DatPocPaket = wkp.DatPocPaket,
                             DatKrajPaket = wkp.DatKrajPaket
                         })
                         .OrderByDescending(x => x.DatKrajPaket);

            return Ok(query);
        }



        //- компании кои имаат доплатено за дополнителни поени при претплата на пакет
        //(hint: во истата активност за пакет ќе е пополнета колоната за дополнителни поени во база)   !!!!OK
        [HttpGet("KompaniiKoiDoplatileZaPoeniPriPretplataNaPaket")]
        public IActionResult KompaniiKoiDoplatileZaPoeniPriPretplataNaPaket()
        {
            var query = (from wka in _db.KbWebKorisnikAktivnosti
                         join anu in _db.AspNetUsers on wka.KorisnikWebId equals anu.UserWebId
                         join pl in _db.KbWebPravniLica on anu.LegalEntityId equals pl.LegalEntityId
                         join wp in _db.KbWebPaketiM on wka.PaketId equals wp.PaketId
                         where wka.TipUsluga == 3 && wka.Poeni > 0
                         group wka by new { anu.UserWebId, pl.LegalEntityId, pl.CompanyName, wp.PaketId, wp.NazivPaket } into g
                         select new
                         {
                             UserWebID = g.Key.UserWebId,
                             LegalEntityId = g.Key.LegalEntityId,
                             CompanyName = g.Key.CompanyName,
                             PaketId = g.Key.PaketId,
                             NazivPaket = g.Key.NazivPaket.Trim(),
                             DopolnitelniPoeni = g.Sum(x => x.Poeni)
                         })
                      .OrderByDescending(x => x.DopolnitelniPoeni)
                      .ToList();

            return Ok(query);
        }



        //- компании кои имаат доплатено за дополнителни поени при претплата на пакет
        //(hint: во истата активност за пакет ќе е пополнета колоната за дополнителни поени во база)   !!!!OK
        [HttpGet("KompaniiKoiDoplatileZaPoeniPoslePretplataNaPaket")]
        public IActionResult KompaniiKoiDoplatileZaPoeniPoslePretplataNaPaket()
        {
            var query = (from wka in _db.KbWebKorisnikAktivnosti
                         join anu in _db.AspNetUsers on wka.KorisnikWebId equals anu.UserWebId
                         join pl in _db.KbWebPravniLica on anu.LegalEntityId equals pl.LegalEntityId
                         join wp in _db.KbWebPaketiM on wka.PaketId equals wp.PaketId
                         where wka.TipUsluga == 4 && wka.Poeni > 0
                         group wka by new { anu.UserWebId, pl.LegalEntityId, pl.CompanyName } into g
                         select new
                         {
                             UserWebID = g.Key.UserWebId,
                             LegalEntityId = g.Key.LegalEntityId,
                             CompanyName = g.Key.CompanyName,
                             DopolnitelniPoeni = g.Sum(x => x.Poeni)
                         })
                      .OrderByDescending(x => x.DopolnitelniPoeni)
                      .ToList();

            return Ok(query);
        }




        //- Корисници со непотврдена Email адреса   !!!!OK
        [HttpGet("KorisniciSoNepotvrdenEmail")]
        public IActionResult KorisniciSoNepotvrdenEmail()
        {
            var query = (from anu in _db.AspNetUsers
                         where anu.EmailConfirmed == false || anu.EmailConfirmed == null
                         select new
                         {
                             anu.Id,
                             anu.UserWebId,
                             IsPravnoLice = anu.LegalEntityId != null ? "YES" : "NO"
                         }).OrderByDescending(x => x.IsPravnoLice);

            return Ok(query);
        }


        //Проба: Во сесија кои активности ги презема корисникот.Потоа да се најде поврзаност меѓу резултатите.
        //Пример: пребарал компанија -> платил за основни информации -> отворил Сопственици, менаџмент и потружници -> 
        //купил корпоративен извештај.Доколку има повеќе вакви „текови“ да се групираат и пребројат.








        //-------------------------------------------------------------------------------------------------------------

        // Трошење на поени по компанија по модул.
        // Endpoint кој ќе прима ЕМБС на компанија и ќе враќа колку поени потрошиле по модул

        [HttpGet("TroseniPoeniPoKompanijaPoModul")]
        public IActionResult TroseniPoeniPoKompanijaPoModul([FromQuery(Name = "embs")] string embs)
        {
            var query =
                    from a in _db.AspNetUsers
                    join kwl in _db.KbWebPravniLica on a.LegalEntityId equals kwl.LegalEntityId
                    join kwk in _db.KbWebKorisnikAktivnosti on a.UserWebId equals kwk.KorisnikWebId
                    join kwm in _db.KbWebModuli on new { ModulId = kwk.ModulId.GetValueOrDefault(), PodmoduliId = kwk.PodModulId.GetValueOrDefault() } equals new { kwm.ModulId, PodmoduliId = kwm.PodModulId }
                    where kwl.Embs == embs && new[] { 1, 2 }.Contains(kwk.TipUsluga) && kwk.Poeni > 0 && kwk.Cena == 0
                    group kwk by kwm.NazivPodModul into g
                    select new
                    {
                        NazivPodModul = g.Key,
                        TotalPointsSpent = g.Sum(kwk => kwk.Poeni)
                    };

            var result = query.ToList();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(result);
        }


        // Компании кои имаат активни пакети по пакети 
        // (број на компании претплатени на Стандард, Бизнис, Премиум и Персонализиран)
        // (сите персонализирани заедно во еден податок)

        [HttpGet("AktivniPaketiPoKompanija")]
        public IActionResult AktivniPaketiPoKompanija()
        {
            var nazivPaketi = new[] { "Премиум", "Стандард", "Бизнис" };

            var query = from a in _db.AspNetUsers
                        join kwp in _db.KbWebPravniLica on a.LegalEntityId equals kwp.LegalEntityId
                        join kwk in _db.KbWebKorisnikAktivnosti on a.UserWebId equals kwk.KorisnikWebId
                        join kwm in _db.KbWebPaketiM on kwk.PaketId equals kwm.PaketId
                        where a.LegalEntityId != null
                              && kwk.StatusPretplata != null
                              && kwk.StatusPretplata == 2
                              && nazivPaketi.Contains(kwm.NazivPaket.Trim())
                        group kwm by kwm.NazivPaket into grouped
                        select new
                        {
                            NazivPaket = grouped.Key.Trim(),
                            Firmi = grouped.Count()
                        };

            return Ok(query);
        }

        // Купени извештаи по начин плаќање
        //[HttpGet("NacinPlakanjeNaTipUsluga")]
        //public IActionResult NacinPlakanjeNaTipUsluga()
        //{
        //    var query = from a in _db.AspNetUsers
        //                join kwk in _db.KbWebKorisnikAktivnosti on a.UserWebId equals kwk.KorisnikWebId
        //                join kwm in _db.KbWebModuli on new { ModulId = kwk.ModulId.GetValueOrDefault(), PodmoduliId = kwk.PodModulId.GetValueOrDefault() } equals new { kwm.ModulId, PodmoduliId = kwm.PodModulId }
        //                join kwt in _db.KbWebTipUslugi on kwm.TipUsluga equals kwt.TipUsluga
        //                join kwn in _db.KbNacinPlakanje on kwk.NacinPlakanje equals kwn.NacinPlakanje
        //                group new { kwt.TipUsluga, kwn.OpisNacinPlakanje } by new { kwt.TipUsluga, kwn.NacinPlakanje } into g
        //                select new
        //                {
        //                    g.Key.TipUsluga,
        //                    g.Key.NacinPlakanje,
        //                    BrojNaPlakanja = g.Count()
        //                } into tmp
        //                join kwt in _db.KbWebTipUslugi on tmp.TipUsluga equals kwt.TipUsluga
        //                join kwn in _db.KbNacinPlakanje on tmp.NacinPlakanje equals kwn.NacinPlakanje
        //                select new
        //                {
        //                    kwn.OpisNacinPlakanje,
        //                    tmp.BrojNaPlakanja,
        //                    kwt.OpisTipUsluga
        //                };
        //    return Ok(query);
        //}

        // Корисници кои го деактивирале/избришале профилот во истиот месец кога и го направиле
        // (DateIns и DateUpd се во ист месец и UserStatus e 0)

        [HttpGet("KorisniciKreiraniBriseniIstiMesec")]
        public IActionResult KorisniciKreiraniBriseniIstiMesec()
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

        [HttpGet("user-profile-changes/{month}")]
        public IActionResult UsersActivatedAndDeactivatedInAMonth(int month)
        {
            var result = _db.AspNetUsers
                .Where(z => z.DateIns.Substring(4, 2).Contains(month.ToString()))
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

        // Активност на порталот по месец(да се враќаат цела активност заедно со описи на IDs)
        [HttpGet("PortalAktivnostPoMesec")]
        public IActionResult PortalAktivnostPoMesec()
        {
            var result = from activity in _db.KbWebKorisnikAktivnosti
                         let year = activity.DatumVnes.Year
                         let month = activity.DatumVnes.Month
                         group activity by new
                         {
                             KorisnikWebID = activity.KorisnikWebId,
                             YearMonth = new { year, month }
                         } into g
                         select new
                         {
                             g.Key.KorisnikWebID,
                             Month = $"{g.Key.YearMonth.year}-{g.Key.YearMonth.month:D2}", // Format month as "yyyy-MM"
                             TotalSpent = g.Sum(x => x.Cena)
                         };

            return Ok(result);
        }

        //Искористеност на главни филтри според пакетот на кој се претплатени
        //(Според KorisnikWebID и датум на филтрирање да се најде пакетот)

        //[HttpGet("user-filters-usage")]
        //public IActionResult UsedFiltersBasedOnPacket()
        //{
        //    var query = _db.KbWebKorisnikAktivnosti
        //        .GroupBy(wk => new
        //        {
        //            wk.KorisnikWebId,
        //            wk.UserName,
        //            // Extract year, month, and day for the date grouping
        //            wk.DatumVnes.Year,
        //            wk.DatumVnes.Month,
        //            wk.DatumVnes.Day
        //        })
        //        .Select(g => new
        //        {
        //            g.Key.KorisnikWebId,
        //            FilterKoristen = g.Key.UserName,
        //            SumaPoeni = g.Sum(x => x.Poeni),
        //            DatumVnes = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day).ToString("yyyyMMdd")
        //        });
        //    return Ok(query);
        //}

        //Искористеност на главни филтри според пакетот на кој се претплатени
        //(Според KorisnikWebID и датум на филтрирање да се најде пакетот)
        [HttpGet("FiltriKoristeniSporedPretplata")]
        public IActionResult FiltriKoristeniSporedPretplata()
        {
            var result = from kwlf in _db.KbWebLogFilterKriteriumi
                        join kwa in _db.KbWebKorisnikAktivnosti
                        on kwlf.UserId equals kwa.KorisnikWebId
                        join kwm in _db.KbWebPaketiM
                        on kwa.PaketId equals kwm.PaketId
                        group new { kwlf, kwa, kwm } by new { kwlf.UserId, kwlf.Datum, kwm.NazivPaket } into g
                        select new
                        {
                            g.Key.UserId,
                            g.Key.Datum,
                            NazivPaket = g.Key.NazivPaket.Trim(),
                            MainFilterAndSubFilterUsed = g.Count(x => x.kwlf.Sektor != null) + g.Count(x => x.kwlf.Sediste != null)
                        };

            return Ok(result);
        }

        //Искористеност на филтри (групирани според главниот филтер (колони Sektor, Sediste ..)
        //со податоци за подфилтри (вредностите во самите колони))
        [HttpGet("IskoristenostNaFiltri")]
        public IActionResult IskoristenostNaFiltri()
        {
            var result = _db.KbWebLogFilterKriteriumi
                .Where(w => w.Sektor != null || w.Sediste != null)
                .GroupBy(w => new { w.Sektor, w.Sediste })
                .Select(g => new
                {
                    g.Key.Sektor,
                    g.Key.Sediste,
                    Koristenost = g.Count()
                });

            return Ok(result);
        }

    }
}

