using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebTipUsluga
{
    public int TipUsluga { get; set; }

    public string? OpisTipUsluga { get; set; }

    public int? GrupaUsluga { get; set; }

    public string? OpisGrupaUsluga { get; set; }

    public virtual ICollection<KbWebModuli> KbWebModulis { get; set; } = new List<KbWebModuli>();
}
