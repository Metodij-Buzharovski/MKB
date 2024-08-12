using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbNacinPlakanje
{
    public byte NacinPlakanje { get; set; }

    public string OpisNacinPlakanje { get; set; } = null!;

    public virtual ICollection<KbBaranjaWeb> KbBaranjaWebs { get; set; } = new List<KbBaranjaWeb>();

    public virtual ICollection<KbWebKorisnikAktivnost> KbWebKorisnikAktivnosts { get; set; } = new List<KbWebKorisnikAktivnost>();
}
