using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Interface
{
    public interface IFinancaRepository
    {
        public Task<IEnumerable<AlunoDossier>> ListAlunoDossier(int number);
        public Task<IEnumerable<AlunoDossierCT>> GetAlunoDossierCTByIdAlunoDossier(int IdAlunoDossier);
        public Task<IEnumerable<AlunoDossierLin>> GetAlunoDossierLinCTByIdAlunoDossierCT(int IdAlunoDossierCT);
        public Task<IEnumerable<AlunoDossierLin>> GetContratosPagosOUNaoPagos(int IdAlunoDossierCT, bool pago);
        public Task<IEnumerable<AlunoDossierLin>> GetContratosPagosOUNaoPagosByYear(int? ano = null,bool? pago = null,int? idTurma = null,int? idClasse = null,int? numAluno = null);
        public Task<IEnumerable<CustomerInvoice>> GetCustomerInvoicesByAnoAndNumAluno(int? ano = null, int? numAluno = null);
    }
}
