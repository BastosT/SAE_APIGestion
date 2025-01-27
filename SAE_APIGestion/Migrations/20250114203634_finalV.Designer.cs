﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SAE_APIGestion.Models.EntityFramework;

#nullable disable

namespace SAE_APIGestion.Migrations
{
    [DbContext(typeof(GlobalDBContext))]
    [Migration("20250114203634_finalV")]
    partial class finalV
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Batiment", b =>
                {
                    b.Property<int>("BatimentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("bat_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BatimentId"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("bat_adresse");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("bat_nom");

                    b.HasKey("BatimentId")
                        .HasName("pk_batiment");

                    b.ToTable("t_e_batiment_bat", (string)null);
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Capteur", b =>
                {
                    b.Property<int>("CapteurId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("cap_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CapteurId"));

                    b.Property<double?>("DistanceChauffage")
                        .HasColumnType("double precision")
                        .HasColumnName("cap_distancechauffage");

                    b.Property<double?>("DistanceFenetre")
                        .HasColumnType("double precision")
                        .HasColumnName("cap_distancefenetre");

                    b.Property<double?>("DistancePorte")
                        .HasColumnType("double precision")
                        .HasColumnName("cap_distanceporte");

                    b.Property<bool>("EstActif")
                        .HasColumnType("boolean")
                        .HasColumnName("cap_estactif");

                    b.Property<double?>("Hauteur")
                        .HasColumnType("double precision")
                        .HasColumnName("cap_hauteur");

                    b.Property<double?>("Longueur")
                        .HasColumnType("double precision")
                        .HasColumnName("cap_longueur");

                    b.Property<int?>("MurId")
                        .HasColumnType("integer")
                        .HasColumnName("mur_id");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("cap_nom");

                    b.Property<double>("PositionX")
                        .HasColumnType("double precision")
                        .HasColumnName("cap_positionx");

                    b.Property<double>("PositionY")
                        .HasColumnType("double precision")
                        .HasColumnName("cap_positiony");

                    b.Property<int?>("SalleId")
                        .HasColumnType("integer")
                        .HasColumnName("sal_id");

                    b.HasKey("CapteurId")
                        .HasName("pk_capteur");

                    b.HasIndex("MurId");

                    b.HasIndex("SalleId");

                    b.ToTable("t_e_capteur_cap", (string)null);
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.DonneesCapteur", b =>
                {
                    b.Property<int>("DonneesCapteurId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("dcp_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DonneesCapteurId"));

                    b.Property<int>("CapteurId")
                        .HasColumnType("integer")
                        .HasColumnName("cap_id");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("dcp_timestamp");

                    b.Property<int>("TypeDonneesId")
                        .HasColumnType("integer")
                        .HasColumnName("tdc_id");

                    b.Property<double>("Valeur")
                        .HasColumnType("double precision")
                        .HasColumnName("dcp_valeur");

                    b.HasKey("DonneesCapteurId")
                        .HasName("pk_donnees_capteur");

                    b.HasIndex("CapteurId");

                    b.HasIndex("TypeDonneesId");

                    b.ToTable("t_e_donneescapteur_dcp", (string)null);
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Equipement", b =>
                {
                    b.Property<int>("EquipementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("equ_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EquipementId"));

                    b.Property<double>("Hauteur")
                        .HasColumnType("double precision")
                        .HasColumnName("equ_hauteur");

                    b.Property<double>("Longueur")
                        .HasColumnType("double precision")
                        .HasColumnName("equ_longueur");

                    b.Property<int?>("MurId")
                        .HasColumnType("integer")
                        .HasColumnName("mur_id");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("equ_nom");

                    b.Property<double>("PositionX")
                        .HasColumnType("double precision")
                        .HasColumnName("equ_positionx");

                    b.Property<double>("PositionY")
                        .HasColumnType("double precision")
                        .HasColumnName("equ_positiony");

                    b.Property<int?>("SalleId")
                        .HasColumnType("integer")
                        .HasColumnName("sal_id");

                    b.Property<int?>("TypeEquipementId")
                        .HasColumnType("integer")
                        .HasColumnName("tye_id");

                    b.HasKey("EquipementId")
                        .HasName("pk_equipement");

                    b.HasIndex("MurId");

                    b.HasIndex("SalleId");

                    b.HasIndex("TypeEquipementId");

                    b.ToTable("t_e_equipement_equ", (string)null);
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Mur", b =>
                {
                    b.Property<int>("MurId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("mur_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MurId"));

                    b.Property<double>("Hauteur")
                        .HasColumnType("double precision")
                        .HasColumnName("mur_hauteur");

                    b.Property<double>("Longueur")
                        .HasColumnType("double precision")
                        .HasColumnName("mur_longueur");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)")
                        .HasColumnName("mur_nom");

                    b.Property<int>("Orientation")
                        .HasColumnType("integer")
                        .HasColumnName("mur_orientation");

                    b.Property<int?>("SalleId")
                        .HasColumnType("integer")
                        .HasColumnName("sal_id");

                    b.HasKey("MurId")
                        .HasName("pk_mur");

                    b.HasIndex("SalleId");

                    b.ToTable("t_e_mur_mur", (string)null);
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Salle", b =>
                {
                    b.Property<int>("SalleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("sal_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SalleId"));

                    b.Property<int>("BatimentId")
                        .HasColumnType("integer")
                        .HasColumnName("bat_id");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("sal_nom");

                    b.Property<double>("Surface")
                        .HasColumnType("double precision")
                        .HasColumnName("sal_surface");

                    b.Property<int>("TypeSalleId")
                        .HasColumnType("integer")
                        .HasColumnName("tys_id");

                    b.HasKey("SalleId")
                        .HasName("pk_salle");

                    b.HasIndex("BatimentId");

                    b.HasIndex("TypeSalleId");

                    b.ToTable("t_e_salle_sal", (string)null);
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.TypeDonneesCapteur", b =>
                {
                    b.Property<int>("TypeDonneesCapteurId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("tdc_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TypeDonneesCapteurId"));

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("tdc_nom");

                    b.Property<string>("Unite")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("tdc_unite");

                    b.HasKey("TypeDonneesCapteurId")
                        .HasName("pk_typecapteur");

                    b.ToTable("t_e_typedonneescapteur_tdc", (string)null);
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.TypeEquipement", b =>
                {
                    b.Property<int>("TypeEquipementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("tye_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TypeEquipementId"));

                    b.Property<string>("Couleur")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("varchar(7)")
                        .HasColumnName("tye_couleur");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("tye_nom");

                    b.HasKey("TypeEquipementId");

                    b.ToTable("t_e_typeequipement_tye");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.TypeSalle", b =>
                {
                    b.Property<int>("TypeSalleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("tys_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TypeSalleId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("tys_description");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("tys_nom");

                    b.HasKey("TypeSalleId")
                        .HasName("pk_typesalle");

                    b.ToTable("t_e_typesalle_tys", (string)null);
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Capteur", b =>
                {
                    b.HasOne("SAE_APIGestion.Models.EntityFramework.Mur", "Mur")
                        .WithMany("Capteurs")
                        .HasForeignKey("MurId");

                    b.HasOne("SAE_APIGestion.Models.EntityFramework.Salle", "Salle")
                        .WithMany("Capteurs")
                        .HasForeignKey("SalleId")
                        .HasConstraintName("fk_capteur_salle");

                    b.Navigation("Mur");

                    b.Navigation("Salle");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.DonneesCapteur", b =>
                {
                    b.HasOne("SAE_APIGestion.Models.EntityFramework.Capteur", "Capteur")
                        .WithMany("DonneesCapteurs")
                        .HasForeignKey("CapteurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_donnees_capteur");

                    b.HasOne("SAE_APIGestion.Models.EntityFramework.TypeDonneesCapteur", "TypeDonnees")
                        .WithMany("DonneesCapteurs")
                        .HasForeignKey("TypeDonneesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_type_donnees");

                    b.Navigation("Capteur");

                    b.Navigation("TypeDonnees");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Equipement", b =>
                {
                    b.HasOne("SAE_APIGestion.Models.EntityFramework.Mur", "Mur")
                        .WithMany("Equipements")
                        .HasForeignKey("MurId")
                        .HasConstraintName("fk_equipement_mur");

                    b.HasOne("SAE_APIGestion.Models.EntityFramework.Salle", "Salle")
                        .WithMany("Equipements")
                        .HasForeignKey("SalleId")
                        .HasConstraintName("fk_equipement_salle");

                    b.HasOne("SAE_APIGestion.Models.EntityFramework.TypeEquipement", "TypeEquipement")
                        .WithMany("Equipements")
                        .HasForeignKey("TypeEquipementId");

                    b.Navigation("Mur");

                    b.Navigation("Salle");

                    b.Navigation("TypeEquipement");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Mur", b =>
                {
                    b.HasOne("SAE_APIGestion.Models.EntityFramework.Salle", "Salle")
                        .WithMany("Murs")
                        .HasForeignKey("SalleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_salle_mur");

                    b.Navigation("Salle");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Salle", b =>
                {
                    b.HasOne("SAE_APIGestion.Models.EntityFramework.Batiment", "Batiment")
                        .WithMany("Salles")
                        .HasForeignKey("BatimentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SAE_APIGestion.Models.EntityFramework.TypeSalle", "TypeSalle")
                        .WithMany("Salles")
                        .HasForeignKey("TypeSalleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Batiment");

                    b.Navigation("TypeSalle");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Batiment", b =>
                {
                    b.Navigation("Salles");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Capteur", b =>
                {
                    b.Navigation("DonneesCapteurs");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Mur", b =>
                {
                    b.Navigation("Capteurs");

                    b.Navigation("Equipements");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.Salle", b =>
                {
                    b.Navigation("Capteurs");

                    b.Navigation("Equipements");

                    b.Navigation("Murs");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.TypeDonneesCapteur", b =>
                {
                    b.Navigation("DonneesCapteurs");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.TypeEquipement", b =>
                {
                    b.Navigation("Equipements");
                });

            modelBuilder.Entity("SAE_APIGestion.Models.EntityFramework.TypeSalle", b =>
                {
                    b.Navigation("Salles");
                });
#pragma warning restore 612, 618
        }
    }
}
