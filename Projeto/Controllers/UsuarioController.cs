using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Projeto.Models;
using Projeto.Utils;

namespace Projeto.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsuarioController : ControllerBase
    {
        private UserManager<Usuario> _userManager;
        private SignInManager<Usuario> _signInManager;
        private readonly AppSettings _appSettings;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IOptions<AppSettings> appSettings) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <remarks>
        /// Amostra de objeto:
        ///
        ///     POST /api/usuario/register
        ///     {
        ///        "userName": "Usuario",
        ///        "nome": "Nome do usuario",
        ///        "email": "email@dousuario.com",
        ///        "password": "senhadousuario"
        ///     }
        ///
        /// </remarks>
        /// <param name="usuarioDTO"></param>
        /// <returns>Nova sala criada</returns>
        /// <response code="200">Retorna objeto {"succeeded": true, "errors": [] } </response>
        /// <response code="400">Houve falha durante o cadastro</response> 
        [HttpPost]
        [Route("Register")]
        public async Task<Object> PostUsuario(UsuarioDTO usuarioDTO)
        {
            var Usuario = new Usuario() {
                UserName = usuarioDTO.UserName,
                Nome = usuarioDTO.Nome,
                Email = usuarioDTO.Email
            };

            if (await _userManager.FindByNameAsync(Usuario.UserName) != null)
            {
                return BadRequest(new {message = "Ja existe um usuário " +Usuario.UserName + " cadastrado."});
            }

            try
            {
                var result = await _userManager.CreateAsync(Usuario, usuarioDTO.Password);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // POST : /api/usuario/login

        /// <summary>
        /// Efetua login do usuario
        /// </summary>
        /// <remarks>
        /// Amostra de objeto:
        ///
        ///     POST /api/usuario/login
        ///     {
        ///        "userName": "Usuario",
        ///        "password": "senhadousuario"
        ///     }
        ///
        /// </remarks>
        /// <param name="usuarioDTO"></param>
        /// <returns>Nova sala criada</returns>
        /// <response code="200">Retorna objeto {"token": "JsonWebToken" } </response>
        /// <response code="400">Houve falha durante o cadastro</response>         
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UsuarioDTO usuarioDTO)
        {
            var user = await _userManager.FindByNameAsync(usuarioDTO.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, usuarioDTO.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)),SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Usuario ou senha inválido."});
        }

        /// <summary>
        /// Retorna as informações do usuário logado
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<Object> GetUsuario() {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                 user.Nome,
                 user.Email,
                 user.UserName
            };
        }
    }
}