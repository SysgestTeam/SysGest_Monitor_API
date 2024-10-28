namespace SistemasdeTarefas.Interface
{
    public interface IloginRepository
    {
        public string login(string user, string senha);

        public string GenerateJwtToken(string username);

    }
}
