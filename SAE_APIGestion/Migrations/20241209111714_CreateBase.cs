using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAE_APIGestion.Migrations
{
    /// <inheritdoc />
    public partial class CreateBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sal_id",
                table: "t_e_mur_mur",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_mur_mur_sal_id",
                table: "t_e_mur_mur",
                column: "sal_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_e_mur_mur_t_e_salle_sal_sal_id",
                table: "t_e_mur_mur",
                column: "sal_id",
                principalTable: "t_e_salle_sal",
                principalColumn: "sal_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_e_mur_mur_t_e_salle_sal_sal_id",
                table: "t_e_mur_mur");

            migrationBuilder.DropIndex(
                name: "IX_t_e_mur_mur_sal_id",
                table: "t_e_mur_mur");

            migrationBuilder.DropColumn(
                name: "sal_id",
                table: "t_e_mur_mur");
        }
    }
}
