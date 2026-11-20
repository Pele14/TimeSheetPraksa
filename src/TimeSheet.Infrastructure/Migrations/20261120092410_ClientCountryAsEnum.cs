using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261120092410_ClientCountryAsEnum : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Clients_Countries_CountryId",
            table: "Clients");

        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects");

        migrationBuilder.DropTable(
            name: "Countries");

        migrationBuilder.DropIndex(
            name: "IX_Clients_CountryId",
            table: "Clients");

        migrationBuilder.DropColumn(
            name: "CountryId",
            table: "Clients");

        migrationBuilder.AddColumn<string>(
            name: "Country",
            table: "Clients",
            type: "TEXT",
            maxLength: 50,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "Country",
            table: "Clients");

        migrationBuilder.AddColumn<Guid>(
            name: "CountryId",
            table: "Clients",
            type: "TEXT",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "Countries",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Countries", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Clients_CountryId",
            table: "Clients",
            column: "CountryId");

        migrationBuilder.AddForeignKey(
            name: "FK_Clients_Countries_CountryId",
            table: "Clients",
            column: "CountryId",
            principalTable: "Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
