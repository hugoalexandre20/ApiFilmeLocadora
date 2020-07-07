using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiLocadora.Migrations
{
    public partial class versao5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Desativado",
                table: "Locacao",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Desativado",
                table: "Filme",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Desativado",
                table: "Cliente",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desativado",
                table: "Locacao");

            migrationBuilder.DropColumn(
                name: "Desativado",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Desativado",
                table: "Cliente");
        }
    }
}
