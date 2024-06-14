using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstudoApiCompleta.Model;

namespace EstudoApiCompleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UsuariosController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.FromSqlRaw("SELECT * FROM Usuarios").ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            var usuario = await _context.Usuarios.FromSqlInterpolated($"SELECT * FROM Usuarios WHERE Id = {id}").ToListAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(Guid id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            var affectedRows = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE Usuarios SET Nome = {usuario.Nome}, Email = {usuario.Email}, Senha = {usuario.Senha}, SenhaConfirmacao = {usuario.SenhaConfirmacao}");

            if (affectedRows == 0)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw new DbUpdateConcurrencyException();
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (usuario.Id == Guid.Empty)
            {
                usuario.Id = Guid.NewGuid();
            }

            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO Usuarios(Id, Nome, Email, Senha, SenhaConfirmacao) VALUES ({usuario.Id}, {usuario.Nome}, {usuario.Email}, {usuario.Senha}, {usuario.SenhaConfirmacao})");

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            var usuario = await _context.Usuarios.FromSqlInterpolated($"SELECT * FROM Usuarios WHERE Id = {id}").ToArrayAsync();
            if (usuario == null)
            {
                return NotFound();
            }

            await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Usuarios WHERE Id = {id}");

            return NoContent();
        }

        private bool UsuarioExists(Guid id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
