using System;
using System.Collections.Generic;

namespace Practica_2.Models;

public partial class Cancione
{
    public int CancionId { get; set; }

    public int ListaId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Artista { get; set; }

    public string? Album { get; set; }

    public TimeOnly? Duracion { get; set; }

    public string? Imagen { get; set; }

    public DateTime? FechaAgregada { get; set; }

    public virtual ListasDeReproduccion Lista { get; set; } = null!;
}
