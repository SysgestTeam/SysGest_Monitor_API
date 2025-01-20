namespace SistemasdeTarefas.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int IdAluno { get; set; }
        public int IdSaldoConsumo { get; set; }
        public bool Apagado { get; set; }
        public string Nome { get; set; }
        public decimal ValorAlmo { get; set; }

        public DateTime Data { get; set; }
        public int NumeroTicket { get; set; }
    }
}
