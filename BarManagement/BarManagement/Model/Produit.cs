using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarManagement.Model
{
    [Table("Produit")]
    public class Produit
    {
        [Key]
        public int Id { get; set; }

        public string Designation { get; set; }

        public decimal Prix { get; set; }

        public string Image { get; set; }
    }
}