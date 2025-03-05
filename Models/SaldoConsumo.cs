namespace SistemasdeTarefas.Models
{
    public class SaldoConsumo
    {
        public int Id { get; set; }
        public int IdAluno { get; set; }
        public bool Anulado { get; set; }
        public decimal UsedValue { get; set; }
        public decimal ValorAlmo { get; set; }
        public string Nome { get; set; }
        public DateTime DataRegisto { get; set; }
        public DateTime DataAlter { get; set; }
    }
}
