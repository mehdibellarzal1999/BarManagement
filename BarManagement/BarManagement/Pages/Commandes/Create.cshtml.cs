using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BarManagement.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace BarManagement.Pages.Commandes
{
    public class CommandeInputModel
    {
        [Required]
        public int IdProduit { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La valeur doit être supérieur à 0")]
        public int Quantite { get; set; }
    }

    [Authorize(Roles = "Serveur")]
    public class CreateModel : PageModel
    {
        private readonly BarManagementDbContext _context;

        [BindProperty]
        public CommandeInputModel CommandeInputModel { get; set; }

        public List<Produit> Produits { get; private set; }

        public Commande Commande { get; private set; }

        public SelectList Tables { get; set; }

        [BindProperty]
        public int IdTable { get; set; }

        public CreateModel(BarManagementDbContext context)
        {
            _context = context;
            Tables = new SelectList(_context.Tables, nameof(Table.Id), nameof(Table.Numero));
        }

        public async Task OnGetAsync()
        {
            await ChargerProduits();
        }

        private async Task ChargerProduits()
        {
            Produits = await _context.Produits.ToListAsync();
            var str = HttpContext.Session.GetString("Commande");
            if (str != null)
                Commande = JsonConvert.DeserializeObject<Commande>(str);
            else
                Commande = null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await ChargerProduits();
                return Page();
            }

            var produit = await _context.Produits.FindAsync(CommandeInputModel.IdProduit);

            if (produit == null)
                return NotFound();

            var str = HttpContext.Session.GetString("Commande");
            if (str != null)
                Commande = JsonConvert.DeserializeObject<Commande>(str);
            else
                Commande = null;

            if (Commande == null)
            {
                Commande = new Commande()
                {
                    DateCreation = DateTime.Now,
                    //PaiedAmount = 0,
                    Etat = EtatCommande.EnCours.ToString(),
                    Montant = produit.Prix * CommandeInputModel.Quantite,
                    ProduitsCommandes = new List<ProduitCommande>()
                    {
                        new ProduitCommande()
                        {
                            Produit = produit,
                            IdProduit = produit.Id,
                            Quantite = CommandeInputModel.Quantite
                        }
                    }
                };
            }
            else
            {
                Commande.Montant += produit.Prix * CommandeInputModel.Quantite;
                var orderProduct = Commande.ProduitsCommandes.FirstOrDefault(produitCommande => produitCommande.IdProduit == produit.Id);
                if (orderProduct == null)
                {
                    Commande.ProduitsCommandes.Add(new ProduitCommande()
                    {
                        Produit = produit,
                        IdProduit = produit.Id,
                        Quantite = CommandeInputModel.Quantite
                    });
                }
                else
                {
                    orderProduct.Quantite += CommandeInputModel.Quantite;
                }
            }

            var orderStr = JsonConvert.SerializeObject(Commande);
            HttpContext.Session.SetString("Commande", orderStr);

            return Redirect("Create");
        }

        public async Task<IActionResult> OnPostValidateAsync()
        {
            var str = HttpContext.Session.GetString("Commande");
            if (str != null)
                Commande = JsonConvert.DeserializeObject<Commande>(str);

            if (Commande != null)
            {
                var table = await _context.Tables.SingleOrDefaultAsync(table => table.Id == IdTable);
                if (table != null)
                {
                    Commande.IdTable = table.Id;
                }
                else
                {
                    await ChargerProduits();
                    return Page();
                }
                if (table.Etat == EtatTable.Occupee.ToString())
                {
                    ModelState.AddModelError("IdTable", $"La table numéro {table.Numero} est occuppée");
                    await ChargerProduits();
                    return Page();
                }

                table.Etat = EtatTable.Occupee.ToString();
                Commande.ProduitsCommandes.ForEach(x => x.Produit = null);
                await _context.Commandes.AddRangeAsync(Commande);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("Commande");
                return Redirect("Index");
            }

            return Page();
        }
    }
}