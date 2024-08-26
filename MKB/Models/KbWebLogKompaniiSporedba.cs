using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebLogKompaniiSporedba
{
    public int Id { get; set; }

    public int? KorisnikWebId { get; set; }

    public long? SessionId { get; set; }

    public string? Embs { get; set; }

    public string? Edb { get; set; }

    public int? Godina { get; set; }

    public string? Datum { get; set; }

    public string? Vreme { get; set; }

    public string? AktivnostId { get; set; }
}
