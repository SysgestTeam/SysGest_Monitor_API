namespace SistemasdeTarefas.Models
{
    public class AlunoDossierLin
    {
        public int IdAlunoDossierLin { get; set; }
        public int IdAlunoDossierCT { get; set; }
        public int IdAlunoDossier { get; set; }
        public bool IsMatricula { get; set; }
        public bool IsConfirmacao { get; set; }
        public bool IsContrato { get; set; }
        public bool IsArtigo { get; set; }
        public bool IsMulta { get; set; }
        public int? IdContrato { get; set; }
        public string? NomeContrato { get; set; }
        public int? IdArtigo { get; set; }
        public decimal? ValorEmissao { get; set; }
        public int Quant { get; set; }
        public decimal? ValorUnitPrevisto { get; set; }
        public decimal? SubTotal { get; set; }
        public bool IsBolseiro { get; set; }
        public int? IdBolsa { get; set; }
        public bool TemOutroDesc { get; set; }
        public decimal? TxDesc { get; set; }
        public decimal? ValorDesc { get; set; }
        public decimal? TxDesc2 { get; set; }
        public decimal? ValorDesc2 { get; set; }
        public string? NomeDesc { get; set; }
        public DateTime DataLimite { get; set; }
        public decimal? ValorPago { get; set; }
        public bool? Pago { get; set; }
        public DateTime? DataPag { get; set; }
        public bool Anulado { get; set; }
        public bool Inactivo { get; set; }
        public bool Facturado { get; set; }
        public int? IdAlunoFacturacaoLin { get; set; }
        public int? IdLinFactura { get; set; }
        public string? NumFactura { get; set; }
        public bool AplicouMulta { get; set; }
        public int? IdMultaCond { get; set; }
        public decimal? ValorMulta { get; set; }
        public int IdRefCTIfMulta { get; set; }
        public int IdAluno { get; set; }
        public int IdDocOrigemCC { get; set; }
        public DateTime? DataRegisto { get; set; }
        public DateTime? DataAlter { get; set; }
        public string? TimeRegist { get; set; }
        public string? TimeAlter { get; set; }
        public int? IdUserRegisto { get; set; }
        public int? IdUserAlter { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? IdUserDel { get; set; }
        public bool? Deleted { get; set; }
        public string? TimeDeleted { get; set; }
        public bool IsFromRecibo { get; set; }
        public int? IdRecibo { get; set; }
        public string? Nome { get; set; }
        public int NumALuno { get; set; }
        public string? NomeClasse { get; set; }
    }

}
