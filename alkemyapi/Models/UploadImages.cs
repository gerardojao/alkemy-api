using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alkemyapi.Models
{
    public class UploadImages
    {
        public IFormFile File { get; set; }
    }
}
