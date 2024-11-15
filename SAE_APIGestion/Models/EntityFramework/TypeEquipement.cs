using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{

    [Table("t_e_typeequipement_tye")]
    public class TypeEquipement
    {
        [Key]
        [Column("tye_id")]
        public int TypeEquipementId { get; set; }

        [Required]
        [Column("tye_nom")]
        public string Nom { get; set; }

    }


}
