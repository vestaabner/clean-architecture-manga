using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    SSN = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Debit",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ExternalUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "CustomerId" },
                values: new object[] { new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), new Guid("197d0438-e04b-453d-b5de-eca05960c6ae") });

            migrationBuilder.InsertData(
                table: "Credit",
                columns: new[] { "Id", "AccountId", "Currency", "TransactionDate", "Value" },
                values: new object[] { new Guid("f5117315-e789-491a-b662-958c37237f9b"), new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), "USD", new DateTime(2020, 7, 21, 21, 1, 17, 178, DateTimeKind.Utc).AddTicks(6204), 400m });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "FirstName", "LastName", "SSN", "UserId" },
                values: new object[] { new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"), "Ivan Paulovich", "Ivan Paulovich", "8608179999", new Guid("b6c5c4ff-b850-41d1-9e3a-89be72476835") });

            migrationBuilder.InsertData(
                table: "Debit",
                columns: new[] { "Id", "AccountId", "Currency", "TransactionDate", "Value" },
                values: new object[] { new Guid("3d6032df-7a3b-46e6-8706-be971e3d539f"), new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), "USD", new DateTime(2020, 7, 21, 21, 1, 17, 178, DateTimeKind.Utc).AddTicks(9632), 400m });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "ExternalUserId" },
                values: new object[] { new Guid("d3ed8166-af7f-4f55-8407-6d426d48dadf"), "Ivan Paulovich" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Debit");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
