using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbWebLogFilterKriterium
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Sektor { get; set; }

    public string? Sediste { get; set; }

    public string? GodinaOsnovanje { get; set; }

    public string? TipKompanija { get; set; }

    public string? Prihod { get; set; }

    public string? Profit { get; set; }

    public string? BrVrab { get; set; }

    public string? Izlozenost { get; set; }

    public string? Konkurencija { get; set; }

    public string? KonkurencijaKompanija { get; set; }

    public string? Datum { get; set; }

    public string? Vreme { get; set; }
}
