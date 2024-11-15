using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_salle_sal")]
    public class Salle
    {
        public Salle()
        {
            Murs = new HashSet<Mur>();
            Equipements = new HashSet<Equipement>();
            Capteurs = new HashSet<Capteur>();
        }

        [Key]
        [Column("sal_id")]
        public int SalleId { get; set; }

        [Required]
        [Column("sal_nom")]
        public string Nom { get; set; }

        [Column("sal_surface")]
        public double Surface { get; set; }

        [Column("tys_id")]
        public int TypeSalleId { get; set; }

        [ForeignKey("TypeSalleId")]
        public virtual TypeSalle TypeSalle { get; set; }

        [Column("bat_id")]
        public int BatimentId { get; set; }

        [ForeignKey("BatimentId")]
        public virtual Batiment BatimentNavigation { get; set; }

        public virtual ICollection<Mur> Murs { get; set; }
        public virtual ICollection<Equipement> Equipements { get; set; }
        public virtual ICollection<Capteur> Capteurs { get; set; }
    }
}
