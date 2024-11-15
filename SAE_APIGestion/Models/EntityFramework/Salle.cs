using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("BatimentId")]
    [Table("t_e_salle_sal")]
    public class Salle
    {
        [Key]
        [Column("sal_id")]
        public int Id { get; set; }

        [Column("sal_nom")]
        public string Nom { get; set; }

        [Column("sal_surface")]
        public double Surface { get; set; }

        [ForeignKey("typsal_id")]
        [Column("type")]
        public TypeSalle Type { get; set; }

        [Column("bat_id")]
        public int BatimentId { get; set; }

        [ForeignKey("BatimentId")]
        [InverseProperty(nameof(Batiment.Salles))]
        public Batiment BatimentNavigation { get; set; }

        [InverseProperty(nameof(Mur.Salle))]
        public List<Mur> Murs { get; set; } = new List<Mur>();

        [InverseProperty(nameof(Equipement.Salle))]
        public List<Equipement> Equipements { get; set; } = new List<Equipement>();

        [InverseProperty(nameof(Capteur.Salle))]
        public List<Capteur> Capteurs { get; set; } = new List<Capteur>();
    }
}
