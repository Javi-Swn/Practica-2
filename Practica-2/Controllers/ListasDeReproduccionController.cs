using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica_2.Models;

namespace Practica_2.Controllers
{
    public class ListasDeReproduccionController : Controller
    {
        private readonly ListasDbContext _context;

        public ListasDeReproduccionController(ListasDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dash()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var usuarioId = int.Parse(userIdClaim.Value);

            var listas = await _context.ListasDeReproduccions
                                        .Where(l => l.UsuarioId == usuarioId)
                                        .ToListAsync();
            return View("~/Views/Home/Dash.cshtml", listas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListasDeReproduccion listasDeReproduccion)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("UserId");
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }

                var usuarioId = int.Parse(userIdClaim.Value);

                listasDeReproduccion.UsuarioId = usuarioId;
                listasDeReproduccion.FechaCreacion = DateTime.Now;

                _context.Add(listasDeReproduccion);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Dash));
            }

            return View(listasDeReproduccion);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listaDeReproduccion = await _context.ListasDeReproduccions.FindAsync(id);
            if (listaDeReproduccion == null)
            {
                return NotFound();
            }
            return View(listaDeReproduccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ListaId, Nombre")] ListasDeReproduccion listaDeReproduccion)
        {
            if (id != listaDeReproduccion.ListaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingLista = await _context.ListasDeReproduccions.FindAsync(id);
                    if (existingLista == null)
                    {
                        return NotFound();
                    }

                    existingLista.Nombre = listaDeReproduccion.Nombre;

                    _context.Update(existingLista);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Dash));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListasDeReproduccionExists(listaDeReproduccion.ListaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(listaDeReproduccion);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listaDeReproduccion = await _context.ListasDeReproduccions
                .FirstOrDefaultAsync(m => m.ListaId == id);
            if (listaDeReproduccion == null)
            {
                return NotFound();
            }

            return View(listaDeReproduccion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listaDeReproduccion = await _context.ListasDeReproduccions.FindAsync(id);

            if (listaDeReproduccion == null)
            {
                return NotFound();
            }

            var canciones = _context.Cancions.Where(c => c.ListaId == id).ToList();

            if (canciones.Any())
            {
                _context.Cancions.RemoveRange(canciones);
            }

            _context.ListasDeReproduccions.Remove(listaDeReproduccion);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dash));
        }

        private bool ListasDeReproduccionExists(int id)
        {
            return _context.ListasDeReproduccions.Any(e => e.ListaId == id);
        }


        public async Task<IActionResult> View(int id)
        {
            var listaDeReproduccion = await _context.ListasDeReproduccions
                .Include(l => l.Cancions)
                .FirstOrDefaultAsync(m => m.ListaId == id);

            if (listaDeReproduccion == null)
            {
                return NotFound();
            }

            return View(listaDeReproduccion); 
        }

        public async Task<IActionResult> Details(int id)
        {
            var listaDeReproduccion = await _context.ListasDeReproduccions
                .Include(l => l.Cancions)
                .FirstOrDefaultAsync(m => m.ListaId == id);

            if (listaDeReproduccion == null)
            {
                return NotFound();
            }

            return View(listaDeReproduccion);
        }

        [HttpPost]
        [Route("ListasDeReproduccion/CreateCancion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCancion(Cancion cancion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cancion);
                await _context.SaveChangesAsync();
                return RedirectToAction("View", "ListasDeReproduccion", new { id = cancion.ListaId });
            }
            return View(cancion);
        }

        [HttpPost]
        [Route("ListasDeReproduccion/DeleteCancion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCancion(int id)
        {
            var cancion = await _context.Cancions.FindAsync(id);
            if (cancion != null)
            {
                var listaId = cancion.ListaId;
                _context.Cancions.Remove(cancion);
                await _context.SaveChangesAsync();
                return RedirectToAction("View", "ListasDeReproduccion", new { id = listaId });
            }
            return NotFound();
        }
    }
}
