using System.ComponentModel.DataAnnotations;

namespace ClienteBlazor.Models
{
    public class UsuarioAutenticacion
    {
        [Required(ErrorMessage ="El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El password es obligatorio")]
        public string Password { get; set; }
    }
}
