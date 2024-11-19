using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_typesalle_tys")]
    public class TypeSalle
    {
        public TypeSalle()
        {
            Salles = new HashSet<Salle>();
        }

        [Key]
        [Column("tys_id")]
        public int TypeSalleId { get; set; }

        [Required]
        [Column("tys_nom" , TypeName ="varchar(100)")]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [Column("tys_description" , TypeName ="varachar(250)")]
        [StringLength(250)]
        public string Description { get; set; }

        public virtual ICollection<Salle> Salles { get; set; }
    }
}
