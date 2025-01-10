using SAE_APIGestion.Models.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_APIGestion.Models.DTO
{
    public class CapteurDTO
    {

        public int CapteurId { get; set; }
        public string Nom { get; set; }
        public bool EstActif { get; set; }
        public double? Longueur { get; set; }
        public double? Hauteur { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }

        public double? DistancePorte { get; set; }
        public double? DistanceFenetre { get; set; }
        public double? DistanceChauffage { get; set; }

        public int? SalleId { get; set; }
        public int? MurId { get; set; }
    }
}
