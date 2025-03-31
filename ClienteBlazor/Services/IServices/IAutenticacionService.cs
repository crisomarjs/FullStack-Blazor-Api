using ClienteBlazor.Models;

namespace ClienteBlazor.Services.IServices
{
    public interface IAutenticacionService
    {
        Task<RespuestaRegistro> RegistrarUsuario(UsuarioRegistro usuarioRegistro);
        Task<RespuestaAutenticacion> Acceder(UsuarioAutenticacion usuarioAutenticacion);
        Task Salir();
    }
}
