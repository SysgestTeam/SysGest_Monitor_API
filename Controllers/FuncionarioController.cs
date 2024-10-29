using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public IEnumerable<Funcionario> Get()
    {
        var alunos = _funcionario.GetAll();
        return alunos;
    }

    [HttpGet("sem-fotos")]
    [Authorize]
    public IEnumerable<Funcionario> getFuncionarioSemFotos()
    {
        var alunos = _funcionario.GetFuncionarioSemFoto();
        return alunos;
    }


    [HttpGet("com-fotos")]
    [Authorize]
    public IEnumerable<Funcionario> getFuncionarioComFotos()
    {
        var alunos = _funcionario.GetFuncionarioComFotos();
        return alunos;
    }

    [HttpGet("com-cartao")]
    [Authorize]
    public IEnumerable<Funcionario> getFuncionarioComCartao()
    {
        var alunos = _funcionario.GetComCartao();
        return alunos;
    }

    [HttpGet("sem-cartao")]
    [Authorize]
    public IEnumerable<Funcionario> getFuncionarioSemCartao()
    {
        var alunos = _funcionario.GetSemCartao();
        return alunos;
    }

}
