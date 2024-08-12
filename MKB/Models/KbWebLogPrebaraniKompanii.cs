using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebLogPrebaraniKompanii
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Datum { get; set; } = null!;

    public string Vreme { get; set; } = null!;

    public string? Embs { get; set; }

    public string? Edb { get; set; }

    public string? Naziv { get; set; }

    public string? Adresa { get; set; }

    public string? Sediste { get; set; }

    public byte TipPrebaruvanje { get; set; }

    public long? SessionId { get; set; }

    public int? AktivnostId { get; set; }
}
