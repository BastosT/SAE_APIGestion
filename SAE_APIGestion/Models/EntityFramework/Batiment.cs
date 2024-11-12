using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("Id")]
    [Table("batiment")]
    public class Batiment
    {
        [Key]
        [Column("idbatiment")]
        public int Id { get; set; }

        [Column("nombatiment")]
        public string Nom { get; set; }

        [Column("adressebatiment")]
        public string Adresse { get; set; }

        [InverseProperty(nameof(Salle.BatimentNavigation))]
        public List<Salle> Salles { get; set; } = new List<Salle>();
    }
}
