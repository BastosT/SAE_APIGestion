using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_APIGestion.Models.DTO
{
    public class TypeSalleDTO
    {

        public int TypeSalleId { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
    }
}
