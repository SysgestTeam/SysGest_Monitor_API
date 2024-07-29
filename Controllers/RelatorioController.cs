using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using SistemasdeTarefas.Repository;

[ApiController]
[Route("api/[controller]")]
public class RelatorioController : ControllerBase
{
    private readonly ILogger<RelatorioController> _logger;
    private readonly IRelatorioRepository _relatorio;

    public RelatorioController(ILogger<RelatorioController> logger, IRelatorioRepository relatorio)
    {
        _logger = logger;
        _relatorio = relatorio;
    }

    [HttpGet(Name = "GetRelatorioIntervalo")]
    public IEnumerable<Relatorio> Get(string dataInicial, string dataFinal)
    {
        var resultado = _relatorio.GetRelatorioPorIntervalo(dataInicial, dataFinal);
        return resultado;
    }
}