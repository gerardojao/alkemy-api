using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace alkemyapi.Models
{
    public class Personaje
    {
        public Personaje()
        {
            PeliculaSeries = new HashSet<PeliculaSerie>();
        }
        [Column(TypeName = "varchar(100)")]
        public string Imagen { get; set; }
        //[NotMapped]
        //public IFormFile File { get; set; }

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; }

        public int? Edad { get; set; }

        public int? Peso { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Historia { get; set; }

        public int PeliculaSerieId { get; set; }

        [JsonIgnore]
        public ICollection<PeliculaSerie> PeliculaSeries { get; set; }
    }
}
