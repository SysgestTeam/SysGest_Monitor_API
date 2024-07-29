using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SistemasdeTarefas.Models;
using System.Data;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamadaController : ControllerBase
    {
        private readonly string _connectionString; // Conexão com o banco de dados

        public ChamadaController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("ChamadaByTurmas")]
        public ActionResult<IEnumerable<Chamada>> GetByTurmas(string Turmas)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("sp_CatracaChamadaAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Adicione parâmetros, se necessário
                        // command.Parameters.AddWithValue("@parametro", valor);

                        using (var reader = command.ExecuteReader())
                        {
                            var chamadas = new List<Chamada>();

                            while (reader.Read())
                            {
                                var chamada = new Chamada
                                {
                                    nome = reader["nome"].ToString(),
                                    hora = reader["hora"].ToString(),
                                    descricao = reader["descricao"].ToString(),
                                    turma = reader["turma"].ToString(),
                                    registo = reader["registo"].ToString()
                                };
                                if (reader["foto"] != DBNull.Value)
                                {
                                    chamada.foto = (byte[])reader["foto"];
                                }
                                chamadas.Add(chamada);
                            }

                            if (chamadas.Any())
                            {
                                if (!string.IsNullOrEmpty(Turmas))
                                {
                                    // Divide a string de Turmas em uma lista
                                    List<string> turmaList = Turmas.Split(',').ToList();

                                    var chamadasFiltradas = chamadas
                                        .Where(c => turmaList.Contains(c.turma))
                                        .ToList();

                                    return Ok(chamadasFiltradas);
                                }
                                else
                                {
                                    return Ok(chamadas); // Retorna todas as chamadas se Turmas estiver vazio
                                }
                            }
                            else
                            {
                                return NotFound(); // Procedimento não retornou dados
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Lide com erros, registre-os ou retorne um código de status de erro apropriado.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao executar o procedimento: {ex.Message}");
            }
        }

        [HttpGet("GetByTurmasAll")]
        public ActionResult<IEnumerable<Chamada>> GetByTurmasAll()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("sp_CatracaChamadaAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Adicione parâmetros, se necessário
                        // command.Parameters.AddWithValue("@parametro", valor);

                        using (var reader = command.ExecuteReader())
                        {
                            var chamadas = new List<Chamada>();

                            while (reader.Read())
                            {
                                var chamada = new Chamada
                                {
                                    nome = reader["nome"].ToString(),
                                    hora = reader["hora"].ToString(),
                                    descricao = reader["descricao"].ToString(),
                                    turma = reader["turma"].ToString(),
                                    registo = reader["registo"].ToString()
                                };
                                if (reader["foto"] != DBNull.Value)
                                {
                                    chamada.foto = (byte[])reader["foto"];
                                }
                                chamadas.Add(chamada);
                            }
                            return Ok(chamadas); // Retorna todas as chamadas se Turmas estiver vazio

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Lide com erros, registre-os ou retorne um código de status de erro apropriado.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao executar o procedimento: {ex.Message}");
            }
        }
        [HttpGet("GetByTurmasParametros")]
        public ActionResult<IEnumerable<Chamada>> GetByTurmasParametros(string Turmas)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Defina o procedimento armazenado com base no valor de Turmas
                        switch (Turmas)
                        {
                            case "piso1":
                                command.CommandText = "sp_CatracaChamada_Tela1";
                                break;

                            case "piso2":
                                command.CommandText = "sp_CatracaChamada_Tela2";
                                break;
                            case "piso3":
                                command.CommandText = "sp_CatracaChamada_Tela3";
                                break;
                            case "piso4":
                                command.CommandText = "sp_CatracaChamadaAll";
                                break;

                            // Adicione outros casos conforme necessário
                            default:
                                return BadRequest("Parâmetro Turmas inválido");
                        }

                        using (var reader = command.ExecuteReader())
                        {
                            var chamadas = new List<Chamada>();

                            while (reader.Read())
                            {
                                var chamada = new Chamada
                                {
                                    nome = reader["nome"].ToString(),
                                    hora = reader["hora"].ToString(),
                                    descricao = reader["descricao"].ToString(),
                                    turma = reader["turma"].ToString(),
                                    registo = reader["registo"].ToString()
                                };
                                if (reader["foto"] != DBNull.Value)
                                {
                                    chamada.foto = (byte[])reader["foto"];
                                }
                                chamadas.Add(chamada);
                            }

                            return Ok(chamadas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Trate exceções conforme necessário
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }



    }
}
