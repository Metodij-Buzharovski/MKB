using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebPaketiM
{
    public int PaketId { get; set; }

    public string NazivPaket { get; set; } = null!;

    public string? OpisNazivPaket { get; set; }

    public byte? PeriodPretplata { get; set; }

    public byte? StatusPaket { get; set; }

    public decimal? CenaPaket { get; set; }

    public short? BrPoeni { get; set; }

    public decimal? CenaPoen { get; set; }

    public bool? DodadiDopKorisnik { get; set; }

    public decimal? CenaDopKorisnik { get; set; }

    public short? BrSubjektiMonitoring { get; set; }

    public decimal? CenaSubjektMonitoring { get; set; }

    public byte? TipPaket { get; set; }

    public string? VkluceniFiltri { get; set; }

    public bool? IndPromena { get; set; }

    public DateTime DatumVnes { get; set; }

    public string UserName { get; set; } = null!;

    public virtual ICollection<KbWebKorisnikPaket> KbWebKorisnikPakets { get; set; } = new List<KbWebKorisnikPaket>();
}
