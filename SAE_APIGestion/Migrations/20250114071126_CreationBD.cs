using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SAE_APIGestion.Migrations
{
    /// <inheritdoc />
    public partial class CreationBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_e_batiment_bat",
                columns: table => new
                {
                    bat_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bat_nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    bat_adresse = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_batiment", x => x.bat_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_typedonneescapteur_tdc",
                columns: table => new
                {
                    tdc_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tdc_nom = table.Column<string>(type: "text", nullable: false),
                    tdc_unite = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_typecapteur", x => x.tdc_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_typeequipement_tye",
                columns: table => new
                {
                    tye_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tye_nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_typeequipement_tye", x => x.tye_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_typesalle_tys",
                columns: table => new
                {
                    tys_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tys_nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    tys_description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_typesalle", x => x.tys_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_salle_sal",
                columns: table => new
                {
                    sal_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sal_nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    sal_surface = table.Column<double>(type: "double precision", nullable: false),
                    tys_id = table.Column<int>(type: "integer", nullable: false),
                    bat_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salle", x => x.sal_id);
                    table.ForeignKey(
                        name: "FK_t_e_salle_sal_t_e_batiment_bat_bat_id",
                        column: x => x.bat_id,
                        principalTable: "t_e_batiment_bat",
                        principalColumn: "bat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_salle_sal_t_e_typesalle_tys_tys_id",
                        column: x => x.tys_id,
                        principalTable: "t_e_typesalle_tys",
                        principalColumn: "tys_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_mur_mur",
                columns: table => new
                {
                    mur_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mur_nom = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    mur_longueur = table.Column<double>(type: "double precision", nullable: false),
                    mur_hauteur = table.Column<double>(type: "double precision", nullable: false),
                    mur_orientation = table.Column<int>(type: "integer", nullable: false),
                    sal_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mur", x => x.mur_id);
                    table.ForeignKey(
                        name: "fk_salle_mur",
                        column: x => x.sal_id,
                        principalTable: "t_e_salle_sal",
                        principalColumn: "sal_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_e_capteur_cap",
                columns: table => new
                {
                    cap_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cap_nom = table.Column<string>(type: "text", nullable: false),
                    cap_estactif = table.Column<bool>(type: "boolean", nullable: false),
                    cap_distancefenetre = table.Column<double>(type: "double precision", nullable: true),
                    cap_longueur = table.Column<double>(type: "double precision", nullable: true),
                    cap_hauteur = table.Column<double>(type: "double precision", nullable: true),
                    cap_positionx = table.Column<double>(type: "double precision", nullable: false),
                    cap_positiony = table.Column<double>(type: "double precision", nullable: false),
                    cap_distanceporte = table.Column<double>(type: "double precision", nullable: true),
                    cap_distancechauffage = table.Column<double>(type: "double precision", nullable: true),
                    sal_id = table.Column<int>(type: "integer", nullable: true),
                    mur_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_capteur", x => x.cap_id);
                    table.ForeignKey(
                        name: "FK_t_e_capteur_cap_t_e_mur_mur_mur_id",
                        column: x => x.mur_id,
                        principalTable: "t_e_mur_mur",
                        principalColumn: "mur_id");
                    table.ForeignKey(
                        name: "fk_capteur_salle",
                        column: x => x.sal_id,
                        principalTable: "t_e_salle_sal",
                        principalColumn: "sal_id");
                });

            migrationBuilder.CreateTable(
                name: "t_e_equipement_equ",
                columns: table => new
                {
                    equ_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equ_nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    equ_hauteur = table.Column<double>(type: "double precision", nullable: false),
                    equ_longueur = table.Column<double>(type: "double precision", nullable: false),
                    equ_positionx = table.Column<double>(type: "double precision", nullable: false),
                    equ_positiony = table.Column<double>(type: "double precision", nullable: false),
                    tye_id = table.Column<int>(type: "integer", nullable: true),
                    sal_id = table.Column<int>(type: "integer", nullable: true),
                    mur_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipement", x => x.equ_id);
                    table.ForeignKey(
                        name: "FK_t_e_equipement_equ_t_e_typeequipement_tye_tye_id",
                        column: x => x.tye_id,
                        principalTable: "t_e_typeequipement_tye",
                        principalColumn: "tye_id");
                    table.ForeignKey(
                        name: "fk_equipement_mur",
                        column: x => x.mur_id,
                        principalTable: "t_e_mur_mur",
                        principalColumn: "mur_id");
                    table.ForeignKey(
                        name: "fk_equipement_salle",
                        column: x => x.sal_id,
                        principalTable: "t_e_salle_sal",
                        principalColumn: "sal_id");
                });

            migrationBuilder.CreateTable(
                name: "t_e_donneescapteur_dcp",
                columns: table => new
                {
                    dcp_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dcp_valeur = table.Column<double>(type: "double precision", nullable: false),
                    dcp_timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cap_id = table.Column<int>(type: "integer", nullable: false),
                    tdc_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_donnees_capteur", x => x.dcp_id);
                    table.ForeignKey(
                        name: "fk_donnees_capteur",
                        column: x => x.cap_id,
                        principalTable: "t_e_capteur_cap",
                        principalColumn: "cap_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_type_donnees",
                        column: x => x.tdc_id,
                        principalTable: "t_e_typedonneescapteur_tdc",
                        principalColumn: "tdc_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_e_capteur_cap_mur_id",
                table: "t_e_capteur_cap",
                column: "mur_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_capteur_cap_sal_id",
                table: "t_e_capteur_cap",
                column: "sal_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_donneescapteur_dcp_cap_id",
                table: "t_e_donneescapteur_dcp",
                column: "cap_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_donneescapteur_dcp_tdc_id",
                table: "t_e_donneescapteur_dcp",
                column: "tdc_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_equipement_equ_mur_id",
                table: "t_e_equipement_equ",
                column: "mur_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_equipement_equ_sal_id",
                table: "t_e_equipement_equ",
                column: "sal_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_equipement_equ_tye_id",
                table: "t_e_equipement_equ",
                column: "tye_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_mur_mur_sal_id",
                table: "t_e_mur_mur",
                column: "sal_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_salle_sal_bat_id",
                table: "t_e_salle_sal",
                column: "bat_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_salle_sal_tys_id",
                table: "t_e_salle_sal",
                column: "tys_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_donneescapteur_dcp");

            migrationBuilder.DropTable(
                name: "t_e_equipement_equ");

            migrationBuilder.DropTable(
                name: "t_e_capteur_cap");

            migrationBuilder.DropTable(
                name: "t_e_typedonneescapteur_tdc");

            migrationBuilder.DropTable(
                name: "t_e_typeequipement_tye");

            migrationBuilder.DropTable(
                name: "t_e_mur_mur");

            migrationBuilder.DropTable(
                name: "t_e_salle_sal");

            migrationBuilder.DropTable(
                name: "t_e_batiment_bat");

            migrationBuilder.DropTable(
                name: "t_e_typesalle_tys");
        }
    }
}
