// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using MKB.Data;
using MKB.Models;



ApplicationDbContext _db = new ApplicationDbContext();
List< AspNetUser> rowsToUpdate = _db.AspNetUsers.Where(u => u.LegalEntityId !=null).ToList();

int id = 2733;
for (int i=0;i<rowsToUpdate.Count; i++)
{
    if(id <= 3732)
    {
        rowsToUpdate[i].LegalEntityId = id;
        id++;
    }
    else
    {
        rowsToUpdate[i].LegalEntityId = null;
    }
    
}

_db.SaveChanges();
