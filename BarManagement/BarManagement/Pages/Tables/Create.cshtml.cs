using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BarManagement.Model;
using Microsoft.EntityFrameworkCore;
using Table = BarManagement.Model.Table;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BarManagement.Pages.Tables
{
    [Authorize(Roles = "Serveur")]
    public class CreateModel : PageModel
    {
        private readonly BarManagementDbContext _context;

        public CreateModel(BarManagementDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Table Table { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var numeroExist = await _context.Tables.AnyAsync(table => table.Numero == Table.Numero);

            if (numeroExist)
            {
                ModelState.AddModelError("Table.Numero", "Ce numéro de table existe déjà");
                return Page();
            }

            Table.Etat = EtatTable.Libre.ToString();

            _context.Tables.Add(Table);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}