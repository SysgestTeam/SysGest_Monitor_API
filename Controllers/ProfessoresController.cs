using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
public class ProfessoresController : ControllerBase
{
    private readonly ILogger<FuncionarioController> _logger;
    private readonly IProfessoresRepository _professor;

    public ProfessoresController(ILogger<FuncionarioController> logger, IProfessoresRepository professor)
    {
        _logger = logger;
        _professor = professor;
    }

    [HttpGet("GetProfessores")]
    public IEnumerable<Funcionario> Get()
    {
        var alunos = _professor.GetAll();
        return alunos;
    }

}
