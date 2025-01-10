
namespace SAE_CLIENTGestion.Models
{
    public class Batiment
    {
        public int BatimentId { get; set; }

        public string Nom { get; set; }

        public string Adresse { get; set; }

        public List<Salle> Salles { get; set; } = new List<Salle>();
    }
}
