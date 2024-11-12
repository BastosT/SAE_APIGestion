using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    //[PrimaryKey("Id")]
    [Table("capteur")]
    public class Capteur
    {
        [Key]
        [Column("idcapteur")]
        public int Id { get; set; }

        [Column("nomcapteur")]
        public string Nom { get; set; }

        [Column("idz_wave")]
        public string IdZWave { get; set; }      // Identifiant unique Z-Wave

        [Column("estactif")]
        public bool EstActif { get; set; }       // Actif (actionneur) ou passif

        [ForeignKey("TypeCapteurId")]
        [Column("type")]
        public TypeCapteur Type { get; set; }

        [Column("positionx")]
        public double PositionX { get; set; }

        [Column("positiony")]
        public double PositionY { get; set; }

        [Column("distancefenetre")]
        public double? DistanceFenetre { get; set; }

        [Column("distanceporte")]
        public double? DistancePorte { get; set; }

        [Column("distanceradiateur")]
        public double? DistanceRadiateur { get; set; }

        [Column("murid")]
        public int? MurId { get; set; }          // Nullable car peut être ailleurs dans la pièce

        [ForeignKey("MurId")]
        [InverseProperty(nameof(Mur.Capteurs))]
        public Mur Mur { get; set; }

        [Column("salleid")]
        public int SalleId { get; set; }

        [ForeignKey("SalleId")]
        [InverseProperty(nameof(Salle.Capteurs))]
        public Salle Salle { get; set; }

        [InverseProperty(nameof(CapaciteCapteur.Capteur))]
        public List<CapaciteCapteur> Capacites { get; set; } = new List<CapaciteCapteur>();
    }
}
