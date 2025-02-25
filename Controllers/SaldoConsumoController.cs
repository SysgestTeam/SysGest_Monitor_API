using Microsoft.AspNetCore.Cors;
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
    [EnableCors("AllowAll")]

    public IEnumerable<SaldoConsumo> Get(int NumALuno) 
    {
            var alunos = _SaldoRepository.GetSaldoConsumo(NumALuno);
            return alunos;
    }

    [HttpGet("listar-todos-ticket")]
    [EnableCors("AllowAll")]
    public IEnumerable<Ticket> Get()
    {
        {
            var alunos = _SaldoRepository.List();
            return alunos;
        }
    }

    [HttpPost("lancamento-consumo")]
    [EnableCors("AllowAll")]
    public IActionResult LancarConsumo(DtoConsumo dto)
    {
        try
        {
            _SaldoRepository.Consumo(dto.numAluno, dto.usedValue);
            return Ok(new { mensagem = "Consumo registrado com sucesso." });
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


    [HttpPost]
    [EnableCors("AllowAll")]
    public IActionResult put(int idsaldo, bool apagado)
    {
        try
        {
            _SaldoRepository.RemoverSaldoETicket(idsaldo, apagado);
            return Ok(new { mensagem = "Ticket Actulizado com sucesso!" });
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


    [HttpGet("historico")]
    [EnableCors("AllowAll")]
    public IEnumerable<SaldoConsumo> get(int NumALuno)
    {
        var alunos = _SaldoRepository.GetHistóricoConsumo(NumALuno);

        return alunos;
    }

    [HttpGet("historico-by-id")]
    [EnableCors("AllowAll")]
    public IEnumerable<SaldoConsumo> getHistoricobyId(int id)
    {
        var alunos = _SaldoRepository.GetHistóricoConsumoById(id);

        return alunos;
    }

    [HttpGet("dashboard")]
    [EnableCors("AllowAll")]
    public IEnumerable<Dashboard> dashboard()
    {
        var alunos = _SaldoRepository.Dashboad();

        return alunos;
    }

    [HttpGet("calculo")]
    [EnableCors("AllowAll")]
    public IEnumerable<CalculoParaEstatistica> calculo()
    {
        var alunos = _SaldoRepository.CalculoParaEstatistica();

        return alunos;
    }


    [HttpGet("ticket")]
    [EnableCors("AllowAll")]
    public IActionResult Ticket(int NumAluno)
    {
        try
        {
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



    [HttpGet("ticket-todos")]
    [EnableCors("AllowAll")]
    public IActionResult Ticket()
    {
        try
        {
            var alunos = _SaldoRepository.ListTicket();
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


    [HttpGet("filtrar-tickets-por-data")]
    [EnableCors("AllowAll")]
    public IActionResult FiltrarTicketsPorData(DateTime? DataInicio = null, DateTime? DataFim = null)
    {
        try
        {
            var alunos = _SaldoRepository.FiltrarTicketsPorData(DataInicio,DataFim);
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