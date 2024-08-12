using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbIzvestaiWeb
{
    public int IzvestajWebId { get; set; }

    public int KorisnikWebId { get; set; }

    public byte TipIzvestaj { get; set; }

    public byte VidKorIzv { get; set; }

    public string DatumIzv { get; set; } = null!;

    public string VremeIzv { get; set; } = null!;

    public string IzvestajXml { get; set; } = null!;

    public bool IndNovIzv { get; set; }

    public byte StatusIzv { get; set; }

    public int? LegalEntityId { get; set; }

    public virtual ICollection<KbBaranjaWeb> KbBaranjaWebs { get; set; } = new List<KbBaranjaWeb>();
}
