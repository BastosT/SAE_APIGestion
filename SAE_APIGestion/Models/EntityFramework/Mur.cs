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
        [Column("mur_nom" , TypeName ="varchar(25)")]
        [StringLength(25)]
        public string Nom { get; set; }    // Nord, Sud, Est, Ouest

        [Required]
        [Column("mur_longueur")]
        public double Longueur { get; set; }

        [Required]
        [Column("mur_hauteur")]
        public double Hauteur { get; set; }

        [Required]
        [Column("mur_type")]
        public TypeMur Type {  get; set; } 

        [Required]
        [Column("sal_id")]
        public int SalleId { get; set; }

        [ForeignKey("SalleId")]
        [InverseProperty(nameof(Salle))]
        public Salle Salle { get; set; }

        [InverseProperty(nameof(Equipement.Mur))]
        public List<Equipement> Equipements { get; set; } = new List<Equipement>();

        [InverseProperty(nameof(Capteur.Mur))]
        public List<Capteur> Capteurs { get; set; } = new List<Capteur>();
    }
    public enum TypeMur
    {
        Face,
        Entree,
        Gauche,
        Droite
    }
}
