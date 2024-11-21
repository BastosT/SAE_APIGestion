﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_CLIENTGestion.Models
{
    public class Capteur
    {
        public int CapteurId { get; set; }
        public bool EstActif { get; set; }
        public double? DistanceFenetre { get; set; }
        public double? Longeur { get; set; }
        public double? Largeur { get; set; }
        public double? DistancePorte { get; set; }
        public double? DistanceChauffage { get; set; }
        public int SalleId { get; set; }
        public int? MurId { get; set; }

        public virtual Mur Mur { get; set; }
        public virtual Salle Salle { get; set; }
        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }
}
