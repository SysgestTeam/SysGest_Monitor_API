namespace SistemasdeTarefas.Models
{
    public class AlunoDossierCT
    {
        public int IdAlunoDossierCT { get; set; }
        public int IdAlunoDossier { get; set; }
        public int? IdAnoLectivo { get; set; }
        public int IdContrato { get; set; }
        public string? NomeContrato { get; set; }
        public decimal? ValorEmissao { get; set; }
        public DateTime? DataIni { get; set; }
        public DateTime? DataFim { get; set; }
        public decimal? ValorPrevisto { get; set; }
        public bool? IsBolseiro { get; set; }
        public int? IdBolsa { get; set; }
        public bool? TemOutroDesc { get; set; }
        public decimal? TxDesc { get; set; }   
        public string? NomeDesc { get; set; }
        public bool? RemoveDesc { get; set; }
        public bool? IsAnoAnterior { get; set; }
        public DateTime? DataRegisto { get; set; }
        public DateTime? DataAlter { get; set; }
        public string? TimeRegist { get; set; }
        public string? TimeAlter { get; set; }
        public int? IdUserRegisto { get; set; }
        public int? IdUserAlter { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? IdUserDel { get; set; }
        public bool Deleted { get; set; }
        public string? TimeDeleted { get; set; }
        public bool? IsMulta { get; set; }
    }
}
