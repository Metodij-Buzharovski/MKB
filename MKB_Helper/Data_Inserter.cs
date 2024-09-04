using MKB.Data;
using MKB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKB_Helper
{
    internal static class Data_Inserter
    {
        private static ApplicationDbContext _db = new ApplicationDbContext();
        public static void FixDataForWebLogIskoristeniPromoKodovi()
        {

            var populatedRows = (from wka in _db.KbWebKorisnikAktivnosti
                                 join anu in _db.AspNetUsers
                                 on wka.KorisnikWebId equals anu.UserWebId
                                 where anu.LegalEntityId != null
                                 select new
                                 {
                                     anu.UserWebId,
                                     anu.LegalEntityId
                                 })
                                  .Distinct()
                                  .ToList();

            var unpopulatedRows = _db.KbWebLogIskoristeniPromoKodovi.ToList();

            Random rnd = new Random();
            int promoKodCount = _db.KbWebLogIskoristeniPromoKodovi.Count();
            for (int i = 0; i < promoKodCount; i++)
            {
                int randomTimes = rnd.Next(0, 21);
                int randomRow = rnd.Next(1, promoKodCount);
                for (int j = 0; i + j < promoKodCount && j < randomTimes; j++)
                {
                    unpopulatedRows[i + j].KorisnikWebId = populatedRows[randomRow].UserWebId;
                    unpopulatedRows[i + j].LegalEntityId = populatedRows[randomRow].LegalEntityId;
                }
                i += randomTimes;
            }

            _db.SaveChanges();
        }


        public static void FixDataForWebKorisnikPaket()
        {
            List<AspNetUser> users = _db.AspNetUsers.ToList();
            List<KbWebKorisnikPaket> paketi = _db.KbWebKorisnikPaketi.ToList();
            foreach (KbWebKorisnikPaket korisnikPaket in paketi)
            {
                var tmp = users.First(u => u.UserWebId == korisnikPaket.KorisnikWebId);
                korisnikPaket.LegalEntityId = tmp.LegalEntityId;
            }

            _db.SaveChanges();
        }
    }
}
