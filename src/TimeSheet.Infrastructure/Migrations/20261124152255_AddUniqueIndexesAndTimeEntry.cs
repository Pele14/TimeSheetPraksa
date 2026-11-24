using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261124152255_AddUniqueIndexesAndTimeEntry : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects");

        migrationBuilder.DropForeignKey(
            name: "FK_TimeEntries_Categories_CategoryId",
            table: "TimeEntries");

        migrationBuilder.DropIndex(
            name: "IX_TimeEntries_TeamMemberId",
            table: "TimeEntries");

        migrationBuilder.RenameColumn(
            name: "Time",
            table: "TimeEntries",
            newName: "Hours");

        migrationBuilder.AlterColumn<string>(
            name: "Status",
            table: "TeamMembers",
            type: "TEXT",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "INTEGER");

        migrationBuilder.AlterColumn<string>(
            name: "Role",
            table: "TeamMembers",
            type: "TEXT",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "INTEGER");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Projects",
            type: "TEXT",
            maxLength: 500,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "TEXT",
            oldMaxLength: 500);

        migrationBuilder.CreateIndex(
            name: "IX_TimeEntries_TeamMemberId_Date",
            table: "TimeEntries",
            columns: new[] { "TeamMemberId", "Date" });

        migrationBuilder.CreateIndex(
            name: "IX_TeamMembers_Email",
            table: "TeamMembers",
            column: "Email",
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_TeamMembers_Username",
            table: "TeamMembers",
            column: "Username",
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Projects_Name",
            table: "Projects",
            column: "Name",
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Clients_Name",
            table: "Clients",
            column: "Name",
            unique: true,
            filter: "IsDeleted = 0");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_TimeEntries_Categories_CategoryId",
            table: "TimeEntries",
            column: "CategoryId",
            principalTable: "Categories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects");

        migrationBuilder.DropForeignKey(
            name: "FK_TimeEntries_Categories_CategoryId",
            table: "TimeEntries");

        migrationBuilder.DropIndex(
            name: "IX_TimeEntries_TeamMemberId_Date",
            table: "TimeEntries");

        migrationBuilder.DropIndex(
            name: "IX_TeamMembers_Email",
            table: "TeamMembers");

        migrationBuilder.DropIndex(
            name: "IX_TeamMembers_Username",
            table: "TeamMembers");

        migrationBuilder.DropIndex(
            name: "IX_Projects_Name",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Clients_Name",
            table: "Clients");

        migrationBuilder.RenameColumn(
            name: "Hours",
            table: "TimeEntries",
            newName: "Time");

        migrationBuilder.AlterColumn<int>(
            name: "Status",
            table: "TeamMembers",
            type: "INTEGER",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "TEXT");

        migrationBuilder.AlterColumn<int>(
            name: "Role",
            table: "TeamMembers",
            type: "INTEGER",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "TEXT");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Projects",
            type: "TEXT",
            maxLength: 500,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "TEXT",
            oldMaxLength: 500,
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_TimeEntries_TeamMemberId",
            table: "TimeEntries",
            column: "TeamMemberId");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Clients_ClientId",
            table: "Projects",
            column: "ClientId",
            principalTable: "Clients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_TimeEntries_Categories_CategoryId",
            table: "TimeEntries",
            column: "CategoryId",
            principalTable: "Categories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
