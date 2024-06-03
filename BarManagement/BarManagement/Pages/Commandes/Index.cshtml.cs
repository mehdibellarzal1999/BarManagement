using BarManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarManagement.Pages.Commandes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly BarManagementDbContext _context;

        public IList<Commande> Commandes { get; private set; }

        public IndexModel(BarManagementDbContext context)
        {
            _context = context;
        }

        public async Task OnGet()
        {
            Commandes = await _context.Commandes.Include(commande => commande.Table)
                                                .OrderByDescending(commande => commande.DateCreation)
                                                .ToListAsync();
        }
    }
}