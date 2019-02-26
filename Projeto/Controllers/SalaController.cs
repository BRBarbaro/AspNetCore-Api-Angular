using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;

namespace Projeto.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class SalaController : ControllerBase
    {
        private readonly ApiContext _context;
        private UserManager<Usuario> _userManager;

        public SalaController(ApiContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Retorna as salas cadastradas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Sala>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Sala>>> GetSalas()
        {
            return await _context.Salas.ToListAsync();
        }

        /// <summary>
        /// Retorna a sala correspondente ao parametro informado.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Retorna a sala correspondente ao parametro informado</response>
        /// <response code="404">Sala não encontrada</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Sala), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]      
        public async Task<ActionResult<Sala>> GetSala(long id)
        {
            var sala = await _context.Salas.Include(x => x.Reservas).FirstOrDefaultAsync(y => y.Id == id);
                
            if (sala == null)
            {
                return NotFound();
            }

            return sala;
        }

        /// <summary>
        /// Cadastra uma nova sala
        /// </summary>
        /// <remarks>
        /// Amostra de objeto:
        ///
        ///     POST /api/sala
        ///     {
        ///        "nome": "Sala",
        ///        "capacidade": 0
        ///     }
        ///
        /// </remarks>
        /// <param name="sala"></param>
        /// <returns>Nova sala criada</returns>
        /// <response code="201">Retorna a sala recem criada</response>
        /// <response code="400">Houve falha durante o cadastro</response> 
        [HttpPost]
        [ProducesResponseType(typeof(Sala), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]           
        public async Task<ActionResult<Sala>> PostSala(Sala sala)
        {
            sala.UsuarioId = User.Claims.First(c => c.Type == "UserID").Value;
            _context.Salas.Add(sala);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSala), new { id = sala.Id }, sala);
        }

        /// <summary>
        /// Atualiza uma sala localizada pelo identificador 
        /// </summary>
        /// <remarks>
        /// Amostra de objeto:
        ///
        ///     PUT /api/sala/{id}
        ///     {
        ///        "id": 1    
        ///        "nome": "Sala",
        ///        "capacidade": 0
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="sala"></param>
        /// <response code="204">Sala atualizada</response>
        /// <response code="400">Houve falha durante a alteração</response>         
        /// <response code="404">Sala não encontrada</response>         
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]  
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]  
        public async Task<IActionResult> PutSala(long id, Sala sala)
        {
            if (id != sala.Id)
            {
                return BadRequest(new { message = "Identificador fornecido não corresponde a sala informada."});
            }

            if (sala.UsuarioId != User.Claims.First(c => c.Type == "UserID").Value)
            {
                return BadRequest(new { message = "Apenas o criador da sala poderá alterá-la."});
            }

            if (!_context.Salas.Any(x => x.Id == id))
            {
                return NotFound();
            }

            _context.Entry(sala).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deleta uma sala localizada pelo identificador
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Sala deletada</response>  
        /// <response code="400">Houve falha durante a deleção</response>           
        /// <response code="404">Sala não encontrada</response>           
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]    
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<IActionResult> DeleteSala(int id)
        {
            var sala = await _context.Salas.FindAsync(id);

            if (sala == null)
            {
                return NotFound();
            }

            if (sala.UsuarioId != User.Claims.First(c => c.Type == "UserID").Value)
            {
                return BadRequest(new { message = "Apenas o criador da sala poderá excluí-la."});
            }

            _context.Salas.Remove(sala);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
