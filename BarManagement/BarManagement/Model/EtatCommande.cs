using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BarManagement.Model
{
    public enum EtatCommande
    {
        [Display(Name = "En Cours")]
        EnCours,

        [Display(Name = "Prête")]
        Prete,

        [Display(Name = "Livrée")]
        Livree,

        [Display(Name = "Encaissée")]
        Encaissee
    }
}