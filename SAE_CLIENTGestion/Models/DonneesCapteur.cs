
namespace SAE_CLIENTGestion.Models
{
    public class DonneesCapteur
    {
        public int DonneesCapteurId { get; set; }
        public double Valeur { get; set; }
        public DateTime Timestamp { get; set; }
        public int CapteurId { get; set; }
        public int TypeDonneesId { get; set; }
        public virtual Capteur Capteur { get; set; }
        public virtual TypeDonneesCapteur TypeDonnees { get; set; }

    }
}
