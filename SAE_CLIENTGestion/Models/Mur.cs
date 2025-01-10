namespace SAE_CLIENTGestion.Models
{
    public class Mur
    {
        public int MurId { get; set; }

        public string Nom { get; set; }

        public double Longueur { get; set; }

        public double Hauteur { get; set; }

        public MurOrientation Orientation { get; set; }




        public virtual ICollection<Equipement> Equipements { get; set; }

        public virtual ICollection<Capteur> Capteurs { get; set; }

        public int? SalleId { get; set; }

        public virtual Salle? Salle { get; set; }
    }

    public enum MurOrientation
    {
        Nord,
        Est,
        Sud,
        Ouest
    }
}