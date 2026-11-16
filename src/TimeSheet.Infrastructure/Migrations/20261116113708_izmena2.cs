using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261116113708_izmena2 : Migration
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

        migrationBuilder.DropForeignKey(
            name: "FK_Projects_TeamMembers_TeamMemberId",
            table: "Projects");

        migrationBuilder.DropForeignKey(
            name: "FK_TimeEntries_Projects_ProjectId",
            table: "TimeEntries");

        migrationBuilder.DropForeignKey(
            name: "FK_TimeEntries_TeamMembers_TeamMemberId",
            table: "TimeEntries");

        migrationBuilder.AlterColumn<double>(
            name: "Overtime",
            table: "TimeEntries",
            type: "REAL",
            nullable: false,
            defaultValue: 0.0,
            oldClrType: typeof(double),
            oldType: "REAL",
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
            oldNullable: true);

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

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_TeamMembers_TeamMemberId",
            table: "Projects",
            column: "TeamMemberId",
            principalTable: "TeamMembers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_TimeEntries_Projects_ProjectId",
            table: "TimeEntries",
            column: "ProjectId",
            principalTable: "Projects",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_TimeEntries_TeamMembers_TeamMemberId",
            table: "TimeEntries",
            column: "TeamMemberId",
            principalTable: "TeamMembers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Clients_Countries_CountryId",
            table: "Clients");

        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects");

        migrationBuilder.DropForeignKey(
            name: "FK_Projects_TeamMembers_TeamMemberId",
            table: "Projects");

        migrationBuilder.DropForeignKey(
            name: "FK_TimeEntries_Projects_ProjectId",
            table: "TimeEntries");

        migrationBuilder.DropForeignKey(
            name: "FK_TimeEntries_TeamMembers_TeamMemberId",
            table: "TimeEntries");

        migrationBuilder.AlterColumn<double>(
            name: "Overtime",
            table: "TimeEntries",
            type: "REAL",
            nullable: true,
            oldClrType: typeof(double),
            oldType: "REAL");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "TimeEntries",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "TEXT",
            oldMaxLength: 200);

        migrationBuilder.AddForeignKey(
            name: "FK_Clients_Countries_CountryId",
            table: "Clients",
            column: "CountryId",
            principalTable: "Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_TeamMembers_TeamMemberId",
            table: "Projects",
            column: "TeamMemberId",
            principalTable: "TeamMembers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_TimeEntries_Projects_ProjectId",
            table: "TimeEntries",
            column: "ProjectId",
            principalTable: "Projects",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_TimeEntries_TeamMembers_TeamMemberId",
            table: "TimeEntries",
            column: "TeamMemberId",
            principalTable: "TeamMembers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
