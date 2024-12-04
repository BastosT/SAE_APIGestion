﻿namespace SAE_CLIENTGestion.Models.DTO
{
    public class SalleDTO
    {

        public int SalleId { get; set; }
        public string Nom { get; set; }
        public double Surface { get; set; }
        public int TypeSalleId { get; set; }
        public int BatimentId { get; set; }
        public int MurFaceId { get; set; }
        public int MurEntreeId { get; set; }
        public int MurGaucheId { get; set; }
        public int MurDroiteId { get; set; }

    }
}