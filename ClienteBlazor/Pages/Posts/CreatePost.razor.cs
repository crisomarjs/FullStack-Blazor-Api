using System.Net.Http.Headers;
using ClienteBlazor.Helpers;
using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        [Parameter]
        public string imagenPost {  get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }

        private async Task OnCreatePost()
        {
            CrearPost.RutaImagen = imagenPost;
            var createPost = await postService.CreatePost(CrearPost);
            await runtimeService.ToastrSuccess("Post creado correctamente");
            navigationManager.NavigateTo("posts");
        }

        private async Task OnUploadFile(InputFileChangeEventArgs e)
        {
            var imageFile = e.File;
            if(imageFile != null)
            {
                var resizeFile = await imageFile.RequestImageFileAsync("image/png", 1000, 700);
                using (var ms = resizeFile.OpenReadStream(resizeFile.Size)) 
                {
                    var content = new MultipartFormDataContent();
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    content.Add(new StreamContent(ms, Convert.ToInt32(resizeFile.Size)), "image", imageFile.Name);
                    imagenPost = await postService.UploadImage(content);
                    await OnChange.InvokeAsync(imagenPost);
                }
            }
        }
    }
}
