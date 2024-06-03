using BarManagement.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarManagement.Pages
{
    public class ProductModel : PageModel
    {
        private readonly BarManagementDbContext _context;

        public List<Produit> Products { get; set; }

        public ProductModel(BarManagementDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Products = await _context.Produits.ToListAsync();
        }
    }
}