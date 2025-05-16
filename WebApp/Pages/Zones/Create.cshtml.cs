using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Data;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace WebApp.Pages.Zones
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;

        public CreateModel(WebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Zone Zone { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Zones.Add(Zone);
            await _context.SaveChangesAsync();

            // --- Логируем событие Create ---
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Events.Add(new Event
            {
                ZoneId = Zone.ZoneId,
                EventType = "Created",
                Timestamp = DateTimeOffset.Now,
                UserId = userId
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
