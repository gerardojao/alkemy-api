﻿using alkemyapi.Data;
using alkemyapi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace alkemyapi.Controllers
{
    [Route("movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;
        private readonly IWebHostEnvironment _env;

        public MovieController(IConfiguration appsettings, AppDbContext context, IRepository repository, IWebHostEnvironment env)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
            _env = env;
        }

        //GET: api/<CharactersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeliculaSerie>>> GetAllMovies()
        {
            Respuesta<object> respuesta = new();
            try
            {
                //var user = await _context.Users.Where(u => u.VerificationCode == token).FirstOrDefaultAsync();
               
                    var movies = await _repository.SelectAll<PeliculaSerie>();

                    if (movies != null)
                    {
                        foreach (var item in movies)
                        {
                            respuesta.Data.Add(new
                            {
                                item.Imagen,
                                item.Titulo,
                                item.FechaCreacion                               
                            });
                        }
                        respuesta.Ok = 1;
                        respuesta.Message = "Peliculas o Series registradas";
                    }
                    else
                    {
                        respuesta.Ok = 0;
                        respuesta.Message = "No hay Peliculas o Series registrados";
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMovieSerieById(int id)
        {
            Respuesta<object> respuesta = new();
           
            try
            {
                var pelis = await _context.PeliculaSeries.Where(m => m.Id == id).FirstOrDefaultAsync();
                if (pelis != null)
                {
                    respuesta.Data.Add(new
                    {
                        pelis.Imagen,
                        pelis.Titulo,
                        pelis.FechaCreacion,
                        pelis.CalificacionId,
                        Personaje = (from personaje in _context.Personajes
                                     where personaje.Id == pelis.PersonajeId
                                     select (personaje.Nombre)).ToList()
                    });

                    respuesta.Ok = 1;
                    respuesta.Message = "Detalle del movie";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "movie no encontrado";
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

        //Ssaved images
        [HttpPost("create")]
        public async Task<ActionResult> CreateMovie([FromForm] MovieFile movie)
        {
            Respuesta<object> respuesta = new();
            try
            {
                if (movie != null)
                {
                    string guidImagen = null;
                    if (movie.File != null)
                    {
                        string ficherosImagenes = Path.Combine(_env.WebRootPath, "File");
                        guidImagen = Guid.NewGuid().ToString() + movie.File.FileName;
                        string ruta = Path.Combine(ficherosImagenes, guidImagen);
                        await movie.File.CopyToAsync(new FileStream(ruta, FileMode.Create));
                    }
                    PeliculaSerie movies = new();
                    movies.Imagen = guidImagen;
                    movies.Titulo = movie.Titulo;
                    movies.FechaCreacion = DateTime.Now;
                    movies.CalificacionId = movie.CalificacionId;                  
                    movies.PersonajeId = movie.PersonajeId;

                    await _repository.CreateAsync(movies);
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

        //Actualizar movie
        [HttpPut("UpdateMovie{Id}")]
        public async Task<ActionResult<PeliculaSerie>> ActualizarPeliculas( int Id, PeliculaSerie movies)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var _movie = await _repository.SelectById<PeliculaSerie>(Id);
                if (_movie!= null)
                {
                    _movie.Titulo = movies.Titulo;                  
                    _movie.CalificacionId = movies.CalificacionId;
                    _movie.PersonajeId = movies.PersonajeId;
                    await _repository.UpdateAsync(_movie);
                    respuesta.Ok = 1;
                    respuesta.Message = "Pelicula o Serie actualizada satisfactoriamente";
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        //Eliminar movie
        [HttpDelete("DeleteMovie{Id}")]
        public async Task<ActionResult<PeliculaSerie>> Deletemovie(int Id)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var movie = await _repository.SelectById<PeliculaSerie>(Id);
                if (movie != null)
                {
                    await _repository.DeleteAsync(movie);
                    respuesta.Ok = 1;
                    respuesta.Message = "movie eliminado satisfactoriamente";
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        [HttpGet("test")]
        public async Task<ActionResult> GetMoviesByQueries([FromQuery] string name, int? genre, string order)
        {
            Respuesta<object> respuesta = new();
            try
            {
                if (order=="ASC")
                {

                    var pelis = await (from movies in _context.PeliculaSeries
                                       join per in _context.Personajes on movies.Id equals per.PeliculaSerieId
                                       join gen in _context.Generos on movies.Id equals gen.PeliculaSerieId
                                       where movies.Titulo == name
                                       select new
                                       {
                                           movies.Imagen,
                                           movies.Titulo,
                                           movies.FechaCreacion,
                                           movies.CalificacionId,
                                           gen.Nombre,
                                           Personaje = (from personaje in _context.Personajes
                                                        where personaje.Id == movies.PersonajeId
                                                        select (personaje.Nombre)).ToList()
                                       }).ToListAsync();
                    if (pelis != null)
                    {
                        respuesta.Data.Add(pelis);
                        respuesta.Ok = 1;
                        respuesta.Message = "Detalle del movie";
                    }
                    else
                    {
                        respuesta.Ok = 0;
                        respuesta.Message = "Data not found";
                        return Ok(respuesta);
                    }

                }
                else
                {
                    var pelis = await (from movies in _context.PeliculaSeries
                                       join per in _context.Personajes on movies.Id equals per.PeliculaSerieId
                                       join gen in _context.Generos on movies.Id equals gen.PeliculaSerieId
                                       where movies.Titulo == name
                                       select new
                                       {
                                           movies.Imagen,
                                           movies.Titulo,
                                           movies.FechaCreacion,
                                           movies.CalificacionId,
                                           gen.Nombre,
                                           Personaje = (from personaje in _context.Personajes
                                                        where personaje.Id == movies.PersonajeId
                                                        select (personaje.Nombre)).ToList()
                                       })
                                       .OrderByDescending(x => x.FechaCreacion)
                                       .ToListAsync();
                    if (pelis != null)
                    {
                        respuesta.Data.Add(pelis);
                        respuesta.Ok = 1;
                        respuesta.Message = "Detalle del movie";
                    }
                    else
                    {
                        respuesta.Ok = 0;
                        respuesta.Message = "Data not found";
                        return Ok(respuesta);
                    }
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
