using System;
using System.Collections.Generic;

namespace MKB.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public int? UserWebId { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Embg { get; set; }

    public string? JobTitle { get; set; }

    public bool? ConfirmedIdentity { get; set; }

    public byte? UserType { get; set; }

    public byte? UserStatus { get; set; }

    public bool? MkbnewsInd { get; set; }

    public string? DateIns { get; set; }

    public string? TimeIns { get; set; }

    public string? DateUpd { get; set; }

    public string? TimeUpd { get; set; }

    public int? LegalEntityId { get; set; }

    public int AccessFailedCount { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public string? NormalizedEmail { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordUpd { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public string? SecurityStamp { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public string? UserName { get; set; }

    public bool? OpstiUsloviInd { get; set; }

    public int? PaketId { get; set; }

    public byte? UserRole { get; set; }

    public bool? Mkbadmin { get; set; }

    public bool? IsOldUser { get; set; }

    public virtual ICollection<KbWebKorisnikAktivnost> KbWebKorisnikAktivnosts { get; set; } = new List<KbWebKorisnikAktivnost>();

    public virtual KbWebKorisnikPaket? KbWebKorisnikPaket { get; set; }

    public virtual KbWebPravniLica? LegalEntity { get; set; }
}
