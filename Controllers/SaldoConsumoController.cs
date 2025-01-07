using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using SistemasdeTarefas.Repository;

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

    [HttpPost("lancamento-consumo")]
    public void post(DtoConsumo dto)
    {
        _SaldoRepository.Consumo(dto.numAluno, dto.usedValue);
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


    [HttpGet("ticket")]
    public IEnumerable<Ticket> ticket(int NumALuno)
    {
        var alunos = _SaldoRepository.ListTicket(NumALuno);

        return alunos;
    }
}