using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models
{
    [PrimaryKey("Id")]
    [Table("salle")]
    public class Salle
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string Nom { get; set; }

        [Column("surface")]
        public double Surface { get; set; }

        [ForeignKey("TypeSalleId")]
        [Column("type")]
        public TypeSalle Type { get; set; }

        [Column("batimentid")]
        public int BatimentId { get; set; }

        [ForeignKey("BatimentId")]
        [InverseProperty(nameof(Batiment.Salles))]
        public Batiment Batiment { get; set; }

        [InverseProperty(nameof(Mur.Salle))]
        public List<Mur> Murs { get; set; } = new List<Mur>();

        [InverseProperty(nameof(Equipement.Salle))]
        public List<Equipement> Equipements { get; set; } = new List<Equipement>();

        [InverseProperty(nameof(Capteur.Salle))]
        public List<Capteur> Capteurs { get; set; } = new List<Capteur>();
    }
}
