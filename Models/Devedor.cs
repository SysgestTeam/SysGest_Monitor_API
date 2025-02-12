namespace SistemasdeTarefas.Models
{
    public class Devedor
    {

        public int IdAluno { get; set; }
        public string Nome { get; set; }
        public int NumAluno { get; set; }
        public string Contrato { get; set; }
        public decimal Valor { get; set; }
        public string Mes { get; set; }
        public string Estado { get; set; }
        public DateTime DataLimite { get; set; }
        public int Ano { get; set; }
        public string Familia { get; set; }
        public string Descr { get; set; }
        public bool Bloqueado { get; set; }

    }
}
 