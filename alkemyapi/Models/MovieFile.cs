using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace alkemyapi.Controllers
{
    public class MovieFile
    {
        [NotMapped]
        public IFormFile File { get; set; }        

        [Column(TypeName = "varchar(100)")]
        public string Titulo { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime FechaCreacion { get; set; }

        public int CalificacionId { get; set; }

        public int PersonajeId { get; set; }       

    }
}