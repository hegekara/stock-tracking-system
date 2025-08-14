using Microsoft.EntityFrameworkCore;
using Backend.Api.Models;

namespace Backend.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CityName).HasMaxLength(100);
            entity.Property(e => e.DistrictName).HasMaxLength(100);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            
            entity.HasOne(e => e.Address)
                  .WithMany()
                  .HasForeignKey("AddressId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderDate).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>();
            
            entity.HasOne(e => e.Customer)
                  .WithMany()
                  .HasForeignKey("CustomerId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.Id });
            
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Order)
                  .WithMany()
                  .HasForeignKey("OrderId")
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Product)
                  .WithMany()
                  .HasForeignKey("ProductId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.UnitsInStock).IsRequired();
            
            entity.HasOne(e => e.Category)
                  .WithMany()
                  .HasForeignKey("CategoryId")
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasOne(e => e.Supplier)
                  .WithMany()
                  .HasForeignKey("SupplierId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TransactionDate).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.TransactionType).HasConversion<string>();
            
            entity.HasOne(e => e.Product)
                  .WithMany()
                  .HasForeignKey("ProductId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            
            entity.HasOne(e => e.Address)
                  .WithMany()
                  .HasForeignKey("AddressId")
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Address>().HasQueryFilter(e => !e.Deleted);
        modelBuilder.Entity<Category>().HasQueryFilter(e => e.Deleted != true);
        modelBuilder.Entity<Customer>().HasQueryFilter(e => e.Deleted != true);
        modelBuilder.Entity<Order>().HasQueryFilter(e => e.Deleted != true);
        modelBuilder.Entity<OrderDetail>().HasQueryFilter(e => e.Deleted != true);
        modelBuilder.Entity<Product>().HasQueryFilter(e => e.Deleted != true);
        modelBuilder.Entity<StockTransaction>().HasQueryFilter(e => e.Deleted != true);
        modelBuilder.Entity<Supplier>().HasQueryFilter(e => e.Deleted != true);
    }
}