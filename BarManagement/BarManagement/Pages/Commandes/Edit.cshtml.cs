using BarManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarManagement.Pages.Commandes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private BarManagementDbContext _context;

        public SelectList EtatsCommande;

        public Commande Commande { get; private set; }

        [BindProperty]
        public int IdCommande { get; set; }

        [BindProperty]
        public int NumeroTable { get; set; }

        [BindProperty]
        public string Etat { get; set; }

        public EditModel(BarManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var etats = new List<string>();
            if (User.IsInRole("Serveur"))
            {
                etats.Add("EnCours");
                etats.Add("Livree");
            }
            if (User.IsInRole("Caissier"))
            {
                etats.Add("Encaissee");
            }
            if (User.IsInRole("Barman"))
            {
                etats.Add("Prete");
            }
            EtatsCommande = new SelectList(etats);

            Commande = await _context.Commandes.Include(commande => commande.ProduitsCommandes)
                                                .ThenInclude(produitCommande => produitCommande.Produit)
                                               .SingleOrDefaultAsync(commande => commande.Id == id);
            if (Commande == null)
            {
                return NotFound();
            }

            IdCommande = id;
            NumeroTable = (await _context.Tables.SingleAsync(c => c.Id == Commande.IdTable)).Numero;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var commande = await _context.Commandes.Include(commande => commande.Table).FirstOrDefaultAsync(commande => commande.Id == IdCommande);

            if (commande == null)
            {
                return NotFound();
            }

            if (Enum.TryParse<EtatCommande>(Etat, out var result))
            {
                if (result == EtatCommande.Encaissee)
                {
                    commande.Table.Etat = EtatTable.Libre.ToString();
                }
                commande.Etat = Etat;

                _context.Update(commande);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return BadRequest("Etat de la commande invalide");
        }
    }
}
