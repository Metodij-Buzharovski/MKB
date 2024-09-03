using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class KbWebLogIskoristeniPromoKodovi
{
    public int Id { get; set; }

    public int? KorisnikWebId { get; set; }

    public int? LegalEntityId { get; set; }

    public string Embs { get; set; } = null!;

    public string PromoKod { get; set; } = null!;

    public byte WebTipPromocija { get; set; }

    public int? AktivnostId { get; set; }

    public DateTime DatumVnes { get; set; }

    public string UserName { get; set; } = null!;
}
