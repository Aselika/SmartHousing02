using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Pages.Schedules
{
    public class EditModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;

        public EditModel(WebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Schedule Schedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule =  await _context.Schedules.FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }
            Schedule = schedule;
           ViewData["ZoneId"] = new SelectList(_context.Zones, "ZoneId", "CommandOff");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Schedule).State = EntityState.Modified;

            try
            {
                // 1) Сохраняем изменения
                await _context.SaveChangesAsync();

                // 2) Логируем событие редактирования расписания
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Events.Add(new Event
                {
                    ZoneId = Schedule.ZoneId,
                    EventType = "ScheduleEdited",
                    Timestamp = DateTimeOffset.Now,
                    UserId = userId
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(Schedule.ScheduleId))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("./Index");
        }


        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.ScheduleId == id);
        }
    }
}
