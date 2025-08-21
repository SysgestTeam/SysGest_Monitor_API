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

    }
}
