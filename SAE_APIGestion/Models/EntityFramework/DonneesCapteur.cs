using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_donneescapteur_dcp")]
    public class DonneesCapteur
    {
        [Key]
        [Column("dcp_id")]
        public int DonneesCapteurId { get; set; }

        [Column("dcp_valeur")]
        public double Valeur { get; set; }

        [Column("dcp_timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("cap_id")]
        public int CapteurId { get; set; }

        [Column("tdc_id")]
        public int TypeDonneesId { get; set; }


        [ForeignKey("CapteurId")]
        public virtual Capteur Capteur { get; set; }
        [ForeignKey("TypeDonneesId")]
        public virtual TypeDonneesCapteur TypeDonnees { get; set; }
    }
}
