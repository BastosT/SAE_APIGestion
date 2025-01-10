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

        public virtual Batiment? Batiment { get; set; }

        public virtual ICollection<Mur> Murs { get; set; }

        public virtual ICollection<Capteur> Capteurs { get; set; }

        public virtual ICollection<Equipement> Equipements { get; set; }
    }
}