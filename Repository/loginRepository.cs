using Microsoft.IdentityModel.Tokens;
using SistemasdeTarefas.Interface;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using SistemasdeTarefas.Service;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Repository
{
    public class loginRepository : IloginRepository
    {
        private readonly string _connectionString;
        private readonly string _ApiKey;
        private readonly IAlunoRepository _alunoRepository;
        private readonly HttpClient _httpClient;
        public loginRepository(IConfiguration configuration, IAlunoRepository alunoRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _ApiKey = configuration.GetConnectionString("ApiKey");
            _alunoRepository = alunoRepository;
            _httpClient = new HttpClient();
        }
        public void CriarSenhaParaPai(string numero, string senha)
        {
            string senhaEncriptada = CryptoHelper.Encriptar(senha);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Primeiro: verificar se o número existe
                string sqlVerifica = "SELECT 1 FROM TABLOGINPAIS WHERE NumeroTelefone = @numero";

                using (SqlCommand cmdVerifica = new SqlCommand(sqlVerifica, conn))
                {
                    cmdVerifica.Parameters.AddWithValue("@numero", numero);
                    var resultado = cmdVerifica.ExecuteScalar();

                    if (resultado == null)
                    {
                        throw new Exception("Número de telefone não encontrado no sistema.");
                    }
                }

                // Segundo: atualizar a senha
                string sqlUpdate = @"
                        UPDATE TABLOGINPAIS
                        SET Senha = @senha, DataAlteracao = GETDATE()
                        WHERE NumeroTelefone = @numero";

                using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@senha", senhaEncriptada);
                    cmdUpdate.Parameters.AddWithValue("@numero", numero);
                    cmdUpdate.ExecuteNonQuery();
                }
            }
        }
        public IEnumerable<Login> ObterSenhaDesencriptada(string numeroTelefone)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT Senha, Nome FROM TABLOGINPAIS WHERE NumeroTelefone = @numero";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@numero", numeroTelefone);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string senhaEncriptada = reader["Senha"].ToString();
                            string nome = reader["Nome"].ToString();

                            string senhaDesencriptada = CryptoHelper.Desencriptar(senhaEncriptada);

                            return new List<Login>
                    {
                        new Login
                        {
                            user = nome,
                            numero = numeroTelefone,
                            senha = senhaDesencriptada
                        }
                    };
                        }
                    }
                }
            }

            return Enumerable.Empty<Login>();
        }
        public string ValidarCodigoVerificacao(int numeroTelefone, string codigoRecebido)
        {
            string resultadoValidacao = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @"
                        DECLARE @Numero VARCHAR(20) = @NumeroTelefone;
                        DECLARE @Codigo NVARCHAR(6) = @CodigoRecebido;
                        DECLARE @CodigoArmazenado NVARCHAR(6);
                        DECLARE @Expira DATETIME;
                        DECLARE @Resultado NVARCHAR(50);

                        SELECT 
                            @CodigoArmazenado = CodigoVerificacao,
                            @Expira = CodigoExpira
                        FROM TABLOGINPAIS
                        WHERE NumeroTelefone = @Numero;

                        IF @CodigoArmazenado IS NULL
                        BEGIN
                            SET @Resultado = 'Número não registado';
                        END
                        ELSE IF @Expira < GETDATE()
                        BEGIN
                            SET @Resultado = 'Código expirado';
                        END
                        ELSE IF @CodigoArmazenado = @Codigo
                        BEGIN
                            SET @Resultado = 'Código válido';

                            -- Opcional: invalidar o código após uso
                            UPDATE [TABLOGINPAIS]
                            SET CodigoVerificacao = NULL, CodigoExpira = NULL
                            WHERE NumeroTelefone = @Numero;
                        END
                        ELSE
                        BEGIN
                            SET @Resultado = 'Código incorreto';
                        END

                        SELECT @Resultado AS ResultadoValidacao;
                 ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NumeroTelefone", numeroTelefone.ToString());
                    cmd.Parameters.AddWithValue("@CodigoRecebido", codigoRecebido);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            resultadoValidacao = reader["ResultadoValidacao"].ToString();
                        }
                    }
                }
            }

            return resultadoValidacao;
        }
        public string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ChaveSuperSecreta12345678901234567890"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
             {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMonths(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<int> GerarNumerode6DigitosParaosPais(string nome, string numero, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(numero))
                throw new Exception("Número de telefone é obrigatório.");

            numero = numero.Replace(" ", "").Trim();
            int codigo = 0;
            bool podeEnviarSms = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sqlCheck = @"
                                    SELECT 1
                                    FROM 
                                        TABALUNOS 
                                        INNER JOIN TABMATRICULAS ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                                        INNER JOIN TABTURMAS ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA    
                                        INNER JOIN TABSTATUS s ON TABALUNOS.IDSTATUS = s.IDSTATUS 
                                    WHERE 
                                        TABALUNOS.INACTIVO = 0 
                                        AND TABMATRICULAS.IDSTATUS IN (2, 4) 
                                        AND TABTURMAS.NOME NOT IN ('FUNCIONÁRIO', 'DOCENTE')
                                        AND TABMATRICULAS.IDANOLECTIVO = (SELECT MAX(IDANO) FROM TABANOSLECTIVOS)
                                        AND (
                                            OITELFPAI = @NumeroTelefone 
                                            OR OITELFMAE = @NumeroTelefone 
                                            OR OITELFENCARG = @NumeroTelefone
                                        )
                                        AND( 
                                           UsaAppSync = 1
                                           OR  PaiUsaApp = 1
                                           OR MaeUsaApp = 1 
                                           OR  EncUsaApp = 1)";

                    bool numeroExiste;
                    using (SqlCommand cmd = new SqlCommand(sqlCheck, connection))
                    {
                        cmd.Parameters.AddWithValue("@NumeroTelefone", numero);
                        var result = await cmd.ExecuteScalarAsync();
                        numeroExiste = result != null;
                    }

                    if (!numeroExiste)
                        return 0; // <- Número não associado a aluno válido, retorna 0

                    // Agora sim gera o código
                    codigo = new Random().Next(100000, 999999);

                    string sqlUpsert = @"
                IF EXISTS (SELECT 1 FROM TABLOGINPAIS WHERE NumeroTelefone = @Numero)
                BEGIN
                    UPDATE TABLOGINPAIS
                    SET Nome = @Nome,
                        Email = @Email,
                        CodigoVerificacao = @Codigo,
                        CodigoExpira = DATEADD(MINUTE, 5, GETDATE()),
                        DataAlteracao = GETDATE()
                    WHERE NumeroTelefone = @Numero;
                END
                ELSE
                BEGIN
                    INSERT INTO TABLOGINPAIS (Nome, NumeroTelefone, Email, CodigoVerificacao, CodigoExpira, DataRegistada)
                    VALUES (@Nome, @Numero, @Email, @Codigo, DATEADD(MINUTE, 5, GETDATE()), GETDATE());
                END";

                    using (SqlCommand cmd = new SqlCommand(sqlUpsert, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", nome);
                        cmd.Parameters.AddWithValue("@Numero", numero);
                        cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Codigo", codigo.ToString());
                        await cmd.ExecuteNonQueryAsync();
                    }

                    podeEnviarSms = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO SMS] {ex.Message}");
                return -1; // <- erro inesperado
            }
            finally
            {
                if (podeEnviarSms && codigo > 0)
                {
                    await EnviarSmsAsync(nome, numero, codigo);
                }
            }

            return codigo; // <- só retorna o código se tudo correu bem
        }
        public async Task<int> GerarCodigoRecuperacaoSenhaAsync(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new Exception("Número de telefone é obrigatório.");

            numero = numero.Replace(" ", "").Trim();
            int codigo = 0;
            bool podeEnviarSms = false;
            string nome = "Responsável"; // valor padrão, caso não seja encontrado

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sqlCheck = @"
                SELECT Nome 
                FROM TABLOGINPAIS 
                WHERE NumeroTelefone = @Numero";

                    using (SqlCommand cmd = new SqlCommand(sqlCheck, connection))
                    {
                        cmd.Parameters.AddWithValue("@Numero", numero);
                        var result = await cmd.ExecuteScalarAsync();

                        if (result == null)
                            return 0; // Número não encontrado, nenhum código será enviado

                        nome = result?.ToString() ?? "Responsável";
                    }

                    // Gera o código aleatório de 6 dígitos
                    codigo = new Random().Next(100000, 999999);

                    string sqlUpdate = @"
                UPDATE TABLOGINPAIS
                SET CodigoVerificacao = @Codigo,
                    CodigoExpira = DATEADD(MINUTE, 5, GETDATE()),
                    DataAlteracao = GETDATE()
                WHERE NumeroTelefone = @Numero";

                    using (SqlCommand cmd = new SqlCommand(sqlUpdate, connection))
                    {
                        cmd.Parameters.AddWithValue("@Numero", numero);
                        cmd.Parameters.AddWithValue("@Codigo", codigo.ToString());
                        await cmd.ExecuteNonQueryAsync();
                    }

                    podeEnviarSms = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO RECUPERACAO] {ex.Message}");
                return -1;
            }
            finally
            {
                if (podeEnviarSms && codigo > 0)
                {
                    await EnviarSmsAsyncRe(nome, numero, codigo); // agora usa o nome real
                }
            }

            return codigo;
        }
        private async Task EnviarSmsAsyncRe(string nome, string numero, int codigo)
        {
            var payload = new
            {
                ApiKey = _ApiKey,
                Destino = new[] { numero },
                Mensagem = $"Olá, {nome}. Use este código para recuperar o acesso à sua conta: {codigo}. O código expira em 5 minutos.",
                CEspeciais = true
            };

            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.wesender.co.ao/envio/apikey", content);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro SMS: {response.StatusCode}, Detalhes: {responseBody}");
            }
        }
        private async Task EnviarSmsAsync(string nome, string numero, int codigo)
        {
            var payload = new
            {
                ApiKey = _ApiKey, 
                Destino = new[] { numero },
                Mensagem = $"Olá, {nome}. O seu código é: {codigo}",
                CEspeciais = true
            };

            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.wesender.co.ao/envio/apikey", content);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro SMS: {response.StatusCode}, Detalhes: {responseBody}");
            }
        }
        public string login(string user, string senha)
        {
            string resultadoLogin = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para verificar o login e obter a resposta como uma variável de saída
                string sqlQuery = @"
                    DECLARE @UserLogin NVARCHAR(50) = @User;
                    DECLARE @Password NVARCHAR(50) = @Senha;
                    DECLARE @SaltPW NVARCHAR(36);
                    DECLARE @PassHSArmazenado VARBINARY(64);
                    DECLARE @PassHSInserido VARBINARY(64);
                    DECLARE @Resultado NVARCHAR(50);

                    -- Busca Salt e Hash armazenados para o usuário
                    SELECT 
                        @SaltPW = CAST(SaltPW AS NVARCHAR(36)), 
                        @PassHSArmazenado = PassHS
                    FROM [TABUSER]
                    WHERE [USERLOGIN] = @UserLogin;

                    -- Verificação da senha
                    IF @SaltPW IS NOT NULL AND @PassHSArmazenado IS NOT NULL
                    BEGIN
                        SET @PassHSInserido = HASHBYTES('SHA2_512', @Password + @SaltPW);

                        IF @PassHSInserido = @PassHSArmazenado
                            SET @Resultado = 'Usuário e senha corretos';
                        ELSE
                            SET @Resultado = 'Senha incorreta';
                    END
                    ELSE
                        SET @Resultado = 'Usuário não encontrado';

                    SELECT @Resultado AS ResultadoLogin;
                 ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@User", user);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            resultadoLogin = reader["ResultadoLogin"].ToString();
                        }
                    }
                }
            }

            return resultadoLogin;
        }
        public async Task<int> GetIdUserAsync(string user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT IDUSER FROM TABUSER WHERE USERLOGIN = @userLogin";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userLogin", user);

                    object result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);

                    return 0; // caso não encontre
                }
            }
        }

    }
}
