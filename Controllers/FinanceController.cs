using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using SistemasdeTarefas.Repository;
using System.Globalization;

namespace SistemasdeTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class FinanceController : Controller
    {

        private readonly ILogger<FinanceController> _logger;
        private readonly IFinancaRepository _financaRepository;

        public FinanceController(ILogger<FinanceController> logger, IFinancaRepository financaRepository)
        {
            _logger = logger;
            _financaRepository = financaRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoDossier>>> ListarAlunoDossier(int number)
        {
            var alunos = await _financaRepository.ListAlunoDossier(number);

            return Ok(alunos);
        }

        [HttpGet("IdStudentDossier ")]
        public async Task<ActionResult<IEnumerable<AlunoDossierCT>>> ListarAlunoDossierCT(int IdStudentDossier)
        {
            var alunos = await _financaRepository.GetAlunoDossierCTByIdAlunoDossier(IdStudentDossier);

            return Ok(alunos);
        }

        [HttpGet("StudentDossierCTId")]
        public async Task<ActionResult<IEnumerable<AlunoDossierLin>>> ListarAlunoDossierLin(int StudentDossierCTId)
        {
            var alunos = await _financaRepository.GetAlunoDossierLinCTByIdAlunoDossierCT(StudentDossierCTId);

            return Ok(alunos);
        }

        [HttpGet("paid-or-unpaid-contracts")]
        public async Task<ActionResult<IEnumerable<AlunoDossierLin>>> GetContratosPagosOUNaoPagos(int IdStudentDossierCT, bool pago)
        {
            var alunos = await _financaRepository.GetContratosPagosOUNaoPagos(IdStudentDossierCT, pago);

            return Ok(alunos);
        }

        //[HttpGet("contratos-por-ano")]
        //public async Task<ActionResult<IEnumerable<AlunoDossierLin>>> GetContratosPagosOUNaoPagosByYear(int? ano = null,
        //     bool? pago = null,
        //     int? idTurma = null,
        //     int? idClasse = null,
        //     int? numAluno = null)
        //{
        //    var alunos = await _financaRepository.GetContratosPagosOUNaoPagosByYear(ano,
        //     pago,
        //     idTurma,
        //     idClasse,
        //     numAluno);

        //    return Ok(alunos);
        //}

        [HttpGet("customer-invoice")]
        public async Task<ActionResult<IEnumerable<CustomerInvoice>>> GetCustomerInvoicesByAnoAndNumAluno(int? year = null,int? numberStudent = null)
        {
            var alunos = await _financaRepository.GetCustomerInvoicesByAnoAndNumAluno(year, numberStudent);

            return Ok(alunos);
        }
    }
}
