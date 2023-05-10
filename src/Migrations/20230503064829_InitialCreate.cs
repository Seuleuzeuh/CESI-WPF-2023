using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CESI_WPF_2023.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dresseurs",
                columns: table => new
                {
                    DresseurId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dresseurs", x => x.DresseurId);
                });

            migrationBuilder.CreateTable(
                name: "PokemonDatas",
                columns: table => new
                {
                    PokemonDataId = table.Column<int>(type: "INTEGER", nullable: false),
                    Commentaire = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    DresseurId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonDatas", x => x.PokemonDataId);
                    table.ForeignKey(
                        name: "FK_PokemonDatas_Dresseurs_DresseurId",
                        column: x => x.DresseurId,
                        principalTable: "Dresseurs",
                        principalColumn: "DresseurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PokemonDatas_DresseurId",
                table: "PokemonDatas",
                column: "DresseurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonDatas");

            migrationBuilder.DropTable(
                name: "Dresseurs");
        }
    }
}
