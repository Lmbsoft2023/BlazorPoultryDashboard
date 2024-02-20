using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorPoultryDashboard.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Birds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TracingId = table.Column<Guid>(type: "TEXT", nullable: true),
                    WeightShackleId = table.Column<int>(type: "INTEGER", nullable: false),
                    GradeShackleId = table.Column<int>(type: "INTEGER", nullable: false),
                    DropOffShackleId = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GradeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DropOffDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Grade = table.Column<int>(type: "INTEGER", nullable: false),
                    DropOff = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AverageWeight = table.Column<double>(type: "REAL", nullable: false),
                    MeanGrade = table.Column<double>(type: "REAL", nullable: false),
                    TotalWeightChange = table.Column<double>(type: "REAL", nullable: false),
                    AverageWeightRate = table.Column<double>(type: "REAL", nullable: false),
                    MinWeight = table.Column<double>(type: "REAL", nullable: false),
                    MaxWeight = table.Column<double>(type: "REAL", nullable: false),
                    MedianWeight = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Birds");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
