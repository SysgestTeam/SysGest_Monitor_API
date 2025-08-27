using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
public class FamiliaArtigoController : ControllerBase
{
    private readonly ILogger<FamiliaArtigoController> _logger;
    private readonly IFamiliaArtigoRepository _repository;

    public FamiliaArtigoController(ILogger<FamiliaArtigoController> logger, IFamiliaArtigoRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    #region FAMÍLIA

    [HttpPost("criar-familia")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> CreateFamily([FromBody] FamiliaDTO familia)
    {
        await _repository.CreatFamily(familia);
        return Ok(new { message = "Família criada com sucesso!" });
    }

    [HttpGet("listar-familias")]
    [EnableCors("AllowAll")]
    public async Task<ActionResult<IEnumerable<Familia>>> ListFamily()
    {
        var result = await _repository.ListFamily();
        return Ok(result);
    }

    [HttpGet("buscar-familia/{id}")]
    [EnableCors("AllowAll")]
    public async Task<ActionResult<IEnumerable<Familia>>> GetFamilyById(int id)
    {
        var result = await _repository.FindByIdAFamily(id);
        return Ok(result);
    }

    [HttpPut("atualizar-familia")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> UpdateFamily([FromBody] Familia familia)
    {
        await _repository.UpdateFamily(familia);
        return Ok(new { message = "Família atualizada com sucesso!" });
    }

    [HttpDelete("apagar-familia/{id}")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> DeleteFamily(int id)
    {
        await _repository.DeleteFamily(id);
        return Ok(new { message = "Família excluída com sucesso!" });
    }

    #endregion

    #region ARTIGO

    [HttpPost("criar-artigo")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> CreateArtigo([FromBody] ArtigoDTO artigo)
    {
        await _repository.CreatArtigo(artigo);
        return Ok(new { message = "Artigo criado com sucesso!" });
    }

    [HttpGet("listar-artigos")]

    [EnableCors("AllowAll")]
    public async Task<ActionResult<IEnumerable<Artigo>>> ListArtigo()
    {

        var result = await _repository.ListArtigo();
        return Ok(result);
    }

    [HttpGet("buscar-artigo/{id}")]

    [EnableCors("AllowAll")]
    public async Task<ActionResult<IEnumerable<Artigo>>> GetArtigoById(int id)
    {
        var result = await _repository.FindByIdArtigo(id);
        return Ok(result);
    }

    [HttpPut("atualizar-artigo")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> UpdateArtigo([FromBody] ArtigoDTOUPDATE artigo)
    {
        await _repository.UpdateArtigo(artigo);
        return Ok(new { message = "Artigo atualizado com sucesso!" });
    }

    [HttpDelete("apagar-artigo/{id}")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> DeleteArtigo(int id)
    {
        await _repository.DeleteArtigo(id);
        return Ok(new { message = "Artigo excluído com sucesso!" });
    }

    #endregion
}
