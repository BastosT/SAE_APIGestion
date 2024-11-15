using Microsoft.EntityFrameworkCore;

namespace SAE_APIGestion.Models.EntityFramework
{
    public class GlobalDBContext : DbContext
    {
        public GlobalDBContext() { }

        public GlobalDBContext(DbContextOptions<GlobalDBContext> options) : base(options) { }

        // DbSet pour chaque entit้
        public virtual DbSet<Batiment> Batiments { get; set; } = null!;
        public virtual DbSet<Salle> Salles { get; set; } = null!;
        public virtual DbSet<Mur> Murs { get; set; } = null!;
        public virtual DbSet<Equipement> Equipements { get; set; } = null!;
        public virtual DbSet<Capteur> Capteurs { get; set; } = null!;
        public virtual DbSet<TypeDonneesCapteur> TypesDonneesCapteurs { get; set; } = null!;
        public virtual DbSet<DonneesCapteur> DonneesCapteurs { get; set; } = null!;
        public virtual DbSet<TypeSalle> TypesSalles { get; set; } = null!;
        public virtual DbSet<TypeEquipement> TypesEquipements { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Supprimez cette logique si vous configurez DbContext dans Program.cs
            if (!optionsBuilder.IsConfigured)
            {
                // Laissez vide si la configuration est faite dans Program.cs
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batiment>(entity =>
            {
                entity.HasKey(e => e.BatimentId).HasName("pk_batiment");
                entity.HasMany(e => e.Salles).WithOne(s => s.BatimentNavigation).HasForeignKey(s => s.BatimentId);
            });

            modelBuilder.Entity<Salle>(entity =>
            {
                entity.HasKey(e => e.SalleId).HasName("pk_salle");

                entity.HasOne(d => d.BatimentNavigation)
                    .WithMany(p => p.Salles)
                    .HasForeignKey(d => d.BatimentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_salle_batiment");

                entity.HasMany(s => s.Murs)
                    .WithOne(m => m.Salle)
                    .HasForeignKey(m => m.SalleId)
                    .HasConstraintName("fk_salle_mur");

                entity.HasMany(s => s.Equipements)
                    .WithOne(e => e.Salle)
                    .HasForeignKey(e => e.SalleId)
                    .HasConstraintName("fk_salle_equipement");

                entity.HasMany(s => s.Capteurs)
                    .WithOne(c => c.Salle)
                    .HasForeignKey(c => c.SalleId)
                    .HasConstraintName("fk_salle_capteur");
            });

            modelBuilder.Entity<Mur>(entity =>
            {
                entity.HasKey(e => e.MurId).HasName("pk_mur");

                entity.HasMany(m => m.Equipements)
                    .WithOne(e => e.Mur)
                    .HasForeignKey(e => e.MurId)
                    .HasConstraintName("fk_mur_equipement");

                entity.HasMany(m => m.Capteurs)
                    .WithOne(c => c.Mur)
                    .HasForeignKey(c => c.MurId)
                    .HasConstraintName("fk_mur_capteur");
            });

            modelBuilder.Entity<Equipement>(entity =>
            {
                entity.HasKey(e => e.EquipementId).HasName("pk_equipement");

                entity.HasOne(e => e.Mur)
                    .WithMany(m => m.Equipements)
                    .HasForeignKey(e => e.MurId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_equipement_mur");

                entity.HasOne(e => e.Salle)
                    .WithMany(s => s.Equipements)
                    .HasForeignKey(e => e.SalleId)
                    .HasConstraintName("fk_equipement_salle");
            });

            modelBuilder.Entity<TypeDonneesCapteur>(entity =>
            {
                entity.HasKey(e => e.TypeDonneesCapteurId).HasName("pk_typecapteur");
            });

            modelBuilder.Entity<Capteur>(entity =>
            {
                entity.HasKey(e => e.CapteurId).HasName("pk_capteur");

                entity.HasMany(c => c.DonneesCapteurs)
                    .WithOne(d => d.Capteur)
                    .HasForeignKey(d => d.CapteurId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_capteur_donneescapteur");

                entity.HasOne(c => c.Salle)
                    .WithMany(s => s.Capteurs)
                    .HasForeignKey(c => c.SalleId)
                    .HasConstraintName("fk_capteur_salle");

                entity.HasOne(c => c.Mur)
                    .WithMany(m => m.Capteurs)
                    .HasForeignKey(c => c.MurId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_capteur_mur");
            });

            modelBuilder.Entity<DonneesCapteur>(entity =>
            {
                entity.HasKey(e => e.DonneesCapteurId).HasName("pk_capacitecapteur");

                entity.HasOne(cc => cc.Capteur)
                    .WithMany(c => c.DonneesCapteurs)
                    .HasForeignKey(cc => cc.CapteurId)
                    .HasConstraintName("fk_capacitecapteur_capteur");

                entity.HasOne(c => c.TypeDonnees)
                    .WithMany(s => s.DonneesCapteurs)
                    .HasForeignKey(c => c.TypeDonneesId)
                    .HasConstraintName("fk_type_donnees");
            });

            modelBuilder.Entity<TypeSalle>(entity =>
            {
                entity.HasKey(e => e.TypeSalleId).HasName("pk_typesalle");
            });

        }
    }
}
