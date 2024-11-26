using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SAE_CLIENTGestion.Models
{
    public class Salle
    {
        public int SalleId { get; set; }
        public string Nom { get; set; }
        public double Surface { get; set; }


        public int TypeSalleId { get; set; }
        public virtual TypeSalle TypeSalle { get; set; }
        public int BatimentId { get; set; }
        public virtual Batiment Batiment { get; set; }


        public int MurFaceId { get; set; }
        public virtual Mur MurFace { get; set; }
        public int MurEntreeId { get; set; }
        public virtual Mur MurEntree { get; set; }
        public int MurGaucheId { get; set; }
        public virtual Mur MurGauche { get; set; }
        public int MurDroiteId { get; set; }
        public virtual Mur MurDroite { get; set; }

    }
}
