using System.Net.Http.Headers;
using System.Text;
using Blazored.LocalStorage;
using ClienteBlogWASM.Helpers;
using ClienteBlogWASM.Modelos;
using ClienteBlogWASM.Servicios.IServicio;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClienteBlogWASM.Servicios
{
    public class ServicioAutenticacion : IServicioAutenticacion
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _stateProveedorAutenticacion;

        public ServicioAutenticacion(HttpClient cliente, 
            ILocalStorageService localStorage,
            AuthenticationStateProvider stateProveedorAutenticacion)
        {
            _httpClient = cliente;
            _localStorageService = localStorage;
            _stateProveedorAutenticacion = stateProveedorAutenticacion;
        }
        public async Task<RespuestaAutenticacion> Acceder(UsuarioAutenticacion usuarioDesdeAutenticacion)
        {
            var content = JsonConvert.SerializeObject(usuarioDesdeAutenticacion);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Initialize.UrlBaseApi}api/usuarios/login", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var resultado = (JObject)JsonConvert.DeserializeObject(contentTemp);
            if (response.IsSuccessStatusCode)
            {
                var Token = resultado["result"]["token"].Value<string>();
                var Usuario = resultado["result"]["usuario"]["nombreUsuario"].Value<string>();
                await _localStorageService.SetItemAsync(Initialize.Token_Local, Token);
                await _localStorageService.SetItemAsync(Initialize.Datos_Usuario_Local, Usuario);
                ((AuthStateProvider)_stateProveedorAutenticacion).NotificarUsuarioLogueado(Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);
                return new RespuestaAutenticacion { IsSuccess = true };
            }
            else
            {
                return new RespuestaAutenticacion { IsSuccess = false };
            }
        }

        public async Task<RespuestaRegistro> RegistrarUsuario(UsuarioRegistro usuarioParaRegistro)
        {
            var content = JsonConvert.SerializeObject(usuarioParaRegistro);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Initialize.UrlBaseApi}api/usuarios/registro", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<RespuestaRegistro>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                return new RespuestaRegistro { RegistroCorrecto = true };
            }
            else
                {
                    return resultado;
                }
        }

        public async Task Salir()
        {
            await _localStorageService.RemoveItemAsync(Initialize.Token_Local);
            await _localStorageService.RemoveItemAsync(Initialize.Datos_Usuario_Local);
            ((AuthStateProvider)_stateProveedorAutenticacion).NotificarUsuarioSalir();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}