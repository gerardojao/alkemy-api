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
    [Route("auth")]
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
        [HttpPost("Register")]
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
                    //await _mailService.SendEmailAsync(userR.Email, "Token to access API-Alkemy", "Your Security Token: " + userR.VerificationCode);                   
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

       
        [HttpPost("login")]        
        public async Task<ActionResult<User>> Login(string email, string username)
        {
            Respuesta<object> respuesta = new();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                        SqlConnection con = new(_appsettings.GetConnectionString("DevConnection").ToString());
                        SqlDataAdapter da = new("SELECT * FROM Users WHERE Email ='"+ email +"' AND Username = '"+ username +"' ", con);
                        DataTable dt = new();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            await _mailService.SendEmailAsync(user.Email, "Token to access API-Alkemy", "Your Security Token: " + user.VerificationCode);
                            respuesta.Ok = 1;
                            respuesta.Message = "Usuario logueado";
                            return Ok(respuesta);
                        }
                    return Ok("Error en email o username");
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
     
}
