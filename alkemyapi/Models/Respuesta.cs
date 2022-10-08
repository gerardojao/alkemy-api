using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alkemyapi.Models
{
        public class Respuesta<T>
        {
            public int Ok { get; set; }

            public List<T> Data { get; set; } = new List<T>();
            public string Message { get; set; }
        }
    
}
