using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("SalleId")]
    [Table("t_e_batiment_bat")]
    public class Batiment
    {
        [Key]
        [Column("bat_id")]
        public int BatimentId { get; set; }

        [Column("bat_nom")]
        public string Nom { get; set; }

        [Column("bat_adresse")]
        public string Adresse { get; set; }

        [InverseProperty(nameof(Salle.BatimentNavigation))]
        public List<Salle> Salles { get; set; } = new List<Salle>();
    }
}
