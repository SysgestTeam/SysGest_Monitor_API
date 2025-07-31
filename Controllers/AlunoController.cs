using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
public class AlunoController : ControllerBase
{
    private readonly ILogger<AlunoController> _logger;
    private readonly IAlunoRepository _alunoRepository;

    public AlunoController(ILogger<AlunoController> logger, IAlunoRepository alunoRepository)
    {
        _logger = logger;
        _alunoRepository = alunoRepository;
    }


    [HttpGet("GetTodosAlunos")]
    [Authorize]
    public ActionResult<IEnumerable<Student>> GetAllStudents(int ano,  int status)
    {
        var alunos = _alunoRepository.GetAllStudents(ano, status);

        return Ok(alunos);
    }

    [HttpGet("GetTodosALunosComFotos")]
    [Authorize]
    public ActionResult<IEnumerable<Student>> GetAllCOmFotos(int ano, int status)
    {
        var alunos = _alunoRepository.GetAllStudentsWithPhoto(ano, status);

        return Ok(alunos);
    }

    [HttpGet("count-by-year-status")]
    [Authorize]
    public ActionResult<IEnumerable<StudentCount>> GetCoutnAllStudentByYear(int ano, int status)
    {
        var alunos = _alunoRepository.GetCoutnAllStudentByYear(ano, status);

        return Ok(alunos);
    }

    [HttpGet("confirmation-or-matriculation")]
    [Authorize]
    public ActionResult<IEnumerable<Student>> GetCoutnAllStudentConfirmationOrMatriculation(int ano, int batch)
    {
        var alunos = _alunoRepository.GetAllStudentsConfirmationORMatriculation(ano, batch);

        return Ok(alunos);
    }

    [HttpGet("courses")]
    public IEnumerable<Class> courses(int ano)
    {
        var alunos = _alunoRepository.GetAllClass(ano);
        return alunos;
    }

    [HttpGet("GetAluno")]
    public IEnumerable<TabAluno> Get()
    {
        var alunos = _alunoRepository.GetAlunos();
        return alunos;
    }

    [HttpGet("GetAluno-Sem-fotos")]
    public IEnumerable<Existencia_Card> GetSemFotos()
    {
        var alunos = _alunoRepository.GetAlunosSemFotos();
        return alunos;
    }

    [HttpGet("GetAluno-Sem-fotos-filtro")]
    public IEnumerable<Existencia_Card> GetSemFotosFiltro(int? idclasse = null, int? idturma = null)
    {
        var alunos = _alunoRepository.GetAlunosSemFotosFiltro(idclasse,idturma);
        return alunos;
    }

    [HttpGet("GetAluno-filtro")]
    public IEnumerable<TabAluno> GetFiltro(int? idclasse = null, int? idturma = null)
    {
        var alunos = _alunoRepository.GetAlunosFiltro(idclasse,idturma);
        return alunos;
    }

    [HttpGet("listar-as-classes")]
    public IEnumerable<Classes> GetClasses()
    {
        var classe = _alunoRepository.GetClasses();
        return classe;
    }

    [HttpGet("listar-as-turmas/{classe}")]
    public IEnumerable<Turmas> GetTurmas(int classe)
    {
        var turmas = _alunoRepository.GetTurmas(classe);
        return turmas;
    }
}
