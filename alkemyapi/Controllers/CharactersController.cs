﻿using alkemyapi.Data;
using alkemyapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace alkemyapi.Controllers
{
   //[Authorize]
    [Route("character")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;
        private readonly IWebHostEnvironment _env;

        public CharactersController(IConfiguration appsettings, AppDbContext context, IRepository repository, IWebHostEnvironment env)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
            _env = env;
        }

        //GET: api/<CharactersController>
       [HttpGet]
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

        ////GET: api/<UsersController>
        //[HttpGet]
        //public async Task<ActionResult> GetByQueries()
        //{
        //    var character = await _repository.SelectAll<Personaje>();
        //    return Ok(character);
        //}
        // GET: query parameters NAME
        [HttpGet(" ")]
        public async Task<ActionResult> GetCharacterByQueries([FromQuery]string? name, int? age, int? idMovie)
        {
            Respuesta<object> respuesta = new();
            try
            {
                if (name==null && age == null && idMovie == null) 
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Debe suministrar algún parametro";
                } else if(name != null)
                {
                    var _person = await _context.Personajes.Where(p => p.Nombre == name).FirstOrDefaultAsync();
                    respuesta.Data.Add(new
                    {
                        _person.Imagen,
                        _person.Nombre,
                        _person.Edad,
                        _person.Peso,
                        _person.Historia,
                        Titulo = (from pel in _context.PeliculaSeries
                                  where pel.PersonajeId == _person.Id
                                  select (pel.Titulo)).ToList()
                    });
                    respuesta.Ok = 1;
                    respuesta.Message = "Detalle del personaje por Nombre";
                }
                else if (age != null && name == null && idMovie == null)
                {
                    var _person = await _context.Personajes.Where(p => p.Edad == age).FirstOrDefaultAsync();
                    respuesta.Data.Add(new
                    {
                        _person.Imagen,
                        _person.Nombre,
                        _person.Edad,
                        _person.Peso,
                        _person.Historia,
                        Titulo = (from pel in _context.PeliculaSeries
                                  where pel.PersonajeId == _person.Id
                                  select (pel.Titulo)).ToList()
                    });
                    respuesta.Ok = 1;
                    respuesta.Message = "Detalle del personaje por Edad";
                }
                else if (idMovie != null && name == null && age == null)
                {
                    
                    var _person = await (from per in _context.Personajes
                                        join pelis in _context.PeliculaSeries on per.PeliculaSerieId equals pelis.Id
                                        where per.PeliculaSerieId == idMovie
                                        select new 
                                        {
                                            per.Imagen,
                                            per.Nombre,
                                            per.Edad,
                                            per.Peso,
                                            per.Historia,
                                            Titulo = (from pel in _context.PeliculaSeries
                                                      where pel.PersonajeId == per.Id
                                                      select (pel.Titulo)).ToList()
                                        }).ToListAsync();
                    if(_person != null)
                    {
                        respuesta.Data.Add(_person);
                        respuesta.Ok = 1;
                        respuesta.Message = "Detalle del personaje por Pelicula o Serie";
                    }                   
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "La busqueda puedes hacerla por un solo parametro";

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
       
        // GET api/<QuestionsVrsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPersonajeById(int id)
        {        
            Respuesta<object> respuesta = new();
            try
            {
                var person = await _context.Personajes.Where(q => q.Id == id).FirstOrDefaultAsync();
                if (person != null)
                {
                    respuesta.Data.Add(new
                    {
                        person.Imagen,
                        person.Nombre,
                        person.Peso,
                        person.Edad,
                        person.Historia,
                        Titulo = (from pel in _context.PeliculaSeries
                                  where pel.PersonajeId == person.Id
                                  select (pel.Titulo)).ToList()

                    });
                    respuesta.Ok = 1;
                    respuesta.Message = "Detalles del personaje";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Personaja no encontrado";
                    return BadRequest(respuesta);
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
                return BadRequest(respuesta);
            }
            return Ok(respuesta);


        }

        //Ssaved images
        [HttpPost]
        public async Task<ActionResult> CreateCharacter([FromForm] PersonajeFile person)        
        {          
            Respuesta<object> respuesta = new();
            try
            {
                if (person!=null)
                {
                    string guidImagen = null;
                    if (person.File != null)
                    {
                        string ficherosImagenes = Path.Combine(_env.WebRootPath, "File");                       
                        guidImagen = Guid.NewGuid().ToString() + person.File.FileName;
                        string ruta = Path.Combine(ficherosImagenes, guidImagen);
                        await person.File.CopyToAsync(new FileStream(ruta, FileMode.Create));
                    }
                    Personaje personaje = new();
                    personaje.Imagen = guidImagen;
                    personaje.Nombre = person.Nombre;
                    personaje.Peso = person.Peso;
                    personaje.Edad = person.Edad;
                    personaje.Historia = person.Historia;
                    personaje.PeliculaSerieId = person.PeliculaSerieId;
                    
                    await _repository.CreateAsync(personaje);
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
        
        //Actualizar Personaje
        [HttpPut("{Id}")]
        public async Task<ActionResult<Personaje>> ActualizarPersonaje(int Id, Personaje personaje)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var _person = await _repository.SelectById<Personaje>(Id);
                if (_person != null)
                {
                    _person.Imagen = personaje.Imagen;
                    _person.Nombre = personaje.Nombre;
                    _person.Peso = personaje.Peso;
                    _person.Edad = personaje.Edad;
                    _person.Historia = personaje.Historia;
                    _person.PeliculaSerieId = personaje.PeliculaSerieId;
                    await _repository.UpdateAsync(_person);
                    respuesta.Ok = 1;
                    respuesta.Message = "Personaje actualizado satisfactoriamente";
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        //Eliminar Personaje
        [HttpDelete("{Id}")]       
        public async Task<ActionResult<Personaje>> DeletePersonaje(int Id)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var person = await _repository.SelectById<Personaje>(Id);
                if (person != null)
                {                   
                    await _repository.DeleteAsync(person);
                    respuesta.Ok = 1;
                    respuesta.Message = "Personaje eliminado satisfactoriamente";
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

    }
}
