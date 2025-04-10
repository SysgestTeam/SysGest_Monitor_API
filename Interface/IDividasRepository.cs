using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IDividasRepository
    {
        IEnumerable<Devedor> GetDevedores(DateTime? dataInicial = null, DateTime? dataFinal = null);
        IEnumerable<Devedor> GetDevedorPorAluno(int numAluno);
        IEnumerable<ConfigBloqueio> ObterTodasConfigBloqueio();
        IEnumerable<LogBloqueio> LogBloqueio(DateTime? dataInicial = null, DateTime? dataFinal = null);
        public int BloquearDevedoresPorMes(DateTime dataInicial, DateTime dataFinal);
        public void LogBloqueio( int IsAluno , int IdEntidade ,string TipoBloqueio,string AcaoBloqueio);
        public int BloqueioCartao(int[] numAluno = null, bool emMassa = false);
        public void DesbloqueioCartao(int[] numAluno = null);
        public void CriarOuAtualizarConfigBloqueio(bool APLICAR_MULTA,int DIA_MULTA,TimeOnly HORA_BLOQUEIO, int NUMERO_MESES_DIVIDA);
        public void NaoOUBloqueioCartao(int[] numAluno = null, int tipo = 1);
    }
}
