using System.Web;
using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace ClienteBlazor.Pages.Authenticacion
{
    public partial class Acceder
    {
        private UsuarioAutenticacion usuarioAutenticacion = new UsuarioAutenticacion();
        public bool EstaProcesando { get; set; } = false;
        public bool MostrarErroresAutenticacion { get; set; }
        public string UrlRetorno { get; set; }
        public string Errores { get; set; }
        [Inject]
        public IAutenticacionService autenticacionService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }

        private async Task AccesoUsuario()
        {
            MostrarErroresAutenticacion = false;
            EstaProcesando = true;
            var result = await autenticacionService.Acceder(usuarioAutenticacion);
            if (result.IsSuccess)
            {
                EstaProcesando = false;
                var urlAbsoluta = new Uri(navigationManager.Uri);
                var parametrosQuery = HttpUtility.ParseQueryString(urlAbsoluta.Query);
                UrlRetorno = parametrosQuery["returnUrl"];

                if(string.IsNullOrEmpty(UrlRetorno))
                {
                    navigationManager.NavigateTo("/");
                }
                else
                {
                    navigationManager.NavigateTo("/" + UrlRetorno);
                }

                    
            }
            else
            {
                EstaProcesando = false;
                MostrarErroresAutenticacion = true;
                Errores = "El Usuario y/o Contraseña son Incorrectos";
                navigationManager.NavigateTo("/acceder");
            }
        }
    }
}
