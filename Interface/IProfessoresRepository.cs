using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IProfessoresRepository
    {
        IEnumerable<Funcionario> GetProfessoresSemFoto();
        IEnumerable<Funcionario> GetProfessoresComFotos();
        IEnumerable<Funcionario> GetAll();
        IEnumerable<Funcionario> GetComCartao();
        IEnumerable<Funcionario> GetSemCartao();
    }
}
