using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAE_APIGestion.Migrations
{
    /// <inheritdoc />
    public partial class CreationBD2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_e_mur_mur_t_e_salle_sal_sal_id",
                table: "t_e_mur_mur");

            migrationBuilder.DropForeignKey(
                name: "fk_salle_murdroite",
                table: "t_e_salle_sal");

            migrationBuilder.DropForeignKey(
                name: "fk_salle_murentree",
                table: "t_e_salle_sal");

            migrationBuilder.DropForeignKey(
                name: "fk_salle_murface",
                table: "t_e_salle_sal");

            migrationBuilder.DropForeignKey(
                name: "fk_salle_murgauche",
                table: "t_e_salle_sal");

            migrationBuilder.DropIndex(
                name: "IX_t_e_salle_sal_mur_droiteid",
                table: "t_e_salle_sal");

            migrationBuilder.DropIndex(
                name: "IX_t_e_salle_sal_mur_entreeid",
                table: "t_e_salle_sal");

            migrationBuilder.DropIndex(
                name: "IX_t_e_salle_sal_mur_faceid",
                table: "t_e_salle_sal");

            migrationBuilder.DropIndex(
                name: "IX_t_e_salle_sal_mur_gaucheid",
                table: "t_e_salle_sal");

            migrationBuilder.DropColumn(
                name: "mur_droiteid",
                table: "t_e_salle_sal");

            migrationBuilder.DropColumn(
                name: "mur_entreeid",
                table: "t_e_salle_sal");

            migrationBuilder.DropColumn(
                name: "mur_faceid",
                table: "t_e_salle_sal");

            migrationBuilder.DropColumn(
                name: "mur_gaucheid",
                table: "t_e_salle_sal");

            migrationBuilder.RenameColumn(
                name: "mur_type",
                table: "t_e_mur_mur",
                newName: "mur_orientation");

            migrationBuilder.AddForeignKey(
                name: "fk_salle_mur",
                table: "t_e_mur_mur",
                column: "sal_id",
                principalTable: "t_e_salle_sal",
                principalColumn: "sal_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_salle_mur",
                table: "t_e_mur_mur");

            migrationBuilder.RenameColumn(
                name: "mur_orientation",
                table: "t_e_mur_mur",
                newName: "mur_type");

            migrationBuilder.AddColumn<int>(
                name: "mur_droiteid",
                table: "t_e_salle_sal",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mur_entreeid",
                table: "t_e_salle_sal",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mur_faceid",
                table: "t_e_salle_sal",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mur_gaucheid",
                table: "t_e_salle_sal",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_salle_sal_mur_droiteid",
                table: "t_e_salle_sal",
                column: "mur_droiteid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_salle_sal_mur_entreeid",
                table: "t_e_salle_sal",
                column: "mur_entreeid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_salle_sal_mur_faceid",
                table: "t_e_salle_sal",
                column: "mur_faceid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_salle_sal_mur_gaucheid",
                table: "t_e_salle_sal",
                column: "mur_gaucheid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_t_e_mur_mur_t_e_salle_sal_sal_id",
                table: "t_e_mur_mur",
                column: "sal_id",
                principalTable: "t_e_salle_sal",
                principalColumn: "sal_id");

            migrationBuilder.AddForeignKey(
                name: "fk_salle_murdroite",
                table: "t_e_salle_sal",
                column: "mur_droiteid",
                principalTable: "t_e_mur_mur",
                principalColumn: "mur_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_salle_murentree",
                table: "t_e_salle_sal",
                column: "mur_entreeid",
                principalTable: "t_e_mur_mur",
                principalColumn: "mur_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_salle_murface",
                table: "t_e_salle_sal",
                column: "mur_faceid",
                principalTable: "t_e_mur_mur",
                principalColumn: "mur_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_salle_murgauche",
                table: "t_e_salle_sal",
                column: "mur_gaucheid",
                principalTable: "t_e_mur_mur",
                principalColumn: "mur_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
