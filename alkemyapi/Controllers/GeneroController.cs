using alkemyapi.Data;
using alkemyapi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace alkemyapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;
        private readonly IWebHostEnvironment _env;

        public GeneroController(IConfiguration appsettings, AppDbContext context, IRepository repository, IWebHostEnvironment env)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
            _env = env;
        }
        //Ssaved images
        [HttpPost("create")]
        public async Task<ActionResult> CreateCharacter([FromForm] Genero genero)
        {
            Respuesta<object> respuesta = new();
            try
            {
                if (genero != null)
                {
                    string guidImagen = null;
                    if (genero.File != null)
                    {
                        string ficherosImagenes = Path.Combine(_env.WebRootPath, "File");
                        guidImagen = Guid.NewGuid().ToString() + genero.File.FileName;
                        string ruta = Path.Combine(ficherosImagenes, guidImagen);
                        await genero.File.CopyToAsync(new FileStream(ruta, FileMode.Create));
                    }
                    Genero gender = new();
                    gender.Imagen = guidImagen;
                    gender.Nombre = genero.Nombre;
                    gender.PeliculaSerieId = genero.PeliculaSerieId;

                    await _repository.CreateAsync(gender);
                    respuesta.Ok = 1;
                    respuesta.Message = "Character registered successfully";
                }

            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
                return Ok(respuesta);
            }
            return Ok(respuesta);
        }
    }
}
