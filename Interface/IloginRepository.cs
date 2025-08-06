namespace SistemasdeTarefas.Interface
{
    public interface IloginRepository
    {
        public string login(string user, string senha);
        public Task<int> GerarNumerode6DigitosParaosPais(string nome, string numero, string email);
        public void CriarSenhaParaPai(string numero, string senha);
        public string ObterSenhaDesencriptada(string numero);
        public string ValidarCodigoVerificacao(int numeroTelefone, string codigoRecebido);
        public string GenerateJwtToken(string username); 

    }
}
