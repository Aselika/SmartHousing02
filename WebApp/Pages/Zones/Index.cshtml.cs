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
    public class IndexModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;

        public IndexModel(WebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Zone> Zone { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Zone = await _context.Zones.ToListAsync();
        }
    }
}
