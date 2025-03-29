using ClienteBlazor.Models;

namespace ClienteBlazor.Services.IServices
{
    public interface IPostService
    {
        public Task<IEnumerable<Post>> GetPosts();
        public Task<Post> GetPost(int id);
        public Task<Post> CreatePost(Post post);
        public Task<Post> UpdatePost(int id, Post post);
        public Task<bool> DeletePost(int id);
        public Task<string> UploadImage(MultipartFormDataContent content);
    }
}
