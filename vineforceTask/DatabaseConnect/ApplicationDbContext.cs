using Microsoft.EntityFrameworkCore;
using vineforceTask.Models;

namespace vineforceTask.DatabaseConnect
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Decimal precision ----------
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            // ---------- Relationships ----------
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Seed Date ----------
            var seedDate = new DateTime(
                2025,
                1,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc
            );

            // ============================================================
            // PRODUCTS
            // ============================================================

            var products = new[]
            {
                // ---------------- ELECTRONICS ----------------

                new Product
                {
                    Id = 1,
                    Name = "Samsung Galaxy M14 5G",
                    Description = "6.6\" display, 6000mAh battery, 50MP camera",
                    Price = 11999m,
                    Sku = "SKU-ELEC-001",
                    StockQuantity = 120,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Galaxy+M14",
                    ProductUrl = "https://www.amazon.in/s?k=Samsung+Galaxy+M14+5G",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 2,
                    Name = "boAt Airdopes 141",
                    Description = "True wireless earbuds, 42H playback",
                    Price = 1299m,
                    Sku = "SKU-ELEC-002",
                    StockQuantity = 300,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Airdopes+141",
                    ProductUrl = "https://www.boat-lifestyle.com/products/airdopes-141",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 3,
                    Name = "Mi Power Bank 3i 20000mAh",
                    Description = "18W fast charging power bank",
                    Price = 1699m,
                    Sku = "SKU-ELEC-003",
                    StockQuantity = 200,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Mi+Power+Bank",
                    ProductUrl = "https://in.event.mi.com/in/power-banks",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 4,
                    Name = "HP 15s Laptop (i5, 8GB, 512GB SSD)",
                    Description = "15.6\" FHD display, Windows 11",
                    Price = 52990m,
                    Sku = "SKU-ELEC-004",
                    StockQuantity = 40,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=HP+15s+Laptop",
                    ProductUrl = "https://www.amazon.in/s?k=HP+15s+i5+8GB+512GB+SSD",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 5,
                    Name = "Noise ColorFit Pro 4 Smartwatch",
                    Description = "1.72\" AMOLED display, Bluetooth calling",
                    Price = 2499m,
                    Sku = "SKU-ELEC-005",
                    StockQuantity = 150,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=ColorFit+Pro+4",
                    ProductUrl = "https://www.amazon.in/s?k=Noise+ColorFit+Pro+4",
                    CreatedAt = seedDate
                },

                // ---------------- HOME ----------------

                new Product
                {
                    Id = 6,
                    Name = "Prestige Electric Kettle 1.5L",
                    Description = "1500W, auto shut-off, stainless steel body",
                    Price = 899m,
                    Sku = "SKU-HOME-001",
                    StockQuantity = 250,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Prestige+Kettle",
                    ProductUrl = "https://www.amazon.in/s?k=Prestige+Electric+Kettle+1.5L",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 7,
                    Name = "Milton Thermosteel Flask 1L",
                    Description = "24-hour hot/cold insulated flask",
                    Price = 799m,
                    Sku = "SKU-HOME-002",
                    StockQuantity = 180,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Milton+Flask",
                    ProductUrl = "https://www.amazon.in/s?k=Milton+Thermosteel+Flask+1L",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 8,
                    Name = "Philips Air Fryer HD9200",
                    Description = "4.1L capacity, rapid air technology",
                    Price = 6495m,
                    Sku = "SKU-HOME-003",
                    StockQuantity = 60,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Philips+Air+Fryer",
                    ProductUrl = "https://www.domesticappliances.philips.co.in/products/philips-analog-4-1-ltr-airfryer-with-rapid-air-technology-hd9200-90",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 9,
                    Name = "Cello Storage Container Set (18 pcs)",
                    Description = "Airtight kitchen storage containers",
                    Price = 1099m,
                    Sku = "SKU-HOME-004",
                    StockQuantity = 220,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Cello+Container+Set",
                    ProductUrl = "https://www.amazon.in/s?k=Cello+Storage+Container+Set+18+pcs",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 10,
                    Name = "Bajaj Ceiling Fan 1200mm",
                    Description = "High-speed 3-blade ceiling fan",
                    Price = 1799m,
                    Sku = "SKU-HOME-005",
                    StockQuantity = 90,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Bajaj+Ceiling+Fan",
                    ProductUrl = "https://www.amazon.in/s?k=Bajaj+Ceiling+Fan+1200mm",
                    CreatedAt = seedDate
                },

                // ---------------- FASHION ----------------

                new Product
                {
                    Id = 11,
                    Name = "Allen Solly Men's Formal Shirt",
                    Description = "Slim fit cotton formal shirt",
                    Price = 1299m,
                    Sku = "SKU-FASH-001",
                    StockQuantity = 300,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Allen+Solly+Shirt",
                    ProductUrl = "https://allensolly.abfrl.in/p/men-white-regular-fit-solid-full-sleeves-formal-shirts-39725028.html",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 12,
                    Name = "Levi's 511 Slim Fit Jeans",
                    Description = "Men's slim fit denim jeans",
                    Price = 2599m,
                    Sku = "SKU-FASH-002",
                    StockQuantity = 150,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Levis+511+Jeans",
                    ProductUrl = "https://levi.in/products/mens-511-slim-fit-blue-jeans-182981650",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 13,
                    Name = "Puma Men's Running Shoes",
                    Description = "Lightweight mesh running shoes",
                    Price = 3499m,
                    Sku = "SKU-FASH-003",
                    StockQuantity = 130,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Puma+Running+Shoes",
                    ProductUrl = "https://in.puma.com/in/en/mens/mens-sport/mens-sport-running",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 14,
                    Name = "Fastrack Analog Watch for Men",
                    Description = "Stainless steel strap, water resistant",
                    Price = 1895m,
                    Sku = "SKU-FASH-004",
                    StockQuantity = 100,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Fastrack+Watch",
                    ProductUrl = "https://www.fastrack.in/shop/analog-watches",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 15,
                    Name = "Wildcraft Backpack 30L",
                    Description = "Water-resistant laptop backpack",
                    Price = 1799m,
                    Sku = "SKU-FASH-005",
                    StockQuantity = 170,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Wildcraft+Backpack",
                    ProductUrl = "https://www.amazon.in/s?k=Wildcraft+Backpack+30L",
                    CreatedAt = seedDate
                },

                // ---------------- SPORTS ----------------

                new Product
                {
                    Id = 16,
                    Name = "Amazon Basics Yoga Mat",
                    Description = "6mm thick, non-slip surface",
                    Price = 699m,
                    Sku = "SKU-SPRT-001",
                    StockQuantity = 200,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Yoga+Mat",
                    ProductUrl = "https://www.amazon.in/s?k=Amazon+Basics+Yoga+Mat+6mm",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 17,
                    Name = "Kore PVC Dumbbell Set 10kg",
                    Description = "Home gym dumbbell set with rods",
                    Price = 1399m,
                    Sku = "SKU-SPRT-002",
                    StockQuantity = 80,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Dumbbell+Set",
                    ProductUrl = "https://www.amazon.in/s?k=Kore+PVC+Dumbbell+Set+10kg",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 18,
                    Name = "Nivia Football Size 5",
                    Description = "Synthetic leather match football",
                    Price = 599m,
                    Sku = "SKU-SPRT-003",
                    StockQuantity = 250,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Nivia+Football",
                    ProductUrl = "https://www.amazon.in/s?k=Nivia+Football+Size+5",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 19,
                    Name = "Strauss Skipping Rope",
                    Description = "Adjustable speed skipping rope",
                    Price = 349m,
                    Sku = "SKU-SPRT-004",
                    StockQuantity = 400,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Skipping+Rope",
                    ProductUrl = "https://www.amazon.in/s?k=Strauss+Skipping+Rope",
                    CreatedAt = seedDate
                },

                new Product
                {
                    Id = 20,
                    Name = "Cosco Badminton Racket Set",
                    Description = "2 rackets + 3 shuttlecocks combo",
                    Price = 899m,
                    Sku = "SKU-SPRT-005",
                    StockQuantity = 140,
                    IsActive = true,
                    ImageUrl = "https://placehold.co/600x600?text=Badminton+Set",
                    ProductUrl = "https://www.amazon.in/s?k=Cosco+Badminton+Racket+Set",
                    CreatedAt = seedDate
                }
            };

            modelBuilder.Entity<Product>().HasData(products);


            // ============================================================
            // ORDERS
            // ============================================================

            var orders = new List<Order>();

            for (int i = 1; i <= 20; i++)
            {
                var product = products[i - 1];

                int qty = (i % 3) + 1;

                decimal unitPrice = product.Price;

                decimal total = unitPrice * qty;

                orders.Add(new Order
                {
                    Id = i,

                    UserId = $"USR-{Guid.NewGuid():N}"
                        .Substring(0, 12)
                        .ToUpper(),

                    ProductId = product.Id,

                    Quantity = qty,

                    UnitPrice = unitPrice,

                    TotalAmount = total,

                    Status = (OrderStatus)(i % 5),

                    CustomerName = $"Customer {i}",

                    CustomerEmail = $"customer{i}@example.com",

                    CustomerPhone =
                        $"9{100000000 + (i * 7654321) % 899999999}",

                    ShippingAddress =
                        $"House No. {i}, Sector {i + 5}, Sample City, Haryana - {121001 + i}",

                    CreatedAt = seedDate.AddDays(i)
                });
            }

            modelBuilder.Entity<Order>().HasData(orders);

        }
    }
}