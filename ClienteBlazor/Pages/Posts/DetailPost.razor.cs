using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace ClienteBlazor.Pages.Posts
{
    public partial class DetailPost
    {
        private Post post { get; set; } = new Post();
        [Inject]
        IPostService postService { get; set; }
        [Parameter]
        public int? Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            post = await postService.GetPost(Id.Value);
        }
    }
}
