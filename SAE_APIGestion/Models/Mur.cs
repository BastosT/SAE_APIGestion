using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models
{
    [PrimaryKey("Id")]
    [Table("mur")]
    public class Mur
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string Nom { get; set; }    // Nord, Sud, Est, Ouest

        [Column("longueur")]
        public double Longueur { get; set; }

        [Column("hauteur")]
        public double Hauteur { get; set; }

        [Column("salleid")]
        public int SalleId { get; set; }

        [ForeignKey("SalleId")]
        [InverseProperty(nameof(Salle.Murs))]
        public Salle Salle { get; set; }

        [InverseProperty(nameof(Equipement.Mur))]
        public List<Equipement> Equipements { get; set; } = new List<Equipement>();

        [InverseProperty(nameof(Capteur.Mur))]
        public List<Capteur> Capteurs { get; set; } = new List<Capteur>();
    }
}
