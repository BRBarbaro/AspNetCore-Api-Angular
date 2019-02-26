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
    public class ReservaController : ControllerBase
    {
        private readonly ApiContext _context;
        private UserManager<Usuario> _userManager;

        public ReservaController(ApiContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Retorna as reservas cadastradas.
        /// </summary>        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Reserva>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas.ToListAsync();
        }

        /// <summary>
        /// Retorna a reserva correspondente ao parametro informado.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Retorna a reserva correspondente ao parametro informado</response>
        /// <response code="404">Reserva não encontrada</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Reserva), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]   
        public async Task<ActionResult<Reserva>> GetReserva(long id)
        {
            var Reserva = await _context.Reservas.FirstOrDefaultAsync(m => m.ReservaId == id);
            if (Reserva == null)
            {
                return NotFound();
            }

            return Reserva;
        }

        /// <summary>
        /// Cadastra uma nova reserva
        /// </summary>
        /// <remarks>
        /// Amostra de objeto:
        ///
        ///     POST /api/reserva
        ///     {  
        ///        "salaId": 1,
        ///        "titulo": "Reuniao geral",
        ///        "inicio: "25/02/2019 09:00",
        ///        "fim": "25/02/2019 12:00"
        ///     }
        ///
        /// </remarks>
        /// <param name="reserva"></param>
        /// <returns>Nova reserva criada</returns>
        /// <response code="201">Retorna a reserva recem criada</response>
        /// <response code="400">Houve falha durante o cadastro</response> 
        [HttpPost]
        [ProducesResponseType(typeof(Reserva), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            var ReservaConcorrente = await _context.Reservas.FirstOrDefaultAsync(x => x.SalaId == reserva.SalaId && 
                                                                                      (x.Inicio >= reserva.Inicio && x.Inicio <= reserva.Fim || 
                                                                                       x.Fim >= reserva.Inicio && x.Fim <= reserva.Fim));

            if (ReservaConcorrente != null)
            {
                return BadRequest(new {message = "Já existe uma reserva para esta sala no período informado."});
            }

            if (reserva.Fim < reserva.Inicio)
            {
                return BadRequest(new {message = "O período final deve ser posterior ao período inicial."});
            }
            
            reserva.UsuarioId = User.Claims.First(c => c.Type == "UserID").Value;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.ReservaId }, reserva);
        }

        /// <summary>
        /// Atualiza uma reserva localizada pelo identificador 
        /// </summary>
        /// <remarks>
        /// Amostra de objeto:
        ///
        ///     PUT /api/reserva/{id}
        ///     {  
        ///        "id":1,
        ///        "salaId": 1,
        ///        "titulo": "Reuniao geral",
        ///        "inicio: "25/02/2019 09:00",
        ///        "fim": "25/02/2019 15:00"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="reserva"></param>
        /// <response code="204">Reserva atualizada</response>
        /// <response code="400">Houve falha durante a alteração</response>         
        /// <response code="404">Reserva não encontrada</response>  
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]  
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  
        [ProducesResponseType(StatusCodes.Status404NotFound)]          
        public async Task<IActionResult> PutReserva(long id, Reserva reserva)
        {
            if (id != reserva.ReservaId)
            {
                return BadRequest(new { message = "Identificador fornecido não corresponde a reserva informada."});
            }

            if (reserva.UsuarioId != User.Claims.First(c => c.Type == "UserID").Value)
            {
                return BadRequest(new { message = "Apenas o criador da reserva poderá alterá-la."});
            }

            if (!_context.Reservas.Any(x => x.ReservaId == id))
            {
                return NotFound();
            }

            var ReservaConcorrente = await _context.Reservas.FirstOrDefaultAsync(x => x.SalaId == reserva.SalaId && 
                                                                                      x.ReservaId != reserva.ReservaId &&
                                                                                      (x.Inicio >= reserva.Inicio && x.Inicio <= reserva.Fim || 
                                                                                       x.Fim >= reserva.Inicio && x.Fim <= reserva.Fim));

            if (ReservaConcorrente != null)
            {
                return BadRequest(new {message = "Já existe uma reserva para esta sala no período informado."});
            }            

            _context.Entry(reserva).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deleta uma reserva localizada pelo identificador
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Reserva deletada</response>  
        /// <response code="400">Houve falha durante a deleção</response>           
        /// <response code="404">Reserva não encontrada</response>   
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]  
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            if (reserva.UsuarioId != User.Claims.First(c => c.Type == "UserID").Value)
            {
                return BadRequest(new { message = "Apenas o criador da reserva poderá excluí-la."});
            }            

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
