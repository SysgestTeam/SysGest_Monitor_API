using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
public class DevedorController : ControllerBase
{
    private readonly ILogger<DevedorController> _logger;
    private readonly IDividasRepository _DividasRepository;

    public DevedorController(ILogger<DevedorController> logger, IDividasRepository DividasRepository)
    {
        _logger = logger;
        _DividasRepository = DividasRepository;
    }



    [HttpGet()]
    [EnableCors("AllowAll")]
    public IActionResult GetDevedores(DateTime? DataInicio = null, DateTime? DataFim = null)
    {
        try
        {
            var alunos = _DividasRepository.GetDevedores(DataInicio,DataFim);
            return Ok(alunos);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno ao processar a solicitação." });
        }
    }



}