using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebPravniLica
{
    public int LegalEntityId { get; set; }

    public string? Embs { get; set; }

    public string? Edb { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress { get; set; }

    public string? City { get; set; }

    public string? ContractPath { get; set; }

    public byte? ContractInd { get; set; }

    public byte? TekSostInd { get; set; }

    public string? DateIns { get; set; }

    public string? DateUpd { get; set; }

    public string? TimeIns { get; set; }

    public string? TimeUpd { get; set; }

    public string? ContractDate { get; set; }

    public string? ContractTime { get; set; }

    public string? UserName { get; set; }

    public string? DogovorBrKorisnik { get; set; }

    public string? DogovorBrMkb { get; set; }

    public bool? IndPotvrdenaKompanija { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();

    public virtual ICollection<KbWebKorisnikPaket> KbWebKorisnikPakets { get; set; } = new List<KbWebKorisnikPaket>();
}
