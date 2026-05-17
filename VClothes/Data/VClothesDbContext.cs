using System.IO;
using Microsoft.EntityFrameworkCore;
using VClothes.Models;

namespace VClothes.Data;

public class VClothesDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
    public DbSet<SalesInvoice> SalesInvoices { get; set; }
    public DbSet<SalesInvoiceDetail> SalesInvoiceDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VClothes.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<PurchaseInvoice>()
            .HasOne(pi => pi.Supplier)
            .WithMany()
            .HasForeignKey(pi => pi.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseInvoice>()
            .HasOne(pi => pi.Employee)
            .WithMany()
            .HasForeignKey(pi => pi.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseInvoiceDetail>()
            .HasOne(d => d.PurchaseInvoice)
            .WithMany(pi => pi.Details)
            .HasForeignKey(d => d.PurchaseInvoiceId);

        modelBuilder.Entity<PurchaseInvoiceDetail>()
            .HasOne(d => d.Product)
            .WithMany(p => p.PurchaseInvoiceDetails)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.Customer)
            .WithMany(c => c.SalesInvoices)
            .HasForeignKey(si => si.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.Employee)
            .WithMany()
            .HasForeignKey(si => si.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoiceDetail>()
            .HasOne(d => d.SalesInvoice)
            .WithMany(si => si.Details)
            .HasForeignKey(d => d.SalesInvoiceId);

        modelBuilder.Entity<SalesInvoiceDetail>()
            .HasOne(d => d.Product)
            .WithMany(p => p.SalesInvoiceDetails)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "Quản trị viên - Toàn quyền" },
            new Role { Id = 2, Name = "Manager", Description = "Quản lý - Quản lý cửa hàng" },
            new Role { Id = 3, Name = "Staff", Description = "Nhân viên - Bán hàng" }
        );

        // Seed Users (password: 123456 - simple hash for demo)
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", PasswordHash = "e10adc3949ba59abbe56e057f20f883e", DisplayName = "Quản trị viên", RoleId = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new User { Id = 2, Username = "manager", PasswordHash = "e10adc3949ba59abbe56e057f20f883e", DisplayName = "Quản lý", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new User { Id = 3, Username = "staff", PasswordHash = "e10adc3949ba59abbe56e057f20f883e", DisplayName = "Nhân viên", RoleId = 3, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Áo thun nam", Description = "Các loại áo thun dành cho nam", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 2, Name = "Áo thun nữ", Description = "Các loại áo thun dành cho nữ", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 3, Name = "Áo thun unisex", Description = "Áo thun cho cả nam và nữ", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 4, Name = "Áo thun trẻ em", Description = "Áo thun dành cho trẻ em", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Category { Id = 5, Name = "Áo polo", Description = "Áo polo các loại", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Suppliers
        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, Name = "Công ty TNHH Dệt May Việt Tiến", Address = "123 Nguyễn Trãi, Q.1, TP.HCM", Phone = "028-38123456", Email = "viettien@email.com", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Supplier { Id = 2, Name = "Công ty CP May Nhà Bè", Address = "456 Lê Lợi, Q.7, TP.HCM", Phone = "028-38654321", Email = "nhabe@email.com", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Supplier { Id = 3, Name = "Xưởng may Đại Phát", Address = "789 Trần Hưng Đạo, Q.5, TP.HCM", Phone = "028-38789012", Email = "daiphat@email.com", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Employees
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, EmployeeCode = "NV001", FullName = "Nguyễn Văn An", Gender = "Nam", Phone = "0901234567", Email = "an@vclothes.com", Position = "Quản lý", IsActive = true, HireDate = new DateTime(2024, 1, 1), UserId = 2 },
            new Employee { Id = 2, EmployeeCode = "NV002", FullName = "Trần Thị Bình", Gender = "Nữ", Phone = "0912345678", Email = "binh@vclothes.com", Position = "Nhân viên bán hàng", IsActive = true, HireDate = new DateTime(2024, 1, 15), UserId = 3 },
            new Employee { Id = 3, EmployeeCode = "NV003", FullName = "Lê Hoàng Cường", Gender = "Nam", Phone = "0923456789", Email = "cuong@vclothes.com", Position = "Nhân viên kho", IsActive = true, HireDate = new DateTime(2024, 2, 1) }
        );

        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FullName = "Phạm Minh Đức", Phone = "0934567890", Email = "duc@gmail.com", Address = "12 Lý Thường Kiệt, Q.10, TP.HCM", CreatedAt = new DateTime(2024, 1, 1) },
            new Customer { Id = 2, FullName = "Hoàng Thị Em", Phone = "0945678901", Email = "em@gmail.com", Address = "34 Hai Bà Trưng, Q.3, TP.HCM", CreatedAt = new DateTime(2024, 1, 1) },
            new Customer { Id = 3, FullName = "Võ Văn Phúc", Phone = "0956789012", Email = "phuc@gmail.com", Address = "56 Pasteur, Q.1, TP.HCM", CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, ProductCode = "SP001", Name = "Áo thun nam cổ tròn trắng", Price = 199000, CostPrice = 120000, StockQuantity = 50, Size = "M,L,XL", Color = "Trắng", Material = "Cotton 100%", CategoryId = 1, SupplierId = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 2, ProductCode = "SP002", Name = "Áo thun nam cổ tròn đen", Price = 199000, CostPrice = 120000, StockQuantity = 45, Size = "M,L,XL", Color = "Đen", Material = "Cotton 100%", CategoryId = 1, SupplierId = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 3, ProductCode = "SP003", Name = "Áo thun nữ form rộng", Price = 179000, CostPrice = 100000, StockQuantity = 60, Size = "S,M,L", Color = "Hồng", Material = "Cotton pha", CategoryId = 2, SupplierId = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 4, ProductCode = "SP004", Name = "Áo thun unisex oversize", Price = 249000, CostPrice = 150000, StockQuantity = 35, Size = "M,L,XL,XXL", Color = "Xám", Material = "Cotton 95%", CategoryId = 3, SupplierId = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 5, ProductCode = "SP005", Name = "Áo polo nam cao cấp", Price = 349000, CostPrice = 200000, StockQuantity = 25, Size = "M,L,XL", Color = "Xanh navy", Material = "Cotton Pique", CategoryId = 5, SupplierId = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Product { Id = 6, ProductCode = "SP006", Name = "Áo thun trẻ em hoạt hình", Price = 129000, CostPrice = 70000, StockQuantity = 80, Size = "S,M,L", Color = "Nhiều màu", Material = "Cotton", CategoryId = 4, SupplierId = 3, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );
    }
}
