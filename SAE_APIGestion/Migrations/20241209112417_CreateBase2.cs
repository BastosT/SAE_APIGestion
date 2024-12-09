using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAE_APIGestion.Migrations
{
    /// <inheritdoc />
    public partial class CreateBase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_e_mur_mur_t_e_salle_sal_sal_id",
                table: "t_e_mur_mur");

            migrationBuilder.AlterColumn<int>(
                name: "sal_id",
                table: "t_e_mur_mur",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_t_e_mur_mur_t_e_salle_sal_sal_id",
                table: "t_e_mur_mur",
                column: "sal_id",
                principalTable: "t_e_salle_sal",
                principalColumn: "sal_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_e_mur_mur_t_e_salle_sal_sal_id",
                table: "t_e_mur_mur");

            migrationBuilder.AlterColumn<int>(
                name: "sal_id",
                table: "t_e_mur_mur",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_t_e_mur_mur_t_e_salle_sal_sal_id",
                table: "t_e_mur_mur",
                column: "sal_id",
                principalTable: "t_e_salle_sal",
                principalColumn: "sal_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
