using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Models.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }
}
