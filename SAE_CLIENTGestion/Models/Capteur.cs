
namespace SAE_CLIENTGestion.Models
{
    public class Capteur
    {
        public int CapteurId { get; set; }
        public string Nom { get; set; }
        public bool EstActif { get; set; }
        public double Longueur { get; set; } = 0;
        public double Hauteur { get; set; } = 0;
        public double PositionX { get; set; } = 0;
        public double PositionY { get; set; } = 0;
        public double? DistanceFenetre { get; set; } = 0;
        public double? DistancePorte { get; set; } = 0;
        public double? DistanceChauffage { get; set; } = 0;
        public int? SalleId { get; set; }
        public int? MurId { get; set; }
        public virtual Mur Mur { get; set; }
        public virtual Salle Salle { get; set; }
        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }
}
