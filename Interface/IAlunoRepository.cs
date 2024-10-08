using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IAlunoRepository
    {
        IEnumerable<TabAluno> GetAlunos();
        IEnumerable<Existencia_Card> GetAlunosSemFotos();
        IEnumerable<TabAluno> GetAlunosFiltro(int? idclasse  = null, int? idturma = null);
        IEnumerable<Turmas> GetTurmas(int classe);
        IEnumerable<Classes> GetClasses();
    }
}
