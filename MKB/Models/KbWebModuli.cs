using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebModuli
{
    public int ModulId { get; set; }

    public string NazivModul { get; set; } = null!;

    public int PodModulId { get; set; }

    public string NazivPodModul { get; set; } = null!;

    public int TipUsluga { get; set; }

    public int Poeni { get; set; }

    public decimal? Cena { get; set; }

    public decimal? CenaFl { get; set; }

    public DateTime DatumVnes { get; set; }

    public string UserName { get; set; } = null!;

    public int? Id { get; set; }

    public int? Idpovrzanost { get; set; }

    public virtual KbWebTipUsluga TipUslugaNavigation { get; set; } = null!;
}
