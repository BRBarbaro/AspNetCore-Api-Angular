using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Projeto.Models
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }

        public ICollection<Sala> Salas { get; set;}

        public ICollection<Reserva> Reservas { get; set;}
    }
}