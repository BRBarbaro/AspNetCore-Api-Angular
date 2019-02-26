using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Projeto.Models;
using Projeto.Controllers;

namespace Projeto.Data
{
    public static class DbSeed
    {
        public static void Seed(ApiContext context/*, UserManager<Usuario> userManager*/)
        {
            context.Database.EnsureCreated();

            if (context.Salas.Any())
            {
                return;
            }

            var salas = new Sala[]
            {
                new Sala{Id=1,Nome="Aroeira",Capacidade=12},
                new Sala{Id=2,Nome="Pinheiro",Capacidade=15},
                new Sala{Id=3,Nome="Cerejeira",Capacidade=13}
            };
            
            foreach(Sala x in salas)
            {
                context.Salas.Add(x);
            }
            context.SaveChanges();

        }
    }
}