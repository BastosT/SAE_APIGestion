using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_CLIENTGestion.Models
{
    public class Equipement
    {
        public int EquipementId { get; set; }

        public string Nom { get; set; }

        public int TypeEquipementId { get; set; }

        public virtual TypeEquipement TypeEquipement { get; set; }

        public double Largeur { get; set; }

        public double Hauteur { get; set; }

        public double PositionX { get; set; }

        public double PositionY { get; set; }

        public int? MurId { get; set; }

        public virtual Mur Mur { get; set; }

        public int SalleId { get; set; }

        public virtual Salle Salle { get; set; }

    }
}
