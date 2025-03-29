using ClienteBlazor.Helpers;
using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClienteBlazor.Pages.Posts
{
    public partial class CreatePost
    {
        [Inject]
        IPostService postService { get; set; }
        [Inject]
        IJSRuntime runtimeService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }
        private Post CrearPost { get; set; } = new Post();

        private async Task OnCreatePost()
        {
            var createPost = await postService.CreatePost(CrearPost);
            await runtimeService.ToastrSuccess("Post creado correctamente");
            navigationManager.NavigateTo("posts");
        }
    }
}
