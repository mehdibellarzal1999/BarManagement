using BarManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarManagement.Pages.Commandes
{
    public class DetailsModel : PageModel
    {
        private readonly BarManagementDbContext _context;

        public Commande Commande { get; private set; }

        public DetailsModel(BarManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Commande = await _context.Commandes.Include(commande => commande.Table)
                                               .Include(commande => commande.ProduitsCommandes)
                                               .ThenInclude(produitCommande => produitCommande.Produit)
                                               .SingleOrDefaultAsync(commande => commande.Id == id);

            if (Commande == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
