using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BarManagement.Model;
using Microsoft.AspNetCore.Authorization;

namespace BarManagement.Pages.Tables
{
    [Authorize(Roles = "Serveur")]
    public class IndexModel : PageModel
    {
        private readonly BarManagementDbContext _context;

        public IndexModel(BarManagementDbContext context)
        {
            _context = context;
        }

        public IList<Table> Table { get; set; }

        public async Task OnGetAsync()
        {
            Table = await _context.Tables.OrderBy(table => table.Numero).ToListAsync();
        }
    }
}