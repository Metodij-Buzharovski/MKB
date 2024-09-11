using System;
using System.Collections.Generic;

namespace MKB_API.Models;

public partial class KbWebPromoKodovi
{
    public string PromoKod { get; set; } = null!;

    public string ImePromocija { get; set; } = null!;

    public int TipUsluga { get; set; }

    public decimal? Cena { get; set; }

    public byte StatusKod { get; set; }

    public int? PaketId { get; set; }

    public DateTime? DatumPocVaznost { get; set; }

    public DateTime? DatumKrajVaznost { get; set; }

    public DateTime DatumVnes { get; set; }

    public string UserName { get; set; } = null!;

    public byte? WebTipPromocija { get; set; }

    public int? PrviNkompanii { get; set; }

    public decimal? ProcentPopust { get; set; }

    public int? BrojIskoristenost { get; set; }
}
