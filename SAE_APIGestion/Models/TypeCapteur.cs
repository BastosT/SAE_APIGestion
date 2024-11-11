using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models
{
    [PrimaryKey("Id")]
    [Table("mesurecapteur")]
    public class MesureCapteur
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("capteurid")]
        public int CapteurId { get; set; }

        [ForeignKey("CapteurId")]
        [InverseProperty(nameof(Capteur.Mesures))]
        public Capteur Capteur { get; set; }

        [Column("nomcapacite")]
        public string NomCapacite { get; set; }

        [Column("valeur")]
        public double Valeur { get; set; }

        [Column("horodatage")]
        public DateTime Horodatage { get; set; }
    }
}
