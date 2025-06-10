using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Grocery_Store_Task_INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class BuildingDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    StartDate = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    IsGreen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.StartDate);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeSlotDate = table.Column<DateTime>(type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_TimeSlots_TimeSlotDate",
                        column: x => x.TimeSlotDate,
                        principalTable: "TimeSlots",
                        principalColumn: "StartDate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartProduct",
                columns: table => new
                {
                    CartProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProduct", x => new { x.CartProductsId, x.ProductCartId });
                    table.ForeignKey(
                        name: "FK_CartProduct_Carts_ProductCartId",
                        column: x => x.ProductCartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartProduct_Products_CartProductsId",
                        column: x => x.CartProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7000-8000-000000000001"), 0, "Ergonomic wireless mouse with adjustable DPI settings, perfect for office and casual gaming.", "Wireless Mouse" },
                    { new Guid("a1b2c3d4-e5f6-7000-8000-000000000002"), 1, "Freshly picked organic Gala apples, sweet and crisp, ideal for snacking or baking.", "Organic Apples" },
                    { new Guid("a1b2c3d4-e5f6-7000-8000-000000000003"), 2, "Handcrafted custom-made leather sofa, designed for comfort and durability. Lead time: 4 weeks.", "Custom Leather Sofa" },
                    { new Guid("a1b2c3d4-e5f6-7000-8000-000000000004"), 0, "Compact and portable Bluetooth keyboard with multi-device connectivity, great for tablets and phones.", "Bluetooth Keyboard" },
                    { new Guid("a1b2c3d4-e5f6-7000-8000-000000000005"), 1, "Freshly baked artisan sourdough bread, made with natural yeast and slow fermentation for a rich flavor.", "Artisan Sourdough" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartProduct_ProductCartId",
                table: "CartProduct",
                column: "ProductCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_TimeSlotDate",
                table: "Carts",
                column: "TimeSlotDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartProduct");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "TimeSlots");
        }
    }
}
