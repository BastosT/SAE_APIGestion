using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("Id")]
    [Table("equipement")]
    public class Equipement
    {
        [Key]
        [Column("idequipement")]
        public int Id { get; set; }

        [Column("nomequipement")]
        public string Nom { get; set; }

        [ForeignKey("TypeEquipementId")]
        [Column("type")]
        public TypeEquipement Type { get; set; }

        [Column("largeur")]
        public double Largeur { get; set; }

        [Column("hauteur")]
        public double Hauteur { get; set; }

        [Column("positionx")]
        public double PositionX { get; set; }    // Position relative sur le mur

        [Column("positiony")]
        public double PositionY { get; set; }

        [Column("murid")]
        public int? MurId { get; set; }          // Nullable car peut être au sol

        [ForeignKey("MurId")]
        [InverseProperty(nameof(Mur.Equipements))]
        public Mur Mur { get; set; }

        [Column("salleid")]
        public int SalleId { get; set; }

        [ForeignKey("SalleId")]
        [InverseProperty(nameof(Salle.Equipements))]
        public Salle Salle { get; set; }
    }
}
