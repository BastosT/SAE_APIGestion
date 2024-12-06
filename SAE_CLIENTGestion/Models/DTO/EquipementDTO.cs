namespace SAE_CLIENTGestion.Models.DTO
{
    public class EquipementDTO
    {

        public int EquipementId { get; set; }
        public string Nom { get; set; }
        public double Hauteur { get; set; }
        public double Longueur { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }


        public int TypeEquipementId { get; set; }
        public int? MurId { get; set; }
        public int SalleId { get; set; }

    }
}
