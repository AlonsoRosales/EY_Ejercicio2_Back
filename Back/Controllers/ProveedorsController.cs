using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back.Models;

namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProveedorsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Proveedors - Listado de los proveedores ordenados por la fecha de modificación
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedors(){
            return await _context.Proveedors.OrderByDescending(p => p.FechaEdicion).ToListAsync();
        }


        // GET: api/Proveedors/5 - Búsqueda de un proveedor por su id
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(string id){
            if(int.TryParse(id, out int intId))
            {
                var proveedor = await _context.Proveedors.FindAsync(intId);
                if(proveedor == null)
                {
                    return NotFound(new { message = $"No se encontró el proveedor con ID: {intId}" });
                }

                return proveedor;
            }
            else
            {
                return BadRequest(new { message = "El ID proporcionado no es válido" });
            }
        }

        // PUT: api/Proveedors - Actualización de información de un proveedor
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(string id, [FromForm] string? razonsocial, 
                                                      [FromForm] string? nombrecomercial,
                                                      [FromForm] string? idtributaria, [FromForm] string? telefono,
                                                      [FromForm] string? correo, [FromForm] string? web,
                                                      [FromForm] string? direccion, [FromForm] string? pais,
                                                      [FromForm] decimal facturacion)
        {
            if (!int.TryParse(id, out int intId))
            {
                return BadRequest(new { message = "El ID proporcionado no es válido" });
            }

            var proveedor = await _context.Proveedors.FindAsync(intId);
            if (proveedor == null)
            {
                return NotFound(new { message = $"No se encontró el proveedor con ID: {intId}" });
            }

            if (!string.IsNullOrWhiteSpace(razonsocial))
                proveedor.RazonSocial = razonsocial;
            if (!string.IsNullOrWhiteSpace(nombrecomercial))
                proveedor.NombreComercial = nombrecomercial;
            if (!string.IsNullOrWhiteSpace(idtributaria))
                proveedor.IdTributaria = long.Parse(idtributaria);
            if (!string.IsNullOrWhiteSpace(telefono))
                proveedor.Telefono = long.Parse(telefono);
            if (!string.IsNullOrWhiteSpace(correo))
                proveedor.Correo = correo;
            if (!string.IsNullOrWhiteSpace(web))
                proveedor.Web = web;
            if (!string.IsNullOrWhiteSpace(direccion))
                proveedor.Direccion = direccion;
            if (!string.IsNullOrWhiteSpace(pais))
                proveedor.Pais = pais;
            if (facturacion >0)
                proveedor.Facturacion = facturacion;

            proveedor.FechaEdicion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Proveedor actualizado exitosamente" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error al actualizar el proveedor" });
            }
        }

        // POST: api/Proveedors - Registro de proveedores
        [HttpPost]
        public async Task<ActionResult> PostProveedor([FromForm] string? razonsocial, [FromForm] string? nombrecomercial,
                                                      [FromForm] string? idtributaria, [FromForm] string? telefono,
                                                      [FromForm] string? correo, [FromForm] string? web,
                                                      [FromForm] string? direccion, [FromForm] string? pais,
                                                      [FromForm] decimal facturacion)
        {
            if (string.IsNullOrWhiteSpace(razonsocial) || string.IsNullOrWhiteSpace(nombrecomercial) || string.IsNullOrWhiteSpace(idtributaria) ||
                string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(web) ||
                string.IsNullOrWhiteSpace(direccion) || string.IsNullOrWhiteSpace(pais) ||  facturacion <= 0 || idtributaria.Length != 11
                || !correo.Contains("@") || !Uri.IsWellFormedUriString(web, UriKind.Absolute))
            {
                return BadRequest(new { message = "Datos inválidos o incompletos" });
            }

            var proveedor = new Proveedor
            {
                RazonSocial = razonsocial,
                NombreComercial = nombrecomercial,
                IdTributaria = long.Parse(idtributaria),
                Telefono = long.Parse(telefono),
                Correo = correo,
                Web = web,
                Direccion = direccion,
                Pais = pais,
                Facturacion = facturacion,
                FechaEdicion = DateTime.Now
            };

            try
            {
                _context.Proveedors.Add(proveedor);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Proveedor creado exitosamente" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error al crear el proveedor" });
            }
        }


        // DELETE: api/Proveedors/5 -  Eliminación de proveedor
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(string id)
        {
            if(int.TryParse(id, out int intId))
            {
                var proveedor = await _context.Proveedors.FindAsync(intId);
                if (proveedor == null)
                {
                    return NotFound(new { message = $"No se encontró el proveedor con ID: {intId}" });
                }

                try
                {
                    _context.Proveedors.Remove(proveedor);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Proveedor eliminado exitosamente" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500, new { message = "Ha ocurrido un error al eliminar el proveedor" });
                }

            }
            else
            {
                return BadRequest(new { message = "El ID proporcionado no es válido." });
            }
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedors.Any(e => e.IdProveedor == id);
        }
    }
}
