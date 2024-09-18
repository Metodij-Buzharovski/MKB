using System;
using System.Collections.Generic;

namespace MKB_API.Models;

public partial class KbVidKorIzv
{
    public byte VidKorIzv { get; set; }

    public string OpisVidKorIzv { get; set; } = null!;

    public bool IndAcdizv { get; set; }

    public bool IndNevazDok { get; set; }

    public byte IndWebApp { get; set; }

    public bool IndWebSvc { get; set; }

    public bool IndDuplikat { get; set; }

    public bool IndwsBlokada { get; set; }

    public bool IndPrikazMkbadmin { get; set; }

    public bool IndDnevnaObrabotka { get; set; }
}
