using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_APIGestion.Models.DTO
{
    public class TypeEquipementDTO
    {

        public int TypeEquipementId { get; set; }
        public string Nom { get; set; }
        public string Couleur { get; set; }
    }
}
