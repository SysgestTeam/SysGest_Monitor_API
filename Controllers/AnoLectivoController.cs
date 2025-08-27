using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
public class AnoLectivoController : ControllerBase
{
    private readonly ILogger<AnoLectivoController> _logger;
    private readonly IAnoLectivo _anoRepository;

    public AnoLectivoController(ILogger<AnoLectivoController> logger, IAnoLectivo anoRepository)
    {
        _logger = logger;
        _anoRepository = anoRepository;
    }


    [HttpGet]
    [EnableCors("AllowAll")]
    public ActionResult<IEnumerable<Student>> GetAnoLectivo()
    {
        var alunos = _anoRepository.GetAnoLectivo();

        return Ok(alunos);
    }

}
