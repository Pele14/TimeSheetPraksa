using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheet.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261125085211_MakeOvertimeNullable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            name: "Overtime",
            table: "TimeEntries",
            type: "REAL",
            nullable: true,
            oldClrType: typeof(double),
            oldType: "REAL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            name: "Overtime",
            table: "TimeEntries",
            type: "REAL",
            nullable: false,
            defaultValue: 0.0,
            oldClrType: typeof(double),
            oldType: "REAL",
            oldNullable: true);
    }
}
