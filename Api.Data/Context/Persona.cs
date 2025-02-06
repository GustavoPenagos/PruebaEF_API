using System;
using System.Collections.Generic;

namespace Api.Data.Context;

public partial class Persona
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string? Apellido { get; set; }

    public int IdDocumento { get; set; }

    public string Documento { get; set; } = null!;

    public DateOnly FechaNcacimiento { get; set; }

    public DateTime Fecha { get; set; }

    public decimal ValorGanar { get; set; }

    public int? IdEstadoCivil { get; set; }

    public virtual TipoDocumento IdDocumentoNavigation { get; set; } = null!;

    public virtual EstadoCivil? IdEstadoCivilNavigation { get; set; }
}
