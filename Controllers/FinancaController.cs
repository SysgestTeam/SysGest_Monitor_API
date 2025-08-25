using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using SistemasdeTarefas.Repository;

namespace SistemasdeTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class FinancaController : Controller
    {

        private readonly ILogger<FinancaController> _logger;
        private readonly IFinancaRepository _financaRepository;

        public FinancaController(ILogger<FinancaController> logger, IFinancaRepository financaRepository)
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

        [HttpGet("alunoDossier")]
        public async Task<ActionResult<IEnumerable<AlunoDossierCT>>> ListarAlunoDossierCT(int IdAlunoDossier)
        {
            var alunos = await _financaRepository.GetAlunoDossierCTByIdAlunoDossier(IdAlunoDossier);

            return Ok(alunos);
        }

        [HttpGet("alunoDossierCT")]
        public async Task<ActionResult<IEnumerable<AlunoDossierLin>>> ListarAlunoDossierLin(int AlunoDossierCT)
        {
            var alunos = await _financaRepository.GetAlunoDossierLinCTByIdAlunoDossierCT(AlunoDossierCT);

            return Ok(alunos);
        }

        [HttpGet("contratos-pagos-ou-nao-pagos")]
        public async Task<ActionResult<IEnumerable<AlunoDossierLin>>> GetContratosPagosOUNaoPagos(int IdAlunoDossierCT, bool pago)
        {
            var alunos = await _financaRepository.GetContratosPagosOUNaoPagos(IdAlunoDossierCT, pago);

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
        public async Task<ActionResult<IEnumerable<AlunoDossierLin>>> GetCustomerInvoicesByAnoAndNumAluno(int? ano = null,int? numAluno = null)
        {
            var alunos = await _financaRepository.GetCustomerInvoicesByAnoAndNumAluno(ano,numAluno);

            return Ok(alunos);
        }
    }
}
