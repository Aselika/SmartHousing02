using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        // Свойства-модели
        public List<Zone> Zones { get; private set; }
        public List<Event> Events { get; private set; }

        public async Task OnGetAsync()
        {
            Zones = await _db.Zones
                              .Take(5)
                              .ToListAsync();

            Events = await _db.Events
                              .Include(e => e.Zone)
                              .OrderByDescending(e => e.Timestamp)
                              .Take(5)
                              .ToListAsync();
        }
    }
}
