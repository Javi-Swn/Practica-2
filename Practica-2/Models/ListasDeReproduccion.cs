using System;
using System.Collections.Generic;

namespace Practica_2.Models;

public partial class ListasDeReproduccion
{
    public int ListaId { get; set; }

    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Cancione> Canciones { get; set; } = new List<Cancione>();

    public virtual Usuario Usuario { get; set; } = null!;
}
