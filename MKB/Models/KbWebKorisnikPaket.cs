using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebKorisnikPaket
{
    public int KorisnikWebId { get; set; }

    public int? LegalEntityId { get; set; }

    public short GratisPoeni { get; set; }

    public int? PaketId { get; set; }

    public short? VkupnoPoeni { get; set; }

    public short? DopolnitelniPoeni { get; set; }

    public short? BrDopKorisnici { get; set; }

    public short? BrDopSubjektiMonitoring { get; set; }

    public DateTime? DatPocPaket { get; set; }

    public DateTime? DatKrajPaket { get; set; }

    public short? PreostanatiPoeni { get; set; }

    public DateTime DatumVnes { get; set; }

    public string UserName { get; set; } = null!;

    public virtual AspNetUser KorisnikWeb { get; set; } = null!;

    public virtual KbWebPravniLica? LegalEntity { get; set; }

    public virtual KbWebPaketiM? Paket { get; set; }
}
