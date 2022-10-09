using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alkemyapi.Models
{
    public class PersonajeFile
    {
       
        [NotMapped]
        public IFormFile File { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; }

        public int? Edad { get; set; }

        public int? Peso { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Historia { get; set; }

        public int PeliculaSerieId { get; set; }

    }
}
