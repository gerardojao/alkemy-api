using alkemyapi.Data;
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
        public async Task<ActionResult<IEnumerable<PeliculaSerie>>> GetAllMovies(string token)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.Users.Where(u => u.VerificationCode == token).FirstOrDefaultAsync();
                if (user != null)
                {
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
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Requiere token de seguridad";
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
        public async Task<ActionResult> GetMovieSerieById(string token, int id)
        {
            Respuesta<object> respuesta = new();           
            try
            {
                var user = await _context.Users.Where(u => u.VerificationCode == token).FirstOrDefaultAsync();
                if (user != null)
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
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Requiere token de seguridad";
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
        [HttpPost]
        public async Task<ActionResult> CreateMovie(string token, [FromForm] PeliculaSerie movie)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.Users.Where(u => u.VerificationCode == token).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (movie != null)
                    {
                        string ficherosImagenes = Path.Combine(_env.WebRootPath, "File");
                        string guidImagen = Guid.NewGuid().ToString() + movie.File.FileName;
                        movie.Imagen = guidImagen;
                        string ruta = Path.Combine(ficherosImagenes, guidImagen);
                        using (var fileStream = new FileStream(ruta, FileMode.Create))
                        {
                            await movie.File.CopyToAsync(fileStream);
                        }
                        await _repository.CreateAsync(movie);
                        respuesta.Ok = 1;
                        respuesta.Message = "Movie/Serie registered successfully";
                    }
                    else
                    {
                        respuesta.Ok = 0;
                        respuesta.Message = "Movie/Serie not created";
                    }
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Requiere token de seguridad";
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
        //Actualizar Personaje
        [HttpPut("{Id}")]
        public async Task<ActionResult<Personaje>> ActualizarPersonaje(string token, int Id, [FromForm] PeliculaSerie movie)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.Users.Where(u => u.VerificationCode == token).FirstOrDefaultAsync();
                if (user != null)
                {
                    var p = await _context.PeliculaSeries.Where(q => q.Id == Id).FirstOrDefaultAsync();
                    if (p != null)
                    {
                        if (movie != null)
                        {
                            string ficherosImagenes = Path.Combine(_env.WebRootPath, "File");
                            string guidImagen = Guid.NewGuid().ToString() + movie.File.FileName;
                            p.Imagen = guidImagen;
                            string ruta = Path.Combine(ficherosImagenes, guidImagen);
                            using (var fileStream = new FileStream(ruta, FileMode.Create))
                            {
                                await movie.File.CopyToAsync(fileStream);
                            }
                            if (movie.Titulo != p.Titulo) { p.Titulo = movie.Titulo; }
                            if (movie.CalificacionId != p.CalificacionId) { p.CalificacionId = movie.CalificacionId; }
                            if (movie.PersonajeId != p.PersonajeId) { p.PersonajeId = movie.PersonajeId; }
                            if (movie.FechaCreacion != p.FechaCreacion) { p.FechaCreacion = movie.FechaCreacion; }

                            await _repository.UpdateAsync(p);
                            respuesta.Ok = 1;
                            respuesta.Message = "Movie/Serie updated successfully";
                        }

                        else
                        {
                            respuesta.Ok = 0;
                            respuesta.Message = "No se pudo actualizar la pelicula o serie";
                        }
                    }
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Requiere token de seguridad";
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
        [HttpDelete("{Id}")]
        public async Task<ActionResult<PeliculaSerie>> Deletemovie(string token, int Id)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.Users.Where(u => u.VerificationCode == token).FirstOrDefaultAsync();
                if (user != null)
                {
                    var movie = await _repository.SelectById<PeliculaSerie>(Id);
                    if (movie != null)
                    {
                        var ficherosImagenes = Path.Combine(_env.WebRootPath, "File", movie.Imagen);
                        if (System.IO.File.Exists(ficherosImagenes))
                            System.IO.File.Delete(ficherosImagenes);

                        await _repository.DeleteAsync(movie);
                        respuesta.Ok = 1;
                        respuesta.Message = "Pelicula o Serie eliminada satisfactoriamente";
                    }
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Requiere token de seguridad";
                }               
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        [HttpGet(" ")]
        public async Task<ActionResult> GetMoviesByQueries(string token, [FromQuery] string name, int? genre, string order)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.Users.Where(u => u.VerificationCode == token).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (order == "ASC")
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
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Requiere token de seguridad";
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
