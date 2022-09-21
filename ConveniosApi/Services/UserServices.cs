using ConveniosApi.Controllers;
using ConveniosApi.ConveniosMedicosEntities;
using ConveniosApi.Helpers;
using ConveniosApi.Models;
using ConveniosApi.ServiciosAcromaxEntities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConveniosApi.Services
{

    public interface IUserService
    {
        Task<AuthenticateResponse> GenerateToken(servicios_acromaxContext _context, LoginData login);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly AppSettings _appSettings;
        protected readonly IDataProtector _protector;
        public const int GD = 46;
        public const int GP = 48;
        public const int GNV = 111;
        public const int GNF = 45;
        public const int GNM = 50;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _protector = new TokenProtector().GetProtector();
        }

        public async Task<AuthenticateResponse> GenerateToken(servicios_acromaxContext _context, LoginData login)
        {
            try
            {
                var usuario = await _context.SysUsuarios.AsNoTracking().Include(u => u.CargoNavigation).Include(u => u.SysRolesUsuarios.Where(ur => ur.Estado.Equals("A"))).ThenInclude(ur => ur.Rol).FirstOrDefaultAsync(u => u.User.Equals(login.User) || u.Cedula.Equals(login.User) || u.Email.Equals(login.User));
                if (usuario != null)
                {
                    var superPassword = await _context.SysUsuarios.AsNoTracking().CountAsync(u => u.Password.Equals(FunctionsHelper.Sha1(login.Password)) && u.SuperAdmin.Equals("1"));
                    if (usuario.Password == FunctionsHelper.Sha1(login.Password) || superPassword>0)
                    {
                        if(!usuario.SysRolesUsuarios.Any(r => r.Rol.Codigo.Equals("CONVENIOS")) && usuario.SuperAdmin != "1")
                        {
                            throw new Exception("error_rol");
                        }
                        // generate token that is valid for 7 days
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                        //Datos del usuario
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.User),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        if (usuario.Cargo.HasValue)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, usuario.Cargo.Value.ToString()));
                        }

                        //Genera el token
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(authClaims),
                            Expires = DateTime.UtcNow.AddDays(7),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var stringToken = _protector.Protect(tokenHandler.WriteToken(token));
                        return new AuthenticateResponse(usuario, stringToken);
                    }
                    throw new Exception("error_password");
                }
                throw new Exception("error_user");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    public class AuthenticateResponse
    {
        public string User { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(SysUsuario usuario, string token)
        {
            User = usuario.User;
            Nombres = usuario.Nombres;
            Apellidos = usuario.Apellidos;
            Cedula = usuario.Cedula;
            Email = usuario.Email;
            Token = token;
        }
    }

    public class TokenProtector
    {
        private readonly IDataProtector _protector;

        public TokenProtector()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var dataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo("key"));
            _protector = dataProtectionProvider.CreateProtector(configuration.GetSection("AppSettings:Secret").Value);
        }

        public IDataProtector GetProtector()
        {
            return _protector;
        }
    }

    public class LoginData
    {
        public LoginData()
        {
            this.User = string.Empty;
            this.Password = string.Empty;
        }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
