namespace SistemasdeTarefas.Interface
{
    public interface IloginRepository
    {
        IEnumerable<string> login(string user, string senha);

    }
}
