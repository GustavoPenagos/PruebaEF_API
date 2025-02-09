using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Api.Data.Context;

public partial class PersonaDto
{
    [JsonPropertyName("Nombres")]
    public string? Nombres { get; set; }

    [JsonPropertyName("Apellidos")]
    public string? Apellidos { get; set; }


    [JsonPropertyName("TipoDocumento")]
    public string? TipoDocumento { get; set; }


    [JsonPropertyName("NroDocumento")]
    public string? Documento { get; set; }

    [JsonPropertyName("EstadoCivil")]
    public string? EstadoCivil { get; set; }

    [JsonPropertyName("FechaNacimiento")]
    public string? FechaNacmiento { get; set; }

    [JsonPropertyName("Fecha")]
    public string? Fecha { get; set; }

    [JsonPropertyName("ValorGanar")]
    public decimal ValorGanar { get; set; }

    //[JsonPropertyName("Fecha")]
    //public string Fecha { get; set; }

}
