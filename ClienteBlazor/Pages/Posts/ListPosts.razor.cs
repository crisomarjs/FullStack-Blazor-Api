using ClienteBlazor.Models;
using ClienteBlazor.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace ClienteBlazor.Pages.Posts
{
    public partial class ListPosts
    {
        [Inject]
        IPostService postService { get; set; }
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

        protected override async Task OnInitializedAsync()
        {
            Posts = await postService.GetPosts();
        }

        public void OnDelete(int id)
        {

        }

    }
}
