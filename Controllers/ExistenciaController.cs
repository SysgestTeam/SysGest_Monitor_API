using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExistenciaController : ControllerBase
    {
        private readonly IExistenciaCardRepository _existenciaCardRepository;

        public ExistenciaController(IExistenciaCardRepository existenciaCardRepository)
        {
            _existenciaCardRepository = existenciaCardRepository;
        }

        [HttpGet("Com-cartao")]
        public ActionResult<IEnumerable<Existencia_Card>> GetExistenciaCard()
        {
            var existenciaCards = _existenciaCardRepository.GetExistenciaCard();
            return Ok(existenciaCards);
        }
        [HttpGet("Com-cartao-SemAcompanhante")]
        public ActionResult<IEnumerable<Existencia_Card>> GetExistenciaCard_SemAcompanhante()
        {
            var existenciaCards = _existenciaCardRepository.GetExistenciaCard_SemAcompanhante();
            return Ok(existenciaCards);
        }

        [HttpGet("sem-cartao")]
        public ActionResult<IEnumerable<Existencia_Card>> GetInexistenciaCard()
        {
            var inexistenciaCards = _existenciaCardRepository.GetInexistenciaCard();
            return Ok(inexistenciaCards);
        }
        [HttpGet("Cartão- Bloqueado")]
        public ActionResult<IEnumerable<Existencia_Card>> GetBloqueados()
        {
            var inexistenciaCards = _existenciaCardRepository.GetBloqueados();
            return Ok(inexistenciaCards);
        }
    }
}
