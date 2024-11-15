using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("Id")]
    [Table("t_e_typesalle_typsal")]
    public class TypeSalle
    {
        [Key]
        [Column("typsal_id")]
        public int Id { get; set; }

        [Column("typsal_nom")]
        public string Nom { get; set; }    // TD, TP, etc.

        [Column("typsal_description")]
        public string Description { get; set; }
    }
}
