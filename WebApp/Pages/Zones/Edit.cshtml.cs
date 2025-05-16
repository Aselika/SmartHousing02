using System;
using System.Linq;
using System.Security.Claims;       
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Pages.Zones
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Zone Zone { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var zone = await _context.Zones.FirstOrDefaultAsync(m => m.ZoneId == id);
            if (zone == null) return NotFound();

            Zone = zone;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Zone).State = EntityState.Modified;

            try
            {
                // Сохраняем изменения зоны
                await _context.SaveChangesAsync();

                // --- ЛОГИРУЕМ событие редактирования ---
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Events.Add(new Event
                {
                    ZoneId = Zone.ZoneId,
                    EventType = "Edited",
                    Timestamp = DateTimeOffset.Now,
                    UserId = userId
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZoneExists(Zone.ZoneId))
                    return NotFound();
                else
                    throw;
            }

            // После логирования — возвращаемся на список
            return RedirectToPage("./Index");
        }

        private bool ZoneExists(int id)
        {
            return _context.Zones.Any(e => e.ZoneId == id);
        }
    }
}
