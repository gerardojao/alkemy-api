using Microsoft.EntityFrameworkCore.Migrations;

namespace alkemyapi.Migrations
{
    public partial class PELICULASSERIE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
               name: "IX_PeliculaSeries_GeneroId",
               table: "PeliculaSeries"
             );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
               name: "IX_PeliculaSeries_GeneroId",
               table: "PeliculaSeries",
                 column: "GeneroId");

        }
    }
}
