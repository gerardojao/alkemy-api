using Microsoft.EntityFrameworkCore.Migrations;

namespace alkemyapi.Migrations
{
    public partial class PELICULASSERIES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PeliculaSeries_GeneroId",
                table: "PeliculaSeries",
                column: "GeneroId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PeliculaSeries_GeneroId",
                table: "PeliculaSeries");
        }
    }
}
