using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{

    [Table("t_e_typeequipement_tye")]
    public class TypeEquipement
    {
        public TypeEquipement()
        {
            Equipements = new HashSet<Equipement>();
        }

        [Key]
        [Column("tye_id")]
        public int TypeEquipementId { get; set; }

        [Required]
        [Column("tye_nom",TypeName ="varchar(100)")]
        [StringLength(100)]
        public string Nom { get; set; }

        [Column("tye_couleur", TypeName = "varchar(7)")]
        [StringLength(7)]
        public string Couleur { get; set; }

        public virtual ICollection<Equipement> Equipements { get; set; }
    }


}
