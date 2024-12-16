using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_mur_mur")]
    public class Mur
    {
        [Key]
        [Column("mur_id")]
        public int MurId { get; set; }

        [Required]
        [Column("mur_nom", TypeName = "varchar(25)")]
        [StringLength(25)]
        public string Nom { get; set; }

        [Required]
        [Column("mur_longueur")]
        public double Longueur { get; set; }

        [Required]
        [Column("mur_hauteur")]
        public double Hauteur { get; set; }

        [Required]
        [Column("mur_orientation")]
        public Orientation Orientation { get; set; }

        [InverseProperty(nameof(Equipement.Mur))]
        public virtual List<Equipement> Equipements { get; set; } = new List<Equipement>();

        [InverseProperty(nameof(Capteur.Mur))]
        public virtual List<Capteur> Capteurs { get; set; } = new List<Capteur>();

        [Column("sal_id")]
        public int? SalleId { get; set; }

        [ForeignKey("SalleId")]
        public virtual Salle? Salle { get; set; }
    }

    public enum Orientation
    {
        Nord,
        Ouest,
        Sud,
        Est
    }
}