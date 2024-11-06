using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    ProductCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "Name", "Price", "ProductCode", "ProductImage" },
                values: new object[,]
                {
                    { new Guid("2d8d45f6-80e0-4acc-9685-652da49bc67b"), new DateTime(2024, 11, 6, 19, 27, 35, 864, DateTimeKind.Local).AddTicks(6438), false, "Muz", 30m, "MUZ", "https://upload.wikimedia.org/wikipedia/commons/8/8a/Banana-Single.jpg" },
                    { new Guid("ace77301-fc64-4da9-bb59-9657d3335b28"), new DateTime(2024, 11, 6, 19, 27, 35, 864, DateTimeKind.Local).AddTicks(6429), false, "Elma", 15m, "ELMA", "https://upload.wikimedia.org/wikipedia/commons/1/15/Red_Apple.jpg" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "IsDeleted", "Password", "UserType", "Username" },
                values: new object[] { new Guid("d2f62e3e-4db4-4ae7-a9f7-96c5d90f77fc"), false, "GGz3dMl7YKHBBu9xjRCXCmoG4GvviVU9muZdk4qIbq4=", 1, "superadmin" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
