using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_batiment_bat")]
    public class Batiment
    {
        [Key]
        [Column("bat_id")]
        public int BatimentId { get; set; }

        [Required]
        [Column("bat_nom" , TypeName = "varchar(100)")]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [Column("bat_adresse",TypeName ="varchar(100)")]
        [StringLength(100)]
        public string Adresse { get; set; }

        [InverseProperty(nameof(Salle.Batiment))]
        public List<Salle> Salles { get; set; } = new List<Salle>();
    }
}
