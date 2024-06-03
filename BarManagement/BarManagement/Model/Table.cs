using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarManagement.Model
{
    [Table("Table")]
    public class Table
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La valeur doit être supérieure à 0")]
        public int Numero { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La valeur doit être supérieure à 0")]
        public int Capacite { get; set; }

        public string Etat { get; set; }
    }
}