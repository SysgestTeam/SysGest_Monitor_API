using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IAlunoRepository
    {
        IEnumerable<TabAluno> GetAlunos();
    }
}
