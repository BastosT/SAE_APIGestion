using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SAE_CLIENTGestion.Models
{
    public class Mur
    {

        public int MurId { get; set; }
        public string Nom { get; set; }    // Nord, Sud, Est, Ouest
        public double Longueur { get; set; }
        public double Hauteur { get; set; }
        public int SalleId { get; set; }
        public Salle Salle { get; set; }
        public List<Equipement> Equipements { get; set; } = new List<Equipement>();
        public List<Capteur> Capteurs { get; set; } = new List<Capteur>();
    }
}
