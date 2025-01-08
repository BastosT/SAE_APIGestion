using Microsoft.EntityFrameworkCore;

namespace SAE_APIGestion.Models.EntityFramework
{
    public partial class GlobalDBContext : DbContext
    {
        public GlobalDBContext() { }
        public GlobalDBContext(DbContextOptions<GlobalDBContext> options) : base(options) { }

        public virtual DbSet<Batiment> Batiments { get; set; } = null!;
        public virtual DbSet<Salle> Salles { get; set; } = null!;
        public virtual DbSet<Mur> Murs { get; set; } = null!;
        public virtual DbSet<Equipement> Equipements { get; set; } = null!;
        public virtual DbSet<Capteur> Capteurs { get; set; } = null!;
        public virtual DbSet<TypeDonneesCapteur> TypesDonneesCapteurs { get; set; } = null!;
        public virtual DbSet<DonneesCapteur> DonneesCapteurs { get; set; } = null!;
        public virtual DbSet<TypeSalle> TypesSalles { get; set; } = null!;
        public virtual DbSet<TypeEquipement> TypesEquipements { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Batiment>(entity =>
            {
                entity.ToTable("t_e_batiment_bat");
                entity.HasKey(e => e.BatimentId).HasName("pk_batiment");
                entity.HasMany(e => e.Salles)
                    .WithOne(s => s.Batiment)
                    .HasForeignKey(s => s.BatimentId);
            });

            modelBuilder.Entity<Salle>(entity =>
            {
                entity.ToTable("t_e_salle_sal");
                entity.HasKey(e => e.SalleId).HasName("pk_salle");

                // Relation avec les murs
                entity.HasMany(s => s.Murs)
                    .WithOne(m => m.Salle)
                    .HasForeignKey(m => m.SalleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_salle_mur");
            });

            modelBuilder.Entity<Mur>(entity =>
            {
                entity.ToTable("t_e_mur_mur");
                entity.HasKey(e => e.MurId).HasName("pk_mur");

                // Relations avec les équipements
                entity.HasMany(m => m.Equipements)
                    .WithOne(e => e.Mur)
                    .HasForeignKey(e => e.MurId)
                    .HasConstraintName("fk_mur_equipement");

                // Relations avec les capteurs
                entity.HasMany(m => m.Capteurs)
                    .WithOne(c => c.Mur)
                    .HasForeignKey(c => c.MurId)
                    .HasConstraintName("fk_mur_capteur");
            });

            modelBuilder.Entity<Equipement>(entity =>
            {
                entity.ToTable("t_e_equipement_equ");
                entity.HasKey(e => e.EquipementId).HasName("pk_equipement");

                entity.HasOne(e => e.TypeEquipement)
                    .WithMany(t => t.Equipements)
                    .HasForeignKey(e => e.TypeEquipementId);

                entity.HasOne(e => e.Mur)
                    .WithMany(m => m.Equipements)
                    .HasForeignKey(e => e.MurId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_equipement_mur");

                // Nouvelle relation avec la salle
                entity.HasOne(c => c.Salle)
                            .WithMany()
                            .HasForeignKey(c => c.SalleId)
                            .OnDelete(DeleteBehavior.SetNull)
                            .HasConstraintName("fk_equipement_salle");

            });

            modelBuilder.Entity<Capteur>(entity =>
            {
                entity.ToTable("t_e_capteur_cap");
                entity.HasKey(e => e.CapteurId).HasName("pk_capteur");


                entity.HasOne(c => c.Mur)
                    .WithMany(m => m.Capteurs)
                    .HasForeignKey(c => c.MurId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_capteur_mur");

                // Nouvelle relation avec la salle
                entity.HasOne(c => c.Salle)
                            .WithMany()
                            .HasForeignKey(c => c.SalleId)
                            .OnDelete(DeleteBehavior.SetNull)
                            .HasConstraintName("fk_capteur_salle");
            });

            modelBuilder.Entity<TypeDonneesCapteur>(entity =>
            {
                entity.ToTable("t_e_typedonneescapteur_tdc");
                entity.HasKey(e => e.TypeDonneesCapteurId).HasName("pk_typecapteur");
            });

            modelBuilder.Entity<DonneesCapteur>(entity =>
            {
                entity.ToTable("t_e_donneescapteur_dcp");
                entity.HasKey(e => e.DonneesCapteurId).HasName("pk_donnees_capteur");

                entity.HasOne(d => d.Capteur)
                    .WithMany(c => c.DonneesCapteurs)
                    .HasForeignKey(d => d.CapteurId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_donnees_capteur");

                entity.HasOne(d => d.TypeDonnees)
                    .WithMany(t => t.DonneesCapteurs)
                    .HasForeignKey(d => d.TypeDonneesId)
                    .HasConstraintName("fk_type_donnees");
            });

            modelBuilder.Entity<TypeSalle>(entity =>
            {
                entity.ToTable("t_e_typesalle_tys");
                entity.HasKey(e => e.TypeSalleId).HasName("pk_typesalle");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}