using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_CLIENTGestion.Models
{
    public class TypeSalle

    {
        public int TypeSalleId { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Salle> Salles { get; set; }
    }
}
