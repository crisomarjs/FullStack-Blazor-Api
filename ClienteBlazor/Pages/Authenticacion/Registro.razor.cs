using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace ClienteBlazor.Pages.Authenticacion
{
    public partial class Registro
    {
        private UsuarioRegistro UsuarioParaRegistro = new UsuarioRegistro();
        public bool EstaProcesando { get; set; } = false;
        public bool MostrarErroresRegistro { get; set; }
        public IEnumerable<string> Errores { get; set; }
        [Inject]
        public IAutenticacionService autenticacionService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }
        
        private async Task RegistrarUsuario()
        {
            MostrarErroresRegistro = false;
            EstaProcesando = true;
            var result = await autenticacionService.RegistrarUsuario(UsuarioParaRegistro);
            if(result.RegistroCorrecto)
            {
                EstaProcesando = false;
                navigationManager.NavigateTo("/acceder");
            }
            else
            {
                EstaProcesando = false;
                Errores = result.Errores;
                MostrarErroresRegistro = true;
            }
        }
    }
}
