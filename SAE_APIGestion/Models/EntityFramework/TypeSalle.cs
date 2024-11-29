using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [Column("tys_description" , TypeName ="varchar(250)")]
        [StringLength(250)]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Salle> Salles { get; set; }
    }
}
