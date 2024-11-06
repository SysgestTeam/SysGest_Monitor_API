using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<AlunoController> _logger;
    private readonly IloginRepository _loginRepository;

    public LoginController(ILogger<AlunoController> logger, IloginRepository loginRepository)
    {
        _logger = logger;
        _loginRepository = loginRepository;
    }


    [HttpPost]
    public string post(Login user)
    {
        var token = string.Empty;
        var login = _loginRepository.login(user.user, user.senha);

        if (login == "Usuário e senha corretos")
        {

            token = _loginRepository.GenerateJwtToken(user.user);
            return token;
        }

        return token;
    }
}
