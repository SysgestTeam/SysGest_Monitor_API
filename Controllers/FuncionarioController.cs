using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
public class FuncionarioController : ControllerBase
{
    private readonly ILogger<FuncionarioController> _logger;
    private readonly IFuncionarioRepository _funcionario;

    public FuncionarioController(ILogger<FuncionarioController> logger, IFuncionarioRepository funcionario)
    {
        _logger = logger;
        _funcionario = funcionario;
    }

    [HttpGet("GetFuncionario")]
    public IEnumerable<Funcionario> Get()
    {
        var alunos = _funcionario.GetAll();
        return alunos;
    }

}
