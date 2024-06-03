
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarManagement.Model
{
    [Table("Commande")]
    public class Commande
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreation { get; set; }

        public string Etat { get; set; }

        public decimal Montant { get; set; }

        //public decimal PaiedAmount { get; set; }

        public Table Table { get; set; }

        [ForeignKey(nameof(Table))]
        public int IdTable { get; set; }

        public List<ProduitCommande> ProduitsCommandes { get; set; }
    }
}