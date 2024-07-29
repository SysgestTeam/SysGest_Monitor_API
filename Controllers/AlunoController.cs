using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using SistemasdeTarefas.Repository;

[ApiController]
[Route("[controller]")]
public class AlunoController : ControllerBase
{
    private readonly ILogger<AlunoController> _logger;
    private readonly IAlunoRepository _alunoRepository;

    public AlunoController(ILogger<AlunoController> logger, IAlunoRepository alunoRepository)
    {
        _logger = logger;
        _alunoRepository = alunoRepository;
    }

    [HttpGet(Name = "GetAluno")]
    public IEnumerable<TabAluno> Get()
    {
        var alunos = _alunoRepository.GetAlunos();
        return alunos;
    }
}
