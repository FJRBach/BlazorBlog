using ClienteBlogWASM.Modelos;
using ClienteBlogWASM.Modelos.ViewModels;

namespace ClienteBlogWASM.Servicios.IServicio
{
    public interface IPostsServicio
    {
        public Task<IEnumerable<Post>> GetPosts();
        public Task<Post> GetPost(int postId);
        Task<Post> CreatePost(PostCrearVM postViewModel);
        public Task<Post> UpdatePost(int postId, Post post);
        public Task<bool> DeletePost(int postId);
        public Task<string> UploadImagen(MultipartFormDataContent content);
    }
}
