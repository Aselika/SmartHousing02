using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Pages.Schedules
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // сам объект расписания
        [BindProperty]
        public Schedule Schedule { get; set; } = default!;

        // Выпадающий список зон
        public SelectList ZoneOptions { get; set; } = default!;

        // Выбранные из формы дни недели (битовые значения)
        [BindProperty]
        public int[] SelectedDays { get; set; } = Array.Empty<int>();

        // Опции для чекбоксов дней
        public List<SelectListItem> DaysOfWeekOptions { get; } = new()
        {
            new SelectListItem("Вс",       value: (1<<0).ToString()),
            new SelectListItem("Пн",       value: (1<<1).ToString()),
            new SelectListItem("Вт",       value: (1<<2).ToString()),
            new SelectListItem("Ср",       value: (1<<3).ToString()),
            new SelectListItem("Чт",       value: (1<<4).ToString()),
            new SelectListItem("Пт",       value: (1<<5).ToString()),
            new SelectListItem("Сб",       value: (1<<6).ToString()),
        };

        public IActionResult OnGet()
        {
            // наполняем выпадающее меню из таблицы Zones
            ZoneOptions = new SelectList(_context.Zones.OrderBy(z => z.Name), "ZoneId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            // 0) Собираем выбранные дни в битовую маску
            Schedule.DaysOfWeek = (byte)SelectedDays.Aggregate(0, (m, b) => m | b);


            if (!ModelState.IsValid)
            {
                // если были ошибки валидации, заново создаём ZoneOptions и возвращаем форму
                ZoneOptions = new SelectList(_context.Zones.OrderBy(z => z.Name), "ZoneId", "Name");
                return Page();
            }

            _context.Schedules.Add(Schedule);
            await _context.SaveChangesAsync();

            // логируем создание в Events
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _context.Events.Add(new Event
            {
                ZoneId = Schedule.ZoneId,
                EventType = "ScheduleCreated",
                Timestamp = DateTimeOffset.Now,
                UserId = userId
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
