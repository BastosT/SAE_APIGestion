using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("SalleId")]
    [Table("t_e_equipement_equ ")]
    public class Equipement
    {
        [Key]
        [Column("equ_id")]
        public int EquipementId { get; set; }

        [Column("equ_nom")]
        public string Nom { get; set; }

        [ForeignKey("typequ_id")]
        [Column("type")]
        public TypeEquipement Type { get; set; }

        [Column("equ_id")]
        public double Largeur { get; set; }

        [Column("equ_hauteur")]
        public double Hauteur { get; set; }

        [Column("equ_positionx")]
        public double PositionX { get; set; }    // Position relative sur le mur

        [Column("equ_positiony")]
        public double PositionY { get; set; }

        [Column("mur_id")]
        public int? MurId { get; set; }          // Nullable car peut être au sol

        [ForeignKey("MurId")]
        [InverseProperty(nameof(Mur.Equipements))]
        public Mur Mur { get; set; }

        [Column("sal_id")]
        public int SalleId { get; set; }

        [ForeignKey("SalleId")]
        [InverseProperty(nameof(Salle.Equipements))]
        public Salle Salle { get; set; }
    }
}
