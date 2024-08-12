using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbBaranjaWeb
{
    public int BaranjeWebId { get; set; }

    public int KorisnikWebId { get; set; }

    public decimal? Iznos { get; set; }

    public int? PretplataId { get; set; }

    public int? IzvestajWebId { get; set; }

    public byte StatusBaranjeWeb { get; set; }

    public string DatumBaranje { get; set; } = null!;

    public string VremeBaranje { get; set; } = null!;

    public byte? NacinPlakanje { get; set; }

    public byte TipIzvestaj { get; set; }

    public byte VidKorIzv { get; set; }

    public virtual KbIzvestaiWeb? IzvestajWeb { get; set; }

    public virtual ICollection<KbWebKorisnikAktivnost> KbWebKorisnikAktivnosts { get; set; } = new List<KbWebKorisnikAktivnost>();

    public virtual KbNacinPlakanje? NacinPlakanjeNavigation { get; set; }

    public virtual KbStatusBaranjeWeb StatusBaranjeWebNavigation { get; set; } = null!;
}
