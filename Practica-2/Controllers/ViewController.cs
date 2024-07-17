using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica_2.Models;
using System.Threading.Tasks;

namespace Practica_2.Controllers
{
    public class ViewController : Controller
    {
        private readonly ListasDbContext _context;

        public ViewController(ListasDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PlaylistDetails(int id)
        {
            var listaDeReproduccion = await _context.ListasDeReproduccions
                .Include(l => l.Cancions)
                .FirstOrDefaultAsync(m => m.ListaId == id);

            if (listaDeReproduccion == null)
            {
                return NotFound();
            }

            return View("PlaylistDetails", listaDeReproduccion); // Especifica el nombre de la vista aquí
        }
    }
}