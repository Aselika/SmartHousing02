using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Pages.Zones
{

    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;

        public DetailsModel(WebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Zone Zone { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zone = await _context.Zones.FirstOrDefaultAsync(m => m.ZoneId == id);
            if (zone == null)
            {
                return NotFound();
            }
            else
            {
                Zone = zone;
            }
            return Page();
        }
    }
}
