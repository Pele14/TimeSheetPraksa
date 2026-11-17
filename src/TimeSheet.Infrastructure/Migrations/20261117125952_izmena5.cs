using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261117125952_izmena5 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "TimeEntries",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "TEXT");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "TimeEntries",
            type: "TEXT",
            maxLength: 200,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "TEXT",
            oldMaxLength: 200);

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "TeamMembers",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "TEXT");

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "Projects",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "TEXT");

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "Countries",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "TEXT");

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "Clients",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "TEXT");

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "Categories",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "TEXT");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "TimeEntries",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "TEXT",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "TimeEntries",
            type: "TEXT",
            maxLength: 200,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "TEXT",
            oldMaxLength: 200,
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "TeamMembers",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "TEXT",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "Projects",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "TEXT",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "Countries",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "TEXT",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "Clients",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "TEXT",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedAt",
            table: "Categories",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "TEXT",
            oldNullable: true);
    }
}
