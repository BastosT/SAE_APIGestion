namespace SAE_CLIENTGestion.Models
{
    public class TypeDonneesCapteur
    {

        public int TypeDonneesCapteurId { get; set; }

        public string Nom { get; set; }
        public string Unite { get; set; }
        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }

    }
}
