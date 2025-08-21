using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IFinancaRepository
    {
        public Task<IEnumerable<AlunoDossier>> ListAlunoDossier(int number);
    }
}
