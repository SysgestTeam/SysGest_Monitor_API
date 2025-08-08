using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<AlunoController> _logger;
    private readonly IloginRepository _loginRepository;
    private readonly IAlunoRepository _alunoRepository;

    public LoginController(ILogger<AlunoController> logger, IloginRepository loginRepository, IAlunoRepository alunoRepository)
    {
        _logger = logger;
        _loginRepository = loginRepository;
        _alunoRepository = alunoRepository;
    }


    [HttpPost]
    [EnableCors("AllowAll")]
    public string post(Login user)
    {
        var token = string.Empty;
        var login = _loginRepository.login(user.user, user.senha);

        if (login == "Usu�rio e senha corretos")
        {

            token = _loginRepository.GenerateJwtToken(user.user);
            return token;
        }

        return token;
    }

    [HttpPost("verificar-numero-envia-codigo")]
    [EnableCors("AllowAll")]
    public async Task<IActionResult> Post([FromBody] EnvioCodigoDTO dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Numero))
            {
                return BadRequest(new { Sucesso = false, Erro = "Nome e n�mero de telefone s�o obrigat�rios." });
            }

            int codigo = await _loginRepository.GerarNumerode6DigitosParaosPais(dto.Nome, dto.Numero, dto.Email);

            if (codigo == 0)
            {
                return BadRequest(new
                {
                    Sucesso = false,
                    Erro = "O n�mero de telefone n�o est� associado a nenhum aluno ativo."
                });
            }

            if (codigo == -1)
            {
                return StatusCode(500, new
                {
                    Sucesso = false,
                    Erro = "Ocorreu um erro interno ao gerar o c�digo. Tente novamente mais tarde."
                });
            }

            return Ok(new
            {
                Sucesso = true,
                Mensagem = "C�digo enviado com sucesso.",
                CodigoVerificacao = codigo
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Sucesso = false,
                Erro = $"Erro inesperado: {ex.Message}"
            });
        }
    }
    [HttpPost("validar-codigo")]
    [EnableCors("AllowAll")]
    public IActionResult PostValidarCodigo(int numeroTelefone, string codigoRecebido)
    {
        try
        {
            string resultado = _loginRepository.ValidarCodigoVerificacao(numeroTelefone, codigoRecebido);

            return Ok(new { resultado });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
    }


    [HttpPost("Criar-senha-para-pai")]
    [EnableCors("AllowAll")]
    public IActionResult CriarSenha([FromQuery] string numeroTelefone, [FromQuery] string senha)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(numeroTelefone) || string.IsNullOrWhiteSpace(senha))
            {
                return BadRequest(new
                {
                    Sucesso = false,
                    Mensagem = "N�mero de telefone e senha s�o obrigat�rios."
                });
            }

            if (!int.TryParse(numeroTelefone, out int numeroConvertido))
            {
                return BadRequest(new
                {
                    Sucesso = false,
                    Mensagem = "O n�mero de telefone deve conter apenas d�gitos."
                });
            }

            var numeroExiste = _alunoRepository.GetAlunosPorNumeroTelefonePai(numeroConvertido);

            if (!numeroExiste.Any())
            {
                return NotFound(new
                {
                    Sucesso = false,
                    Mensagem = "N�mero de telefone n�o encontrado na base de alunos."
                });
            }

            _loginRepository.CriarSenhaParaPai(numeroTelefone, senha);

            return Ok(new
            {
                Sucesso = true,
                Mensagem = "Senha criada."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Sucesso = false,
                Mensagem = "Ocorreu um erro ao processar a solicita��o.",
                Erro = ex.Message
            });
        }
    }


    [HttpPost("obter-senha-desencriptada")]
    [EnableCors("AllowAll")]
    public IActionResult ObterSenhaDesencriptada(string numeroTelefone)
    {
        try
        {
            IEnumerable<Login> resultado = _loginRepository.ObterSenhaDesencriptada(numeroTelefone);

            return Ok(new { resultado });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
    }


}
