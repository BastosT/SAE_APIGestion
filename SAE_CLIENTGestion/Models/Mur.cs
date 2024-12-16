using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SAE_CLIENTGestion.Models
{
    public class Mur
    {
        public int MurId { get; set; }
        public string Nom { get; set; }
        public double Longueur { get; set; }
        public double Hauteur { get; set; }
        public Orientation Orientation { get; set; }
        public virtual List<Equipement> Equipements { get; set; } = new List<Equipement>();
        public virtual List<Capteur> Capteurs { get; set; } = new List<Capteur>();
        public int? SalleId { get; set; }
        public virtual Salle? Salle { get; set; }
    }

    public enum Orientation
    {
        Nord,
        Ouest,
        Sud,
        Est
    }
}