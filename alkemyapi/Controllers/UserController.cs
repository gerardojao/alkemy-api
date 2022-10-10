using alkemyapi.Data;
using alkemyapi.Models;
using alkemyapi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace alkemyapi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;
        private IMailService _mailService;
        

        public UserController(IConfiguration appsettings, AppDbContext context, IRepository repository, IMailService mailService)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
            _mailService = mailService;
          
        }

        // POST api/<UserController>
        [HttpPost("auth/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User userR)
        {
            Respuesta<object> respuesta = new();
            string token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8).ToUpper();
            try
            {
                userR.Email = UsersConfig.CheckGmail(userR.Email);
                var user = await _context.Users.Where(u => u.Email == userR.Email).FirstOrDefaultAsync();
                if (user != null)
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "El usuario ya está registrado";
                }
                else
                {
                    userR.VerificationCode = token;
                    var userId = await _repository.CreateAsync(userR);
                    await _mailService.SendEmailAsync(userR.Email,"Token to access API-Alkemy", $"<h2>Thanks {userR.Username} por registrarte en nuetra App</h2><br><h4>Acá te enviamos tu token de segurdad para ingresar: </h4>" + $"<p>{userR.VerificationCode }</p>");
                   
                    respuesta.Ok = 1;
                    respuesta.Data.Add(userId);
                    respuesta.Message = "Usuario Registrado";
                    
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

       
        [HttpPost("auth/login")]        
        public async Task<ActionResult<User>> Login(string email, string token)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                        SqlConnection con = new(_appsettings.GetConnectionString("DevConnection").ToString());
                        SqlDataAdapter da = new("SELECT * FROM Users WHERE Email ='"+email +"' AND VerificationCode = '"+token+"' ", con);
                        DataTable dt = new();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            
                            respuesta.Ok = 1;
                            respuesta.Message = "Usuario logueado";
                            return Ok(respuesta);
                        }
                    return Ok("Error en email o token");
                }
                else
                {
                        respuesta.Ok = 0;
                        respuesta.Message = "Error en email o token ";
                        return Ok(respuesta);  
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
     

    public class UserLoginModel
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Code { get; set; }
        
    }
}
