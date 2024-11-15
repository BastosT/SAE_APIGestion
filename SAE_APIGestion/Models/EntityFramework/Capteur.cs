using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("capteur")]
    public class Capteur
    {
        [Column("estactif")]
        public bool EstActif { get; set; }

        [Column("distancefenetre")]
        public double? DistanceFenetre { get; set; }

        [Column("distanceporte")]
        public double? DistancePorte { get; set; }

        [Column("distancechauffage")]
        public double? DistanceChauffage { get; set; }

        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }

    [Table("typedonneescapteur")]
    public class TypeDonneesCapteur
    {
        [Key]
        [Column("idtypedonneescapteur")]
        public int Id { get; set; }

        [Column("nom")]
        [Required]
        public string Nom { get; set; }  // e.g. "Temperature", "CO2"

        [Column("unite")]
        public string Unite { get; set; } // e.g. "°C", "ppm"

        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }

    [Table("donneescapteur")]
    public class DonneesCapteur
    {
        [Key]
        [Column("iddonneescapteur")]
        public int Id { get; set; }

        [Column("capteurid")]
        public int CapteurId { get; set; }
        [ForeignKey("CapteurId")]
        public virtual Capteur Capteur { get; set; }

        [Column("typedonneesid")]
        public int TypeDonneesId { get; set; }
        [ForeignKey("TypeDonneesId")]
        public virtual TypeDonneesCapteur TypeDonnees { get; set; }

        [Column("valeur")]
        public double Valeur { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}