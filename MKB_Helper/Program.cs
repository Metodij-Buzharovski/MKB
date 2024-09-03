// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using MKB.Data;
using MKB.Models;



ApplicationDbContext _db = new ApplicationDbContext();
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
for (int i=0; i<promoKodCount; i++)
{
    int randomTimes = rnd.Next(0, 21);
    int randomRow = rnd.Next(1, promoKodCount);
    for (int j = 0; i+j < promoKodCount && j < randomTimes; j++)
    {
        unpopulatedRows[i + j].KorisnikWebId = populatedRows[randomRow].UserWebId;
        unpopulatedRows[i + j].LegalEntityId = populatedRows[randomRow].LegalEntityId;
    }
    i += randomTimes;
}

_db.SaveChanges();
