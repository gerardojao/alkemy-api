using alkemyapi.Data;
using alkemyapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace alkemyapi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PersonajesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;


        public PersonajesController(IConfiguration appsettings, AppDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;

        }

        //GET: api/<PersonajesController>
            [HttpGet("character")]
        // public async Task<ActionResult> GetCharacter(string token)
        public async Task<ActionResult<IEnumerable<Personaje>>> GetAllPersonajes(string token)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.Users.Where(u => u.VerificationCode == token).FirstOrDefaultAsync();
                if (user != null)
                {
                    var character = await _repository.SelectAll<Personaje>();

                    if (character != null)
                    {                                  
                        foreach (var item in character)
                        {
                            respuesta.Data.Add(new
                            {
                                item.Nombre,
                                item.Imagen
                            });
                        }
                        respuesta.Ok = 1;
                        respuesta.Message = "Personajes registrados";
                    }                                  
                    else
                    {
                        respuesta.Ok = 0;
                        respuesta.Message = "No hay Personajes registrados";
                        return BadRequest(respuesta);
                    }
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Requiere token para continuar";
                    return BadRequest(respuesta);
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.ToString() + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

       
    }
}
