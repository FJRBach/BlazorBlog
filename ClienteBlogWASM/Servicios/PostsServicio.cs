using System.Text;
using ClienteBlogWASM.Helpers;
using ClienteBlogWASM.Modelos;
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
        public async Task<bool> DeletePost(int postId)
        {
            var response = await _httpClient.GetAsync($"{Initialize.UrlBaseApi}api/posts/{postId}");
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

        public async Task<Post> GetPost(int postId)
        {
            var response = await _httpClient.GetAsync($"{Initialize.UrlBaseApi}api/posts/{postId}");
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

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var response = await _httpClient.GetAsync($"{Initialize.UrlBaseApi}api/posts");
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<IEnumerable<Post>>(content);
            return posts;
        }

        public async Task<Post> CreatePost(Post post)
        {
            var content = JsonConvert.SerializeObject(post);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Initialize.UrlBaseApi}api/posts", bodyContent);
            if (response.IsSuccessStatusCode)
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Post>(contentTemp);
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(contentTemp);
                throw new Exception(errorModel.ErrorMessage);
            }
        }


        public async Task<Post> UpdatePost(int postId, Post post)
        {
            var content = JsonConvert.SerializeObject(post);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Initialize.UrlBaseApi}api/posts/{postId}", bodyContent);
            if (response.IsSuccessStatusCode)
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Post>(contentTemp);
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(contentTemp);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public Task<string> UploadImagen(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }
    }
}
