using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BarManagement.Pages.Tables
{
    [Authorize(Roles = "Serveur")]
    public class EditModel : PageModel
    {
        private readonly BarManagementDbContext _context;

        public EditModel(BarManagementDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EditInputModel EditInput { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables.FirstOrDefaultAsync(table => table.Id == id);
            if (table == null)
            {
                return NotFound();
            }
            EditInput = new EditInputModel()
            {
                Id = table.Id,
                Numero = table.Numero,
                Capacite = table.Capacite
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var table = await _context.Tables.FirstOrDefaultAsync(table => table.Id == EditInput.Id);

            if (table == null)
            {
                return NotFound();
            }

            if (EditInput.Numero != table.Numero)
            {
                var numeroExist = await _context.Tables.AnyAsync(table => table.Numero == EditInput.Numero);

                if (numeroExist)
                {
                    ModelState.AddModelError("EditInput.Numero", "Ce numéro de table existe déjà");
                    return Page();
                }

                table.Numero = EditInput.Numero;
            }

            table.Capacite = EditInput.Capacite;
            _context.Update(table);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableExists(table.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TableExists(long id)
        {
            return _context.Tables.Any(e => e.Id == id);
        }
    }

    public class EditInputModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La valeur doit être supérieur à 0")]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La valeur doit être supérieur à 0")]
        public int Numero { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La valeur doit être supérieur à 0")]
        public int Capacite { get; set; }
    }
}