using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace ClienteBlazor.Pages.Authenticacion
{
    public partial class Salir
    {
        [Inject]
        public IAutenticacionService autenticacionService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await autenticacionService.Salir();
            navigationManager.NavigateTo("/");
        }
    }
}
