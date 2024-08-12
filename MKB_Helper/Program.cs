// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using MKB.Data;
using MKB.Models;



ApplicationDbContext _db = new ApplicationDbContext();
List< AspNetUser> rowsToUpdate = _db.AspNetUsers.Where(u => u.LegalEntityId !=null).ToList();

int id = 1367;
for (int i=0;i<rowsToUpdate.Count && id<=2732;i++)
{
    rowsToUpdate[i].LegalEntityId = id;
    id++;
}

_db.SaveChanges();
