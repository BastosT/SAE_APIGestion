using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_capteur_cap")]
    public class Capteur
    {
        [Key]
        [Column("cap_id")]
        public int CapteurId { get; set; }

        [Column("cap_estactif")]
        public bool EstActif { get; set; }

        [Column("cap_distancefenetre")]
        public double? DistanceFenetre { get; set; }

        [Column("cap_distanceporte")]
        public double? DistancePorte { get; set; }

        [Column("cap_distancechauffage")]
        public double? DistanceChauffage { get; set; }

        [Column("sal_id")]
        public int SalleId { get; set; }

        [Column("mur_id")]
        public int MurId { get; set; }

        [Column("dc_id")]
        public int DonneesCapteurId { get; set; }

        [ForeignKey("MurId")]
        [InverseProperty(nameof(Mur.Capteurs))]
        public Mur Mur { get; set; }

        [ForeignKey("SalleId")]
        [InverseProperty(nameof(Salle.Capteurs))]
        public Salle Salle { get; set; }

        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }

    [Table("t_e_typedonneescapteur_tdc")]
    public class TypeDonneesCapteur
    {
        [Key]
        [Column("tdc_id")]
        public int TypeDonneesCapteurId { get; set; }

        [Column("tdc_nom")]
        [Required]
        public string Nom { get; set; }  // e.g. "Temperature", "CO2"

        [Column("tdc_unite")]
        public string Unite { get; set; } // e.g. "°C", "ppm"

        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }

    [Table("t_e_donneescapteur_dc")]
    public class DonneesCapteur
    {
        [Key]
        [Column("dc_id")]
        public int DonneesCapteurId { get; set; }

        [Column("cap_id")]
        public int CapteurId { get; set; }

        [ForeignKey("CapteurId")]
        public virtual Capteur Capteur { get; set; }

        [Column("tdc_id")]
        public int TypeDonneesId { get; set; }

        [ForeignKey("TypeDonneesId")]
        public virtual TypeDonneesCapteur TypeDonnees { get; set; }

        [Column("dc_valeur")]
        public double Valeur { get; set; }

        [Column("dc_timestamp")]
        public DateTime Timestamp { get; set; }
    }
}