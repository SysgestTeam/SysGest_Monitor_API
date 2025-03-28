using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IDividasRepository
    {
        IEnumerable<Devedor> GetDevedores(DateTime? dataInicial = null, DateTime? dataFinal = null);
        IEnumerable<LogBloqueio> LogBloqueio(DateTime? dataInicial = null, DateTime? dataFinal = null);
        public int BloquearDevedoresPorMes(DateTime dataInicial, DateTime dataFinal);
        public void LogBloqueio( int IsAluno , int IdEntidade ,string TipoBloqueio,string AcaoBloqueio);
        public int BloqueioCartao(int[] numAluno = null, bool emMassa = false);
        public void DesbloqueioCartao(int[] numAluno = null);
        public void NaoOUBloqueioCartao(int[] numAluno = null, int tipo = 1);
    }
}
