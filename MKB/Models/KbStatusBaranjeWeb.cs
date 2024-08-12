using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class KbStatusBaranjeWeb
{
    public byte StatusBaranjeWeb { get; set; }

    public string? OpisStatusBaranjeWeb { get; set; }

    public virtual ICollection<KbBaranjaWeb> KbBaranjaWebs { get; set; } = new List<KbBaranjaWeb>();
}
