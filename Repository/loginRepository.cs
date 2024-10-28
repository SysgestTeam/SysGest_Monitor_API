using Microsoft.IdentityModel.Tokens;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemasdeTarefas.Repository
{
    public class loginRepository : IloginRepository
    {

        private readonly string _connectionString;

        public loginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
    }
}
