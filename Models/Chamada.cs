namespace SistemasdeTarefas.Models
{
    public class Chamada
    {
        public string nome { get; set; }
        public string hora { get; set; }
        public string descricao { get; set; }
        public string turma { get; set; }
        public byte[] foto { get; set; }
        public string registo { get; set; }
    }
}
