using System;
using System.Collections.Generic;

namespace Practica_2.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<ListasDeReproduccion> ListasDeReproduccions { get; set; } = new List<ListasDeReproduccion>();
}
