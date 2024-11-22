using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAE_APIGestion.Models.EntityFramework
{
    [Table("t_e_salle_sal")]
    public class Salle
    {
        [Key]
        [Column("sal_id")]
        public int SalleId { get; set; }

        [Required]
        [Column("sal_nom", TypeName = "varchar(100)")]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [Column("sal_surface")]
        public double Surface { get; set; }

        [Required]
        [Column("tys_id")]
        public int TypeSalleId { get; set; }

        [ForeignKey("TypeSalleId")]
        public virtual TypeSalle TypeSalle { get; set; }

        [Required]
        [Column("bat_id")]
        public int BatimentId { get; set; }

        [ForeignKey("BatimentId")]
        public virtual Batiment Batiment { get; set; }

        [Column("mur_faceid")]
        public int MurFaceId { get; set; }

        [ForeignKey("MurFaceId")]
        public virtual Mur MurFace { get; set; }

        [Column("mur_entreeid")]
        public int MurEntreeId { get; set; }

        [ForeignKey("MurEntreeId")]
        public virtual Mur MurEntree { get; set; }

        [Column("mur_gaucheid")]
        public int MurGaucheId { get; set; }

        [ForeignKey("MurGaucheId")]
        public virtual Mur MurGauche { get; set; }

        [Column("mur_droiteid")]
        public int MurDroiteId { get; set; }

        [ForeignKey("MurDroiteId")]
        public virtual Mur MurDroite { get; set; }
    }

}
