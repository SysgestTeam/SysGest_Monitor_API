using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IRelatorioRepository
    {
       // IEnumerable<Relatorio> GetEntradasSaidasPorMes(int mes);
       // IEnumerable<Relatorio> GetEntradasSaidasPorAno(int ano);
        IEnumerable<Relatorio> GetRelatorioPorIntervalo(String dataInicial, String dataFinal);
    }
}
