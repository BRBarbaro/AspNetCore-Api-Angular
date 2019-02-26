using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Reserva
    {
        public int ReservaId { get; set; }

        [Required(ErrorMessage="O título da reserva é obrigatório.")]
        [MaxLength(100,ErrorMessage="O título deve possuir no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage="O período inicial da reserva é obrigatório.")]
        public DateTime Inicio { get; set; }

        [Required(ErrorMessage="O período final da reserva é obrigatório.")]
        public DateTime Fim { get; set; }

        [Required(ErrorMessage="Chave estrangeira da sala é obrigatória.")]
        public int SalaId { get; set; }

        public Sala Sala { get; set; }

        [Column("UserId")]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
    }
}