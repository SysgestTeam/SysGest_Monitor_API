namespace SistemasdeTarefas.Models
{
    public class ConsumoPosDto
    {
        public int NumAluno { get; set; }
        public string? NomeAluno { get; set; }
        public decimal UsedValue { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnidade { get; set; }
        public string? NomeItem { get; set; }
        public int IdFamilia { get; set; }
        public int IdArtigo { get; set; }
        public int? IdSaldoConsumo { get; set; }
        public bool Anular { get; set; }
        public int IdUser { get; set; }
    }
}
