using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("capacite_capteur")]
    public class CapaciteCapteur
    {
        [Key]
        [Column("idcapacitecapteur")]
        public int Id { get; set; }

        [Column("nomcapacitecapteur")]
        public string? Nom { get; set; }          // ex: "température", "humidité"

        [Column("unitecapacitecapteur")]
        public string? Unite { get; set; }        // ex: "°C", "%"

        [Column("valeur_min")]
        public double ValeurMin { get; set; }

        [Column("valeur_max")]
        public double ValeurMax { get; set; }

        [Column("id_capteur")]
        public int CapteurId { get; set; }

        [ForeignKey(nameof(CapteurId))]
        public Capteur? Capteur { get; set; }
    }
}
