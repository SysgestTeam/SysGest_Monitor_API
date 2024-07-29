using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IExistenciaCardRepository
    {
        IEnumerable<Existencia_Card> GetExistenciaCard();
        IEnumerable<Existencia_Card> GetInexistenciaCard();
        IEnumerable<Existencia_Card> GetBloqueados();
        IEnumerable<Existencia_Card> GetExistenciaCard_SemAcompanhante();
    }
}
