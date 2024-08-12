using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebKorisnikAktivnost
{
    public int Id { get; set; }

    public int KorisnikWebId { get; set; }

    public short TipUsluga { get; set; }

    public int? ModulId { get; set; }

    public int? PodModulId { get; set; }

    public int? PaketId { get; set; }

    public short? Poeni { get; set; }

    public short? DopKorisnici { get; set; }

    public short? DopMonitoringSubjekti { get; set; }

    public decimal? CenaDopMonitoringSubjekti { get; set; }

    public decimal? Cena { get; set; }

    public byte? NacinPlakanje { get; set; }

    public bool? IndPlateno { get; set; }

    public byte? StatusPretplata { get; set; }

    public long SessionId { get; set; }

    public int? BaranjeWebId { get; set; }

    public DateTime DatumVnes { get; set; }

    public string UserName { get; set; } = null!;

    public virtual KbBaranjaWeb? BaranjeWeb { get; set; }

    public virtual AspNetUser KorisnikWeb { get; set; } = null!;

    public virtual KbNacinPlakanje? NacinPlakanjeNavigation { get; set; }
}
