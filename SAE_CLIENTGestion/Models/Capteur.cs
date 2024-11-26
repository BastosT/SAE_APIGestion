using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_CLIENTGestion.Models
{
    public class Capteur
    {
        public int CapteurId { get; set; }
        public string Nom { get; set; }
        public bool EstActif { get; set; }
        public double? DistanceFenetre { get; set; }
        public double? Longeur { get; set; }
        public double? Hauteur { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double? DistancePorte { get; set; }
        public double? DistanceChauffage { get; set; }
        public int SalleId { get; set; }
        public int? MurId { get; set; }
        public virtual Mur Mur { get; set; }
        public virtual Salle Salle { get; set; }
        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }
}
