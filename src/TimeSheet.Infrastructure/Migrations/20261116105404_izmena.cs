using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261116105404_izmena : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "TimeEntries",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletedAt",
            table: "TimeEntries",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "TimeEntries",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "TimeEntries",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "TeamMembers",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletedAt",
            table: "TeamMembers",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "TeamMembers",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "TeamMembers",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Projects",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletedAt",
            table: "Projects",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Projects",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Projects",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Countries",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletedAt",
            table: "Countries",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Countries",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Countries",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Clients",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletedAt",
            table: "Clients",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Clients",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Clients",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Categories",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletedAt",
            table: "Categories",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Categories",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Categories",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "TimeEntries");

        migrationBuilder.DropColumn(
            name: "DeletedAt",
            table: "TimeEntries");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "TimeEntries");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "TimeEntries");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "TeamMembers");

        migrationBuilder.DropColumn(
            name: "DeletedAt",
            table: "TeamMembers");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "TeamMembers");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "TeamMembers");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "DeletedAt",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Countries");

        migrationBuilder.DropColumn(
            name: "DeletedAt",
            table: "Countries");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Countries");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Countries");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Clients");

        migrationBuilder.DropColumn(
            name: "DeletedAt",
            table: "Clients");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Clients");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Clients");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Categories");

        migrationBuilder.DropColumn(
            name: "DeletedAt",
            table: "Categories");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Categories");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Categories");
    }
}
