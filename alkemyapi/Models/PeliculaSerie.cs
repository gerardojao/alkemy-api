using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alkemyapi.Models
{
    public class PeliculaSerie
    {
        [Column(TypeName = "varchar(100)")]
        public string Imagen { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Titulo { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime FechaCreacion  { get; set; }

        public int CalificacionId { get; set; }

        public int PersonajeId { get; set; }

        [JsonIgnore]
        public List<Personaje> Personajes { get; set; }
        [JsonIgnore]
        public virtual Calificacion Calificaciones { get; set; }
      
    }
}