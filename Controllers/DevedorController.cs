using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
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

    [HttpPost("nao-ou-bloqueio-cartao")]
    public IActionResult NaoOuBloqueioEmMassa(int[] numAluno = null, int tipo = 1)
    {
        try
        {
            _DividasRepository.NaoOUBloqueioCartao(numAluno,tipo);
            return Ok(new { mensagem = "Operação realizado com sucesso."});
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

    [HttpGet("devedor-por-aluno")]
    public IActionResult DevedorPorAluno(int numAluno)
    {
        try
        {
            var alunos = _DividasRepository.GetDevedorPorAluno(numAluno);
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

    [HttpPost("Criar-Ou-Atualizar-ConfigBloqueio")]
    public void CriarOuAtualizarConfigBloqueio(bool APLICAR_MULTA, int DIA_MULTA, string horaBloqueio, int NUMERO_MESES_DIVIDA)
    {
        try
        {
            TimeOnly HORA_BLOQUEIO = TimeOnly.Parse(horaBloqueio);
            _DividasRepository.CriarOuAtualizarConfigBloqueio(APLICAR_MULTA, DIA_MULTA, HORA_BLOQUEIO, NUMERO_MESES_DIVIDA);
            Ok();
        }
        catch (FormatException)
        {
            BadRequest(new { mensagem = "Formato de hora inválido. Use HH:mm:ss." });
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

    [HttpGet("confi-bloqueio")]
    public IActionResult ConfigBloqueio()
    {
        try
        {
            var alunos = _DividasRepository.ObterTodasConfigBloqueio();
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