using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace alkemyapi.Models
{
    public class Genero
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Imagen { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        [Column(TypeName = "varchar(100)")]
        public int PeliculaSerieId { get; set; }

        [JsonIgnore]
        public List<PeliculaSerie> PeliculaSeries { get; set; }
    }
}