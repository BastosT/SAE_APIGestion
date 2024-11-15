using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("EquipementId")]
    [Table("t_e_mur_mur")]
    public class Mur
    {
        [Key]
        [Column("mur_id")]
        public int MurId { get; set; }

        [Column("mur_nom")]
        public string Nom { get; set; }    // Nord, Sud, Est, Ouest

        [Column("mur_longueur")]
        public double Longueur { get; set; }

        [Column("mur_hauteur")]
        public double Hauteur { get; set; }

        [Column("sal_id")]
        public int SalleId { get; set; }

        [ForeignKey("SalleId")]
        [InverseProperty(nameof(Salle.Murs))]
        public Salle Salle { get; set; }

        [InverseProperty(nameof(Equipement.Mur))]
        public List<Equipement> Equipements { get; set; } = new List<Equipement>();

        [InverseProperty(nameof(Capteur.Mur))]
        public List<Capteur> Capteurs { get; set; } = new List<Capteur>();
    }
}
