using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using ClienteBlazor.Models;
using ClienteBlazor.Helpers;

namespace ClienteBlazor.Pages.Posts
{
    public partial class EditPost
    {
        [Inject]
        IPostService postService { get; set; }
        [Inject]
        IJSRuntime runtimeService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }
        private Post EditarPost { get; set; } = new Post();
        [Parameter]
        public int? Id { get; set; }
        [Parameter]
        public string imagenPost { get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }

        protected override async Task OnInitializedAsync()
        {
            EditarPost = await postService.GetPost(Id.Value);
        }

        private async Task OnEditPost()
        {
            EditarPost.RutaImagen = imagenPost;
            var editPost = await postService.UpdatePost(Id.Value, EditarPost);
            await runtimeService.ToastrSuccess("Post actualizado correctamente");
            navigationManager.NavigateTo("posts");
        }

        private async Task OnUploadFile(InputFileChangeEventArgs e)
        {
            var imageFile = e.File;
            if (imageFile != null)
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
