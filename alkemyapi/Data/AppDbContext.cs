using alkemyapi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alkemyapi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<PeliculaSerie> PeliculaSeries { get; set; }
        public DbSet<Personaje> Personajes { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
    }
}
