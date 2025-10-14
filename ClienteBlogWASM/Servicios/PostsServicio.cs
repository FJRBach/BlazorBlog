using System.Text;
using ClienteBlogWASM.Helpers;
using ClienteBlogWASM.Modelos;
using ClienteBlogWASM.Modelos.ViewModels;
using ClienteBlogWASM.Servicios.IServicio;
using Newtonsoft.Json;

namespace ClienteBlogWASM.Servicios
{
    public class PostsServicio : IPostsServicio
    {
        private readonly HttpClient _httpClient;

        public PostsServicio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            // --- ¡AQUÍ ESTÁ LA MAGIA! ---
            // Se elimina la ruta absoluta para usar la BaseAddress
            var response = await _httpClient.GetAsync("api/posts");
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<IEnumerable<Post>>(content);
            return posts;
        }

        public async Task<Post> GetPost(int postId)
        {
            // --- ¡AQUÍ ESTÁ LA MAGIA! ---
            // Se elimina la ruta absoluta para usar la BaseAddress
            var response = await _httpClient.GetAsync($"api/posts/{postId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var post = JsonConvert.DeserializeObject<Post>(content);
                return post;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<Post> CreatePost(PostCrearVM postCreateVM)
        {
            var content = JsonConvert.SerializeObject(postCreateVM);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/posts", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Post>(contentTemp);
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(contentTemp);
                throw new Exception(errorModel?.ErrorMessage ?? "Ocurrió un error desconocido");
            }
        }

        public async Task<bool> UpdatePost(int postId, PostActualizarVM postViewModel)
        {
            var content = JsonConvert.SerializeObject(postViewModel);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"api/posts/{postId}", bodyContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(contentTemp);
                throw new Exception(errorModel?.ErrorMessage ?? "Ocurrió un error al actualizar el post");
            }
        }

        public async Task<bool> DeletePost(int postId)
        {
            var response = await _httpClient.DeleteAsync($"api/posts/{postId}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<string> UploadImagen(MultipartFormDataContent content)
        {
            var response = await _httpClient.PostAsync("api/posts/upload-imagen", content);
            if (response.IsSuccessStatusCode)
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<dynamic>(contentTemp);
                return resultado.ruta;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(contentTemp);
                throw new Exception(errorModel?.ErrorMessage ?? "Error al subir la imagen");
            }
        }
    }
}