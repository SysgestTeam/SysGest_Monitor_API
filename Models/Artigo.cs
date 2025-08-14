namespace SistemasdeTarefas.Models
{
    public class Artigo
    {
        public int IDARTIGO { get; set; }
        public int CODIGO { get; set; }
        public string NOME { get; set; }
        public int IDFAMILIA { get; set; }
        public int IDUNIDADE { get; set; } = 0;
        public decimal PRCVENDA { get; set; }
        public int IDTAXAIPC { get; set; } = 1;
        public int STOCK { get; set; }
        public byte[] FOTO { get; set; }
        public int IDUSR { get; set; } = 0;
        public int IdTaxaImposto { get; set; } = 0;
        public decimal ValorTaxa { get; set; } = 0;
        public int IdArmazem { get; set; } = 0;
        public bool IsDepositoSaldo { get; set; } = false;
        public bool IsMulta { get; set; } = false;
    }

}
