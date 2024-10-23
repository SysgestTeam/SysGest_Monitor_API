using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IFuncionarioRepository
    {
        IEnumerable<Funcionario> GetFuncionarioSemFoto();
        IEnumerable<Funcionario> GetFuncionarioComFotos();
        IEnumerable<Funcionario> GetAll();
        IEnumerable<Funcionario> GetComCartao();
        IEnumerable<Funcionario> GetSemCartao();
    }
}
