using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiBlog.Data;
using ApiBlog.Models;
using ApiBlog.Models.DTO;
using ApiBlog.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using XSystem.Security.Cryptography;

namespace ApiBlog.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        private string securityKey;

        public UsuarioRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            securityKey = configuration.GetValue<string>("ApiSettings:Secreta");
        }

        public Usuario GetUsuario(int id)
        {
            return _context.Usuario.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _context.Usuario.OrderBy(u => u.Id).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var usuarioExist = _context.Usuario.FirstOrDefault(u => u.NombreUsuario == usuario);
            if(usuarioExist == null)
            {
                return true;
            }
            return false;
        }

        public async Task<UsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            var passwordEncrypted = obtenermd5(usuarioLoginDTO.Password);
            var usuario = _context.Usuario.FirstOrDefault(
                u => u.NombreUsuario == usuarioLoginDTO.NombreUsuario.ToLower() 
                && u.Password == passwordEncrypted
            );

            // validar si el usuario existe
            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDTO()
                {
                    Token = "",
                    Usuario = null
                };
            }

            // si el usuario existe, generar token
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDTO usuarioLoginRespuestaDTO = new UsuarioLoginRespuestaDTO()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };
            
            return usuarioLoginRespuestaDTO;
        }

        public async Task<Usuario> Registro(UsuarioRegistroDTO usuarioRegistroDTO)
        {
            var passwordEncrypted = obtenermd5(usuarioRegistroDTO.Password);
            Usuario usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDTO.NombreUsuario,
                Nombre = usuarioRegistroDTO.Nombre,
                Email = usuarioRegistroDTO.Email,
                Password = passwordEncrypted
            };

            _context.Usuario.Add(usuario); 
            usuario.Password = passwordEncrypted;
            await _context.SaveChangesAsync();
            return usuario; 
        }

        // encriptar contraseña
        private static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for(int i = 0; i < data.Length; i++) 
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }
    }
}
