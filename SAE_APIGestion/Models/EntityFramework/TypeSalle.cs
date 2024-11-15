using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("Id")]
    [Table("t_e_typesalle_tys")]
    public class TypeSalle
    {
        [Key]
        [Column("tys_id")]
        public int TypeSalleId { get; set; }

        [Column("tys_nom")]
        public string Nom { get; set; }    // TD, TP, etc.

        [Column("tys_description")]
        public string Description { get; set; }
    }
}
