using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [PrimaryKey("Id")]
    [Table("typesalle")]
    public class TypeSalle
    {
        [Key]
        [Column("idtypesalle")]
        public int Id { get; set; }

        [Column("nomtypesalle")]
        public string Nom { get; set; }    // TD, TP, etc.

        [Column("descriptiontypesalle")]
        public string Description { get; set; }
    }
}
