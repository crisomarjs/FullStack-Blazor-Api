
using System.Text;
using ClienteBlazor.Helpers;
using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Newtonsoft.Json;

namespace ClienteBlazor.Services
{
    public class PostService : IPostService
    {
        private readonly HttpClient _httpClient;

        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Post> CreatePost(Post post)
        {
            var content = JsonConvert.SerializeObject(post);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync($"{Inicializar.UrlApi}api/posts", bodyContent);

            if (res.IsSuccessStatusCode)
            {
                var contentTemp = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Post>(contentTemp);
                return result;
            }
            else
            {
                var contentTemp = await res.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<bool> DeletePost(int id)
        {
            var res = await _httpClient.DeleteAsync($"{Inicializar.UrlApi}api/posts/{id}");
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<Post> GetPost(int id)
        {
            var res = await _httpClient.GetAsync($"{Inicializar.UrlApi}api/posts/{id}");
            if(res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var post = JsonConvert.DeserializeObject<Post>(content);
                return post;
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var res = await _httpClient.GetAsync($"{Inicializar.UrlApi}api/posts");
            var content = await res.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<IEnumerable<Post>>(content);

            return posts;
        }

        public async Task<Post> UpdatePost(int id, Post post)
        {
            var content = JsonConvert.SerializeObject(post);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var res = await _httpClient.PatchAsync($"{Inicializar.UrlApi}api/posts/{id}", bodyContent);

            if (res.IsSuccessStatusCode)
            {
                var contentTemp = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Post>(contentTemp);
                return result;
            }
            else
            {
                var contentTemp = await res.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ModeloError>(contentTemp);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<string> UploadImage(MultipartFormDataContent content)
        {
            var postResult = await _httpClient.PostAsync($"{Inicializar.UrlApi}api/upload", content);
            var postContent = await postResult.Content.ReadAsStringAsync();
            if(!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException(postContent);
            }
            else
            {
                var imagenPost = Path.Combine($"{Inicializar.UrlApi}", postContent);
                return imagenPost;
            }
        }
    }
}
