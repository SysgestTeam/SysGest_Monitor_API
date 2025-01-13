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
    public void post(DtoConsumo dto)
    {
        _SaldoRepository.Consumo(dto.numAluno, dto.usedValue);
    }

    [HttpGet("historico")]
    public IEnumerable<SaldoConsumo> get(int NumALuno)
    {
        var alunos = _SaldoRepository.GetHist�ricoConsumo(NumALuno);

        return alunos;
    }


    [HttpGet("historico-by-id")]
    public IEnumerable<SaldoConsumo> getHistoricobyId(int id)
    {
        var alunos = _SaldoRepository.GetHist�ricoConsumoById(id);

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

        // Retornar os tickets no formato JSON
        return Ok(alunos);
    }
    catch (ApplicationException ex)
    {
        // Erro espec�fico da aplica��o, retornando com detalhes
        return BadRequest(new { mensagem = ex.Message });
    }
    catch (Exception ex)
    {
        // Erro gen�rico, logando detalhes e retornando mensagem gen�rica
        // Logger.LogError(ex, "Erro ao obter tickets para o aluno.");
        return StatusCode(500, new { mensagem = "Erro interno ao processar a solicita��o." });
    }
}

}