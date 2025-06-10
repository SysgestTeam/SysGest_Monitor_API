using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IAlunoRepository
    {
       
        IEnumerable<TabAluno> GetAlunos();
        IEnumerable<Student> GetAllStudents(int ano, int status);
        IEnumerable<Class> GetAllClass(int ano);
        IEnumerable<Existencia_Card> GetAlunosSemFotos();
        IEnumerable<Existencia_Card> GetAlunosSemFotosFiltro(int? idclasse = null, int? idturma = null);
        IEnumerable<TabAluno> GetAlunosFiltro(int? idclasse  = null, int? idturma = null);
        IEnumerable<Turmas> GetTurmas(int classe);
        IEnumerable<Classes> GetClasses();
    }
}
