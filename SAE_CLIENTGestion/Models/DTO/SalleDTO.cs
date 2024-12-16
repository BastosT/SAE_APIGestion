namespace SAE_CLIENTGestion.Models.DTO
{
    public class SalleDTO
    {
        public int SalleId { get; set; }
        public string Nom { get; set; }
        public double Surface { get; set; }
        public int TypeSalleId { get; set; }
        public int BatimentId { get; set; }
        public ICollection<int> MursIds { get; set; }

        public SalleDTO()
        {
            MursIds = new HashSet<int>();
        }
    }
}