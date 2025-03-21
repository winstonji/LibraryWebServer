using System;
using System.Collections.Generic;

namespace LibraryWebServer.Models;

public partial class Patrons
{
    public uint CardNum { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CheckedOut> CheckedOut { get; } = new List<CheckedOut>();

    public virtual ICollection<Phones> Phones { get; } = new List<Phones>();
}
