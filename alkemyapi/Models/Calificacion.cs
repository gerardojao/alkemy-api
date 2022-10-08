using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alkemyapi.Models
{
    public class Calificacion
    {
        [Key]
        public int Id { get; set; }

        public int CalificationNumber { get; set; }

        public List<PeliculaSerie> PeliculaSeries { get; set; }
    }
}