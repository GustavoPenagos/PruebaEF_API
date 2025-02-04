using System;
using System.Collections.Generic;

namespace Api.Data.Context;

public partial class TipoDocumento
{
    public int IdDocumento { get; set; }

    public string Documento { get; set; } = null!;

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
