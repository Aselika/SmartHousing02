using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        // Метрики
        public int ZonesCount { get; private set; }
        public int SchedulesCount { get; private set; }
        public int EventsCount { get; private set; }

        // Список последних событий
        public List<Event> RecentEvents { get; private set; }

        public async Task OnGetAsync()
        {
            ZonesCount = await _db.Zones.CountAsync();
            SchedulesCount = await _db.Schedules.CountAsync();
            EventsCount = await _db.Events.CountAsync();

            RecentEvents = await _db.Events
                                   .Include(e => e.Zone)
                                   .OrderByDescending(e => e.Timestamp)
                                   .Take(5)
                                   .ToListAsync();
        }
    }
}
