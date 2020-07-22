using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    SSN = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    CreditId = table.Column<Guid>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.CreditId);
                    table.ForeignKey(
                        name: "FK_Credit_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Debit",
                columns: table => new
                {
                    DebitId = table.Column<Guid>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debit", x => x.DebitId);
                    table.ForeignKey(
                        name: "FK_Debit_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "ExternalUserId" },
                values: new object[] { new Guid("e278ee65-6c41-42d6-9a73-838199a44d62"), "github/ivanpaulovich" });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerId", "FirstName", "LastName", "SSN", "UserId" },
                values: new object[] { new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"), "Ivan Paulovich", "Ivan Paulovich", "8608179999", new Guid("e278ee65-6c41-42d6-9a73-838199a44d62") });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountId", "CustomerId" },
                values: new object[] { new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), new Guid("197d0438-e04b-453d-b5de-eca05960c6ae") });

            migrationBuilder.InsertData(
                table: "Credit",
                columns: new[] { "CreditId", "AccountId", "Currency", "TransactionDate", "Value" },
                values: new object[] { new Guid("7bf066ba-379a-4e72-a59b-9755fda432ce"), new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), "USD", new DateTime(2020, 7, 22, 14, 13, 10, 752, DateTimeKind.Utc).AddTicks(5459), 400m });

            migrationBuilder.InsertData(
                table: "Debit",
                columns: new[] { "DebitId", "AccountId", "Currency", "TransactionDate", "Value" },
                values: new object[] { new Guid("31ade963-bd69-4afb-9df7-611ae2cfa651"), new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), "USD", new DateTime(2020, 7, 22, 14, 13, 10, 752, DateTimeKind.Utc).AddTicks(9184), 400m });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CustomerId",
                table: "Account",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Credit_AccountId",
                table: "Credit",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserId",
                table: "Customer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Debit_AccountId",
                table: "Debit",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropTable(
                name: "Debit");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
