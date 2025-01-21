using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
public class SaldoConsumoController : ControllerBase
{
    private readonly ILogger<SaldoConsumoController> _logger;
    private readonly ISaldoConsumo _SaldoRepository;

    public SaldoConsumoController(ILogger<SaldoConsumoController> logger, ISaldoConsumo saldoRepository)
    {
        _logger = logger;
        _SaldoRepository = saldoRepository;
    }

    [HttpGet]
    public IEnumerable<SaldoConsumo> Get(int NumALuno) {
        {
            var alunos = _SaldoRepository.GetSaldoConsumo(NumALuno);
            return alunos;
        }

    }

    [HttpGet("listar-todos-ticket")]
    public IEnumerable<Ticket> Get()
    {
        {
            var alunos = _SaldoRepository.List();
            return alunos;
        }
    }

    [HttpPost("lancamento-consumo")]
    public IActionResult LancarConsumo(DtoConsumo dto)
    {
        try
        {
            _SaldoRepository.Consumo(dto.numAluno, dto.usedValue);
            return Ok(new { mensagem = "Consumo registrado com sucesso." });
        }
        catch (ApplicationException ex)
        {
            // Retorna um BadRequest com a mensagem de erro específica
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            // Logar o erro antes de retornar a resposta
            // _logger.LogError(ex, "Erro interno ao processar a solicitação.");
            return StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }


    [HttpDelete]
    public IActionResult put(int idsaldo, bool apagado)
    {
        try
        {
            _SaldoRepository.RemoverSaldoEEliminarTicket(idsaldo, apagado);
            return Ok(new { mensagem = "Ticket Actulizado com sucesso!" });
        }
        catch (ApplicationException ex)
        {
            // Retorna um BadRequest com a mensagem de erro específica
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            // Logar o erro antes de retornar a resposta
            // _logger.LogError(ex, "Erro interno ao processar a solicitação.");
            return StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }


    [HttpGet("historico")]
    public IEnumerable<SaldoConsumo> get(int NumALuno)
    {
        var alunos = _SaldoRepository.GetHistóricoConsumo(NumALuno);

        return alunos;
    }


    [HttpGet("historico-by-id")]
    public IEnumerable<SaldoConsumo> getHistoricobyId(int id)
    {
        var alunos = _SaldoRepository.GetHistóricoConsumoById(id);

        return alunos;
    }

    [HttpGet("dashboard")]
    public IEnumerable<Dashboard> dashboard()
    {
        var alunos = _SaldoRepository.Dashboad();

        return alunos;
    }

    [HttpGet("calculo")]
    public IEnumerable<CalculoParaEstatistica> calculo()
    {
        var alunos = _SaldoRepository.CalculoParaEstatistica();

        return alunos;
    }


    [HttpGet("ticket")]
    public IActionResult Ticket(int NumAluno)
{
    try
    {
        // Obter a lista de tickets
        var alunos = _SaldoRepository.ListTicket(NumAluno);
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