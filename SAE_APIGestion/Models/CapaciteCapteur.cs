using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models
{
    [PrimaryKey("Id")]
    [Table("typecapteur")]
    public class TypeCapteur
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nom")]
        public string Nom { get; set; }          // Température, CO2, Lumière, etc.

        [Column("description")]
        public string Description { get; set; }

        [Column("estactionneur")]
        public bool EstActionneur { get; set; }  // Indique si c'est un actionneur
    }
}
