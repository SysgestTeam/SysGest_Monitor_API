using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IDividasRepository
    {
        IEnumerable<Devedor> GetDevedores(DateTime? dataInicial = null, DateTime? dataFinal = null);
    }
}
