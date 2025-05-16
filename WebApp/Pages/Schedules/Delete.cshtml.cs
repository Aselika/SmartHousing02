using System;
using System.Security.Claims;           // <-- для User.FindFirstValue
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;                     // <-- для Schedule и Event

namespace WebApp.Pages.Schedules
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Schedule Schedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var schedule = await _context.Schedules
                                         .FirstOrDefaultAsync(m => m.ScheduleId == id);

            if (schedule == null)
                return NotFound();

            Schedule = schedule;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // 1) Находим правило
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule != null)
            {
                // 2) Удаляем
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();

                // 3) Логируем событие удаления
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Events.Add(new Event
                {
                    ZoneId = schedule.ZoneId,
                    EventType = "ScheduleDeleted",
                    Timestamp = DateTimeOffset.Now,
                    UserId = userId
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
