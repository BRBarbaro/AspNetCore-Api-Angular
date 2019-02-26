using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Sala
    {
        public int Id { get; set; }

        [Required(ErrorMessage="O nome da sala é obrigatório.")]
        [MaxLength(100,ErrorMessage="O nome deve possuir no máximo 100 caracteres.")]
        public string Nome { get; set; }

        public int Capacidade { get; set; }

        public ICollection<Reserva> Reservas { get; set;}

        [Column("UserId")]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
    }
}
