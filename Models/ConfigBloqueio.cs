namespace SistemasdeTarefas.Models
{
    public class ConfigBloqueio
    {
        public bool APLICAR_MULTA { get; set; }
        public int DIA_MULTA { get; set; }
        public int IDCONFIGBLOQUEIO { get; set; }
        public TimeOnly HORA_BLOQUEIO { get; set; }
        public bool ATIVO { get; set; }
        public int NUMERO_MESES_DIVIDA { get; set; }
        public DateTime DATA_CRIACAO { get; set; }
        public DateTime DATA_ATUALIZACAO { get; set; }
    }
}
