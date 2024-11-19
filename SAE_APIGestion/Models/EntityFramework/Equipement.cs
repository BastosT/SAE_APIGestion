using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_equipement_equ ")]
    public class Equipement
    {
        [Key]
        [Column("equ_id")]
        public int EquipementId { get; set; }

        [Required]
        [Column("equ_nom" , TypeName ="varchar(100)")]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [Column("tye_id")]
        public int TypeEquipementId { get; set; }

        [ForeignKey("TypeEquipementId")]
        public virtual TypeEquipement TypeEquipement { get; set; }

        [Column("equ_largeur")]
        public double Largeur { get; set; }

        [Column("equ_hauteur")]
        public double Hauteur { get; set; }

        [Column("equ_positionx")]
        public double PositionX { get; set; }

        [Column("equ_positiony")]
        public double PositionY { get; set; }

        [Column("mur_id")]
        public int? MurId { get; set; }

        [ForeignKey("MurId")]
        public virtual Mur Mur { get; set; }

        [Column("sal_id")]
        public int SalleId { get; set; }

        [ForeignKey("SalleId")]
        public virtual Salle Salle { get; set; }
    }
}
