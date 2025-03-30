using ClienteBlazor.Helpers;
using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClienteBlazor.Pages.Posts
{
    public partial class ListPosts
    {
        [Inject]
        IPostService postService { get; set; }
        [Inject]
        IJSRuntime runtimeService { get; set; }
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        private bool inProcess {  get; set; } = false;
        private int? DeleteIdPost { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            Posts = await postService.GetPosts();
        }

        public async Task OnDelete(int id)
        {
            DeleteIdPost = id;
            await runtimeService.InvokeVoidAsync("MostrarModalConfirmacionBorrado");
        }

        public async Task Click_Delete(bool confirm)
        {
            inProcess = true;
            if(confirm && DeleteIdPost != null)
            {
                //Post post = await postService.GetPost(DeleteIdPost.Value);
                await postService.DeletePost(DeleteIdPost.Value);
                await runtimeService.ToastrSuccess("Post borrado correctamente");
                Posts = await postService.GetPosts();
            }


            await runtimeService.InvokeVoidAsync("OcultarModalConfirmacionBorrado");
            inProcess = false;
        }

    }
}
