using ClienteBlogWASM.Modelos;
using ClienteBlogWASM.Modelos.ViewModels;

namespace ClienteBlogWASM.Servicios.IServicio
{
    public interface IPostsServicio
    {
        public Task<IEnumerable<Post>> GetPosts();
        public Task<Post> GetPost(int postId);
        Task<Post> CreatePost(PostCrearVM postViewModel);
        Task<bool> UpdatePost(int postId, PostActualizarVM postViewModel);
        public Task<bool> DeletePost(int postId);
        public Task<string> UploadImagen(MultipartFormDataContent content);
    }
}
