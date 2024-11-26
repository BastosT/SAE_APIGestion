using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_typedonneescapteur_tdc")]
    public class TypeDonneesCapteur
    {
        public TypeDonneesCapteur()
        {
            DonneesCapteurs = new HashSet<DonneesCapteur>();
        }

        [Key]
        [Column("tdc_id")]
        public int TypeDonneesCapteurId { get; set; }

        [Required]
        [Column("tdc_nom")]
        public string Nom { get; set; }

        [Required]
        [Column("tdc_unite")]
        public string Unite { get; set; }
        public virtual ICollection<DonneesCapteur> DonneesCapteurs { get; set; }
    }
}
