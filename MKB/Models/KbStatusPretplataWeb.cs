using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbStatusPretplataWeb
{
    public byte StatusPretplata { get; set; }

    public string OpisStatusPretplata { get; set; } = null!;

    public byte OrderId { get; set; }

    public virtual ICollection<KbWebKorisnikAktivnost> KbWebKorisnikAktivnosts { get; set; } = new List<KbWebKorisnikAktivnost>();
}
