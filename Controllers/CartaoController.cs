using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartaoController : ControllerBase
    {
        private readonly ICartaoRepository _cartaoRepository;

        public CartaoController(ICartaoRepository cartaoRepository)
        {
            _cartaoRepository = cartaoRepository;
        }

        [HttpGet("encarregados")]
        [Authorize]
        public ActionResult<IEnumerable<Cartao>> GetEncarregados(string codigo)
        {
            var encarregados = _cartaoRepository.GetEncarregados(codigo);
            return Ok(encarregados);
        }

        [HttpGet("estudantes")]
        [Authorize]
        public ActionResult<IEnumerable<Cartao>> GetEstudantes(string codigo)
        {
            var estudantes = _cartaoRepository.GetEstudantes(codigo);
            return Ok(estudantes);
        }
    }
}
