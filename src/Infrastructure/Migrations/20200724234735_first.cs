using System;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.Contracts;

namespace Infrastructure.Migrations
{
  public partial class first : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      if (migrationBuilder == null)
        throw new ArgumentNullException(nameof(migrationBuilder));

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
            Amount = table.Column<decimal>(nullable: false),
            TransactionDate = table.Column<DateTime>(nullable: false),
            AccountId = table.Column<Guid>(nullable: false)
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
            Name = table.Column<string>(nullable: false),
            SSN = table.Column<string>(nullable: false)
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
            Amount = table.Column<decimal>(nullable: false),
            TransactionDate = table.Column<DateTime>(nullable: false),
            AccountId = table.Column<Guid>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Debit", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "User",
          columns: table => new
          {
            CustomerId = table.Column<Guid>(nullable: false),
            ExternalUserId = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_User", x => new { x.ExternalUserId, x.CustomerId });
          });

      migrationBuilder.InsertData(
          table: "Account",
          columns: new[] { "Id", "CustomerId" },
          values: new object[] { new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), new Guid("197d0438-e04b-453d-b5de-eca05960c6ae") });

      migrationBuilder.InsertData(
          table: "Credit",
          columns: new[] { "Id", "AccountId", "Amount", "TransactionDate" },
          values: new object[] { new Guid("f5117315-e789-491a-b662-958c37237f9b"), new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), 400m, new DateTime(2020, 7, 24, 23, 47, 34, 909, DateTimeKind.Utc).AddTicks(9410) });

      migrationBuilder.InsertData(
          table: "Customer",
          columns: new[] { "Id", "Name", "SSN" },
          values: new object[] { new Guid("197d0438-e04b-453d-b5de-eca05960c6ae"), "Ivan Paulovich", "8608179999" });

      migrationBuilder.InsertData(
          table: "Debit",
          columns: new[] { "Id", "AccountId", "Amount", "TransactionDate" },
          values: new object[] { new Guid("3d6032df-7a3b-46e6-8706-be971e3d539f"), new Guid("4c510cfe-5d61-4a46-a3d9-c4313426655f"), 400m, new DateTime(2020, 7, 24, 23, 47, 34, 910, DateTimeKind.Utc).AddTicks(2750) });

      migrationBuilder.InsertData(
          table: "User",
          columns: new[] { "ExternalUserId", "CustomerId" },
          values: new object[] { "Ivan Paulovich", new Guid("197d0438-e04b-453d-b5de-eca05960c6ae") });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      if (migrationBuilder == null)
        throw new ArgumentNullException(nameof(migrationBuilder));

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
