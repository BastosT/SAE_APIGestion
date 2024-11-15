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
        [Column("tys_idtypesalle")]
        public int IdTypeSalle { get; set; }

        [Column("tys_nomtypesalle")]
        public string NomTypeSalle { get; set; }    // TD, TP, etc.

        [Column("tys_descriptiontypesalle")]
        public string Description { get; set; }
    }
}
