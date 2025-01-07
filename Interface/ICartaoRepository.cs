using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface ICartaoRepository
    {
        IEnumerable<Cartao> GetEncarregados(string codigo);
        IEnumerable<Cartao> GetEstudantes(string codigo);
        IEnumerable<Cartao> GetEstudantesPeloNome(string nome);
    }
}
