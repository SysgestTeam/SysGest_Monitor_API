using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;

[ApiController]
[Route("api/[controller]")]
public class DevedorController : ControllerBase
{
    private readonly ILogger<DevedorController> _logger;
    private readonly IDividasRepository _DividasRepository;

    public DevedorController(ILogger<DevedorController> logger, IDividasRepository DividasRepository)
    {
        _logger = logger;
        _DividasRepository = DividasRepository;
    }



    [HttpGet()]
    [EnableCors("AllowAll")]
    public IActionResult GetDevedores(DateTime? DataInicio = null, DateTime? DataFim = null)
    {
        try
        {
            var alunos = _DividasRepository.GetDevedores(DataInicio,DataFim);
            return Ok(alunos);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }



    [HttpPost("bloqueio-cartao")]
    [EnableCors("AllowAll")]
    public IActionResult BloqueioEmMassa(int[] numAluno, bool emMassa = false)
    {
        try
        {
            int linhasAfetadas = _DividasRepository.BloqueioCartao(numAluno, emMassa);
            return Ok(new { mensagem = "Bloqueio realizado com sucesso.", totalLinhasAfetadas = linhasAfetadas });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }



    [HttpPost("desbloqueio-cartao")]
    [EnableCors("AllowAll")]
    public void desbloqueio(int[] numAluno)
    {
        try
        {
            _DividasRepository.DesbloqueioCartao(numAluno);
            Ok();
        }
        catch (ApplicationException ex)
        {
            BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }

    [HttpPost("bloqueio-cartao-por-mes")]
    [EnableCors("AllowAll")]
    public IActionResult BloquearDevedoresPorMes(DateTime dataInicial, DateTime dataFinal)
    {
        try
        {
            int linhasAfetadas = _DividasRepository.BloquearDevedoresPorMes(dataInicial, dataFinal);
            return Ok(new { mensagem = "Bloqueio realizado com sucesso.", totalLinhasAfetadas = linhasAfetadas });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }


    [HttpPost("log")]
    [EnableCors("AllowAll")]
    public void LogBloqueio(int IsAluno, int IdEntidade, string TipoBloqueio, string AcaoBloqueio)
    {
        try
        {
            _DividasRepository.LogBloqueio(IsAluno, IdEntidade, TipoBloqueio, AcaoBloqueio);
            Ok();
        }
        catch (ApplicationException ex)
        {
            BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }


    [HttpGet("log-bloqueio")]
    [EnableCors("AllowAll")]
    public IActionResult LogBloqueio(DateTime? dataInicial, DateTime? dataFinal)
    {
        try
        {
            var alunos = _DividasRepository.LogBloqueio(dataInicial, dataFinal);
            return Ok(alunos);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }

}