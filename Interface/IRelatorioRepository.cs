using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IRelatorioRepository
    {
        IEnumerable<Relatorio> GetRelatorioPorIntervalo(DateTime dataInicial, DateTime dataFinal);
    }
}
