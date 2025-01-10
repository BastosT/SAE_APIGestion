using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DTO
{
    public class MurDTO
    {

        public int MurId { get; set; }

        public string Nom { get; set; }

        public double Longueur { get; set; }

        public double Hauteur { get; set; }

        public MurOrientation Orientation { get; set; }

        public int? SalleId { get; set; }

    }
}
