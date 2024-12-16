using SAE_APIGestion.Models.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAE_APIGestion.Models.DTO
{
    public class BatimentDTO
    {

        public int BatimentId { get; set; }

        public string Nom { get; set; }

        public string Adresse { get; set; }


    }
}
