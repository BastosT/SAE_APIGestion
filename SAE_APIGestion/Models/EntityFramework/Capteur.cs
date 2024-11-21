using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_capteur_cap")]
    public class Capteur
    {
        public Capteur()
        {
            DonneesCapteurs = new HashSet<DonneesCapteur>();
        }

        [Key]
        [Column("cap_id")]
        public int CapteurId { get; set; }
       
        [Required]
        [Column("cap_estactif")]
        public bool EstActif { get; set; }
       
        [Column("cap_distancefenetre")]
        public double? DistanceFenetre { get; set; }

        [Required]
        [Column("cap_longueur")]
        public double? Longeur { get; set; }

        [Required]
        [Column("cap_largeur")]
        public double? Largeur { get; set; }

        [Column("cap_distanceporte")]
        public double? DistancePorte { get; set; }
        
        [Column("cap_distancechauffage")]
        public double? DistanceChauffage { get; set; }

        [Column("sal_id")]
        public int SalleId { get; set; }
        
        [Column("mur_id")]
        public int? MurId { get; set; }

        [ForeignKey("MurId")]
        public virtual Mur Mur { get; set; }
        [ForeignKey("SalleId")]
        public virtual Salle Salle { get; set; }
        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }
}