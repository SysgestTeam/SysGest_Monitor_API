using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

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
    [Authorize]
    public IEnumerable<Relatorio> Get(string dataInicial, string dataFinal)
    {
        var resultado = _relatorio.GetRelatorioPorIntervalo(dataInicial, dataFinal);
        return resultado;
    }
}
