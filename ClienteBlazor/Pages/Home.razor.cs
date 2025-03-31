using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace ClienteBlazor.Pages
{
    public partial class Home
    {
        private IEnumerable<Post> Posts { get; set; } = new List<Post>();
        [Inject]
        IPostService postService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Posts = await postService.GetPosts();
        }
    }
}
