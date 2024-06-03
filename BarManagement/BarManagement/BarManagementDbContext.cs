using BarManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace BarManagement
{
    public class BarManagementDbContext : DbContext
    {
        public BarManagementDbContext(DbContextOptions<BarManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProduitCommande>().HasKey(produitCommande => new { produitCommande.IdProduit, produitCommande.IdCommande });

            //modelBuilder.Entity<OrderProduct>().HasOne(orderProduct => orderProduct.Product)
            //                                   .WithMany()
            //                                   .HasForeignKey(orderProduct => orderProduct.ProductId);

            //modelBuilder.Entity<OrderProduct>().HasOne(orderProduct => orderProduct.Order)
            //                                   .WithMany(orderProduct => orderProduct.OrderProducts)
            //                                   .HasForeignKey(orderProduct => orderProduct.OrderId);
        }

        public DbSet<Table> Tables { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<ProduitCommande> ProduitsCommandes { get; set; }
    }
}