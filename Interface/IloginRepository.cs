using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IloginRepository
    {
        public string login(string user, string senha);
        public Task<int> GerarNumerode6DigitosParaosPais(string nome, string numero, string email);
        public void CriarSenhaParaPai(string numero, string senha);
        public IEnumerable<Login> ObterSenhaDesencriptada(string numeroTelefone);
        public string ValidarCodigoVerificacao(int numeroTelefone, string codigoRecebido);
        public string GenerateJwtToken(string username); 

    }
}
