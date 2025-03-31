using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using Blazored.LocalStorage;
using ClienteBlazor.Helpers;
using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClienteBlazor.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AutenticacionService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<RespuestaAutenticacion> Acceder(UsuarioAutenticacion usuarioAutenticacion)
        {
            var content = JsonConvert.SerializeObject(usuarioAutenticacion);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync($"{ Inicializar.UrlApi}api/usuarios/login", bodyContent);
            var contentTemp = await res.Content.ReadAsStringAsync();
            var result = (JObject)JsonConvert.DeserializeObject(contentTemp);

            if(res.IsSuccessStatusCode)
            {
                var Token = result["result"]["token"].Value<string>();
                var Usuario = result["result"]["usuario"]["nombreUsuario"].Value<string>();

                await _localStorageService.SetItemAsync(Inicializar.TokenLocal, Token);
                await _localStorageService.SetItemAsync(Inicializar.DatosUsuarioLocal, Usuario);
                ((AuthStateProvider)_authenticationStateProvider).NotificarUsuarioLogeado(Token);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);
                return new RespuestaAutenticacion { IsSuccess = true };
            }
            else
            {
                return new RespuestaAutenticacion { IsSuccess = false };
            }

        }

        public async Task<RespuestaRegistro> RegistrarUsuario(UsuarioRegistro usuarioRegistro)
        {
            var content = JsonConvert.SerializeObject(usuarioRegistro);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync($"{Inicializar.UrlApi}api/usuarios/registro", bodyContent);
            var contentTemp = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RespuestaRegistro>(contentTemp);

            if (res.IsSuccessStatusCode)
            {
                return new RespuestaRegistro { RegistroCorrecto = true };
            }
            else
            {
                return result;
            }
        }

        public async Task Salir()
        {
            await _localStorageService.RemoveItemAsync(Inicializar.TokenLocal);
            await _localStorageService.RemoveItemAsync(Inicializar.DatosUsuarioLocal);
            ((AuthStateProvider)_authenticationStateProvider).NotificarUsuarioSalir();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
