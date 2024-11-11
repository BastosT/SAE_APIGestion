using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models
{
    [PrimaryKey("Id")]
    [Table("typesalle")]
    public class TypeSalle
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string Nom { get; set; }    // TD, TP, etc.

        [Column("description")]
        public string Description { get; set; }
    }
}
