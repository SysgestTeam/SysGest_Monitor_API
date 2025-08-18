using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface ISaldoConsumo
    {
        IEnumerable<SaldoConsumo> GetSaldoConsumo(int NumeroAluno);
        IEnumerable<SaldoConsumo> GetHistóricoConsumo(int NumeroAluno);
        IEnumerable<SaldoConsumo> GetHistóricoConsumoById(int id);
        IEnumerable<DashboardTicketDia> DashoboarTickDia();
        IEnumerable<ConsumoPosDto> Listas_ConsumoPOs();
        public void Consumo(int numAluno, decimal UsedValue);
        public void GerarTicket(int numAluno, int idsaldo);
        public void RemoverSaldoETicket(int idsaldo, bool apagado);
        public IEnumerable<Ticket> ListTicket(int numAluno);
        public IEnumerable<Ticket> FiltrarTicketsPorData( DateTime? DataInicio = null ,DateTime? DataFim = null );
        public IEnumerable<Ticket> ListTicket();
        public IEnumerable<Ticket> List();
        public IEnumerable<Dashboard> Dashboad();
        public IEnumerable<CalculoParaEstatistica> CalculoParaEstatistica();
        public Task InserirPosCafeteriaAsync(
            decimal quantidade,
            decimal precoUnidade,
            string nomeItem,
            int idFamilia,
            int idArtigo,
            int idSaldoConsumo,
            bool anular,
            int idUser,
            int aluno);

        public Task ConsumoPOSAsync(
            int numAluno,
            decimal usedValue,
            decimal quantidade,
            decimal precoUnidade,
            string nomeItem,
            int idFamilia,
            int idArtigo,
            bool anular,
            int idUser);
        public void PrintTicket(int numAluno);

    }
}
