using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;


namespace WebApp.Pages.Monitoring
{
    [Authorize(Roles = "Admin")]
    public class VideoModel : PageModel
    {
        public void OnGet() { }
    }
}
