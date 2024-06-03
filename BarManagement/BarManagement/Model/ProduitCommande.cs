using System.ComponentModel.DataAnnotations.Schema;

namespace BarManagement.Model
{
    [Table("ProduitCommande")]
    public class ProduitCommande
    {
        public Produit Produit { get; set; }

        [ForeignKey(nameof(Produit))]
        public int IdProduit { get; set; }

        public Commande Commande { get; set; }

        [ForeignKey(nameof(Commande))]
        public int IdCommande { get; set; }

        public int Quantite { get; set; }
    }
}