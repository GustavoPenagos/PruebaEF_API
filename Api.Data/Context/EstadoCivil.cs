using System;
using System.Collections.Generic;

namespace Api.Data.Context;

public partial class EstadoCivil
{
    public int IdEstadoCivil { get; set; }

    public string EstadoCivil1 { get; set; } = null!;

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
