using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
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


    [HttpGet("sem-fotos")]
    public IEnumerable<Funcionario> getFuncionarioSemFotos()
    {
        var alunos = _professor.GetProfessoresSemFoto();
        return alunos;
    }
    [HttpGet("com-fotos")]
    public IEnumerable<Funcionario> getFuncionarioComFotos()
    {
        var alunos = _professor.GetProfessoresComFotos();
        return alunos;
    }


    [HttpGet("com-cartao")]
    public IEnumerable<Funcionario> getProfessoresComCartao()
    {
        var alunos = _professor.GetComCartao();
        return alunos;
    }

    [HttpGet("sem-cartao")]
    public IEnumerable<Funcionario> getProfessorSemCartao()
    {
        var alunos = _professor.GetSemCartao();
        return alunos;
    }
}
