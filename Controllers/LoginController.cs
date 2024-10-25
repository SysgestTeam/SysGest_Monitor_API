using Microsoft.AspNetCore.Mvc;
using SistemasdeTarefas.Interface;

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

}
