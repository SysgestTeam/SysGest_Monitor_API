using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface ISaldoConsumo
    {
        IEnumerable<SaldoConsumo> GetSaldoConsumo(int NumeroAluno);
        IEnumerable<SaldoConsumo> GetHistóricoConsumo(int NumeroAluno);
        IEnumerable<SaldoConsumo> GetHistóricoConsumoById(int id);
        public void Consumo(int numAluno, decimal UsedValue);
        public void GerarTicket(int numAluno);
        public IEnumerable<Ticket> ListTicket(int numAluno);

        public void PrintTicket(int numAluno);

    }
}
