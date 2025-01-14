namespace SAE_CLIENTGestion.Models
{
    public class TypeEquipement
    {
        public int TypeEquipementId { get; set; }

        public string Nom { get; set; }
        public string Couleur { get; set; }

        public virtual ICollection<Equipement> Equipements { get; set; }

    }
}
