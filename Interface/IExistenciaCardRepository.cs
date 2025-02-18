using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IExistenciaCardRepository
    {
        IEnumerable<Existencia_Card> GetExistenciaCard();
        IEnumerable<Existencia_Card> GetInexistenciaCard();
        IEnumerable<Existencia_Card> GetInexistenciaCardFiltro(int? idclasse = null, int? idturma = null);
        IEnumerable<Existencia_Card> GetInexistenciaCardBloqueadoFiltro(int? idclasse = null, int? idturma = null);
        IEnumerable<Existencia_Card> GetInexistenciaCardSemAcompanhateFiltro(int? idclasse = null, int? idturma = null);
        IEnumerable<Existencia_Card> GetBloqueados();
        IEnumerable<Existencia_Card> GetTodosCartoes();
        IEnumerable<Existencia_Card> GetExistenciaCard_SemAcompanhante();
    }
}
