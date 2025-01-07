namespace SistemasdeTarefas.Models
{
    public class Cartao
    {
        public string Nome{ get; set; }
        public string CodigoCartao { get; set; }
        public byte[] Foto { get; set; }
        public string NomeTurma { get; set; }
        public int NumAluno { get; set; }
        public decimal saldo { get; set; }
    }
}
