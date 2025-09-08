namespace SistemasdeTarefas.Models
{
    public class Existencia_Card
    {
        public int NumAluno { get; set; }
        public string  Nome { get; set; }
        public string NomeTurma { get; set; }
        public byte[] Foto { get; set; }
        public string CodigoCartao { get; set; }
        public decimal saldo { get; set; }
        public decimal limiteDiario { get; set; }
        public decimal ValorDiaConsumido { get; set; }
        public bool Bloqueado { get; set; }
        public bool? NaoBloqueavel { get; set; }
    }
}
