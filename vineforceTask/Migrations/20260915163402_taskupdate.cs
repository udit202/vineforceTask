using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace vineforceTask.Migrations
{
    /// <inheritdoc />
    public partial class taskupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    RazorpayOrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazorpayPaymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazorpaySignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Price", "ProductUrl", "Sku", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6.6\" display, 6000mAh battery, 50MP camera", "https://placehold.co/600x600?text=Galaxy+M14", true, "Samsung Galaxy M14 5G", 11999m, "https://www.amazon.in/s?k=Samsung+Galaxy+M14+5G", "SKU-ELEC-001", 120, null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "True wireless earbuds, 42H playback", "https://placehold.co/600x600?text=Airdopes+141", true, "boAt Airdopes 141", 1299m, "https://www.boat-lifestyle.com/products/airdopes-141", "SKU-ELEC-002", 300, null },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "18W fast charging power bank", "https://placehold.co/600x600?text=Mi+Power+Bank", true, "Mi Power Bank 3i 20000mAh", 1699m, "https://in.event.mi.com/in/power-banks", "SKU-ELEC-003", 200, null },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "15.6\" FHD display, Windows 11", "https://placehold.co/600x600?text=HP+15s+Laptop", true, "HP 15s Laptop (i5, 8GB, 512GB SSD)", 52990m, "https://www.amazon.in/s?k=HP+15s+i5+8GB+512GB+SSD", "SKU-ELEC-004", 40, null },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1.72\" AMOLED display, Bluetooth calling", "https://placehold.co/600x600?text=ColorFit+Pro+4", true, "Noise ColorFit Pro 4 Smartwatch", 2499m, "https://www.amazon.in/s?k=Noise+ColorFit+Pro+4", "SKU-ELEC-005", 150, null },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1500W, auto shut-off, stainless steel body", "https://placehold.co/600x600?text=Prestige+Kettle", true, "Prestige Electric Kettle 1.5L", 899m, "https://www.amazon.in/s?k=Prestige+Electric+Kettle+1.5L", "SKU-HOME-001", 250, null },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "24-hour hot/cold insulated flask", "https://placehold.co/600x600?text=Milton+Flask", true, "Milton Thermosteel Flask 1L", 799m, "https://www.amazon.in/s?k=Milton+Thermosteel+Flask+1L", "SKU-HOME-002", 180, null },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "4.1L capacity, rapid air technology", "https://placehold.co/600x600?text=Philips+Air+Fryer", true, "Philips Air Fryer HD9200", 6495m, "https://www.domesticappliances.philips.co.in/products/philips-analog-4-1-ltr-airfryer-with-rapid-air-technology-hd9200-90", "SKU-HOME-003", 60, null },
                    { 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Airtight kitchen storage containers", "https://placehold.co/600x600?text=Cello+Container+Set", true, "Cello Storage Container Set (18 pcs)", 1099m, "https://www.amazon.in/s?k=Cello+Storage+Container+Set+18+pcs", "SKU-HOME-004", 220, null },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High-speed 3-blade ceiling fan", "https://placehold.co/600x600?text=Bajaj+Ceiling+Fan", true, "Bajaj Ceiling Fan 1200mm", 1799m, "https://www.amazon.in/s?k=Bajaj+Ceiling+Fan+1200mm", "SKU-HOME-005", 90, null },
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Slim fit cotton formal shirt", "https://placehold.co/600x600?text=Allen+Solly+Shirt", true, "Allen Solly Men's Formal Shirt", 1299m, "https://allensolly.abfrl.in/p/men-white-regular-fit-solid-full-sleeves-formal-shirts-39725028.html", "SKU-FASH-001", 300, null },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Men's slim fit denim jeans", "https://placehold.co/600x600?text=Levis+511+Jeans", true, "Levi's 511 Slim Fit Jeans", 2599m, "https://levi.in/products/mens-511-slim-fit-blue-jeans-182981650", "SKU-FASH-002", 150, null },
                    { 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lightweight mesh running shoes", "https://placehold.co/600x600?text=Puma+Running+Shoes", true, "Puma Men's Running Shoes", 3499m, "https://in.puma.com/in/en/mens/mens-sport/mens-sport-running", "SKU-FASH-003", 130, null },
                    { 14, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stainless steel strap, water resistant", "https://placehold.co/600x600?text=Fastrack+Watch", true, "Fastrack Analog Watch for Men", 1895m, "https://www.fastrack.in/shop/analog-watches", "SKU-FASH-004", 100, null },
                    { 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Water-resistant laptop backpack", "https://placehold.co/600x600?text=Wildcraft+Backpack", true, "Wildcraft Backpack 30L", 1799m, "https://www.amazon.in/s?k=Wildcraft+Backpack+30L", "SKU-FASH-005", 170, null },
                    { 16, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "6mm thick, non-slip surface", "https://placehold.co/600x600?text=Yoga+Mat", true, "Amazon Basics Yoga Mat", 699m, "https://www.amazon.in/s?k=Amazon+Basics+Yoga+Mat+6mm", "SKU-SPRT-001", 200, null },
                    { 17, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Home gym dumbbell set with rods", "https://placehold.co/600x600?text=Dumbbell+Set", true, "Kore PVC Dumbbell Set 10kg", 1399m, "https://www.amazon.in/s?k=Kore+PVC+Dumbbell+Set+10kg", "SKU-SPRT-002", 80, null },
                    { 18, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Synthetic leather match football", "https://placehold.co/600x600?text=Nivia+Football", true, "Nivia Football Size 5", 599m, "https://www.amazon.in/s?k=Nivia+Football+Size+5", "SKU-SPRT-003", 250, null },
                    { 19, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Adjustable speed skipping rope", "https://placehold.co/600x600?text=Skipping+Rope", true, "Strauss Skipping Rope", 349m, "https://www.amazon.in/s?k=Strauss+Skipping+Rope", "SKU-SPRT-004", 400, null },
                    { 20, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2 rackets + 3 shuttlecocks combo", "https://placehold.co/600x600?text=Badminton+Set", true, "Cosco Badminton Racket Set", 899m, "https://www.amazon.in/s?k=Cosco+Badminton+Racket+Set", "SKU-SPRT-005", 140, null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "CustomerEmail", "CustomerName", "CustomerPhone", "ProductId", "Quantity", "ShippingAddress", "Status", "TotalAmount", "UnitPrice", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "customer1@example.com", "Customer 1", "9107654321", 1, 2, "House No. 1, Sector 6, Sample City, Haryana - 121002", 1, 23998m, 11999m, null, "USR-2D5CA79E" },
                    { 2, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "customer2@example.com", "Customer 2", "9115308642", 2, 3, "House No. 2, Sector 7, Sample City, Haryana - 121003", 2, 3897m, 1299m, null, "USR-D9A0D80B" },
                    { 3, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "customer3@example.com", "Customer 3", "9122962963", 3, 1, "House No. 3, Sector 8, Sample City, Haryana - 121004", 3, 1699m, 1699m, null, "USR-9DA86BD2" },
                    { 4, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "customer4@example.com", "Customer 4", "9130617284", 4, 2, "House No. 4, Sector 9, Sample City, Haryana - 121005", 4, 105980m, 52990m, null, "USR-F53CBADB" },
                    { 5, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "customer5@example.com", "Customer 5", "9138271605", 5, 3, "House No. 5, Sector 10, Sample City, Haryana - 121006", 0, 7497m, 2499m, null, "USR-4C1B9CBE" },
                    { 6, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "customer6@example.com", "Customer 6", "9145925926", 6, 1, "House No. 6, Sector 11, Sample City, Haryana - 121007", 1, 899m, 899m, null, "USR-BF902AFD" },
                    { 7, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "customer7@example.com", "Customer 7", "9153580247", 7, 2, "House No. 7, Sector 12, Sample City, Haryana - 121008", 2, 1598m, 799m, null, "USR-0F393EAC" },
                    { 8, new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), "customer8@example.com", "Customer 8", "9161234568", 8, 3, "House No. 8, Sector 13, Sample City, Haryana - 121009", 3, 19485m, 6495m, null, "USR-C2D04583" },
                    { 9, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "customer9@example.com", "Customer 9", "9168888889", 9, 1, "House No. 9, Sector 14, Sample City, Haryana - 121010", 4, 1099m, 1099m, null, "USR-811400D1" },
                    { 10, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), "customer10@example.com", "Customer 10", "9176543210", 10, 2, "House No. 10, Sector 15, Sample City, Haryana - 121011", 0, 3598m, 1799m, null, "USR-9228F6FE" },
                    { 11, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "customer11@example.com", "Customer 11", "9184197531", 11, 3, "House No. 11, Sector 16, Sample City, Haryana - 121012", 1, 3897m, 1299m, null, "USR-D5AC564F" },
                    { 12, new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), "customer12@example.com", "Customer 12", "9191851852", 12, 1, "House No. 12, Sector 17, Sample City, Haryana - 121013", 2, 2599m, 2599m, null, "USR-F6A013A8" },
                    { 13, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), "customer13@example.com", "Customer 13", "9199506173", 13, 2, "House No. 13, Sector 18, Sample City, Haryana - 121014", 3, 6998m, 3499m, null, "USR-B9614A5A" },
                    { 14, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "customer14@example.com", "Customer 14", "9207160494", 14, 3, "House No. 14, Sector 19, Sample City, Haryana - 121015", 4, 5685m, 1895m, null, "USR-2211662B" },
                    { 15, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), "customer15@example.com", "Customer 15", "9214814815", 15, 1, "House No. 15, Sector 20, Sample City, Haryana - 121016", 0, 1799m, 1799m, null, "USR-CB341CB4" },
                    { 16, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), "customer16@example.com", "Customer 16", "9222469136", 16, 2, "House No. 16, Sector 21, Sample City, Haryana - 121017", 1, 1398m, 699m, null, "USR-D4BC450A" },
                    { 17, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), "customer17@example.com", "Customer 17", "9230123457", 17, 3, "House No. 17, Sector 22, Sample City, Haryana - 121018", 2, 4197m, 1399m, null, "USR-7BD4E559" },
                    { 18, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), "customer18@example.com", "Customer 18", "9237777778", 18, 1, "House No. 18, Sector 23, Sample City, Haryana - 121019", 3, 599m, 599m, null, "USR-40417B0F" },
                    { 19, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "customer19@example.com", "Customer 19", "9245432099", 19, 2, "House No. 19, Sector 24, Sample City, Haryana - 121020", 4, 698m, 349m, null, "USR-9814BA8A" },
                    { 20, new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), "customer20@example.com", "Customer 20", "9253086420", 20, 3, "House No. 20, Sector 25, Sample City, Haryana - 121021", 0, 2697m, 899m, null, "USR-9AA5D3CD" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
