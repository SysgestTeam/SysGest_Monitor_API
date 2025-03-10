namespace SistemasdeTarefas.Models
{
    public class LogBloqueio
    {
        public int IdLogCartao { get; set; }
        public int IdCartaoAcesso { get; set; }
        public string Codigo { get; set; }
        public bool IsAluno { get; set; }
        public int IdEntidade { get; set; }
        public int NumInterno { get; set; }
        public string NomeEntidade { get; set; }
        public bool IsExterno { get; set; }
        public DateTime DataRegisto { get; set; }
        public DateTime HoraRegisto { get; set; }
        public string TipoBloqueio { get; set; }
        public string AcaoBloqueio { get; set; }
    }
}
