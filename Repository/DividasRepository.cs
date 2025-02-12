using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemasdeTarefas.Repository
{
    public class DividasRepository : IDividasRepository
    {
        private readonly string _connectionString;

        public DividasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IEnumerable<Devedor> GetDevedores(DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            List<Devedor> alunos = new List<Devedor>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

   
                // Chame o procedimento armazenado

                using (SqlCommand cmd = new SqlCommand("SP_DEVEDORES", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@DATAINI", SqlDbType.DateTime) { Value = dataInicial });
                    cmd.Parameters.Add(new SqlParameter("@DATAFIM", SqlDbType.DateTime) { Value = dataFinal });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Devedor aluno = new Devedor
                            { 
                                IdAluno = reader.GetInt32(1),
                                Nome = reader.GetString(3),
                                NumAluno =  reader.GetInt32(4),
                                Contrato = reader.GetString(5),
                                Valor = reader.GetDecimal(6),
                                DataLimite = reader.GetDateTime(7),
                                Mes = reader.GetString(8),
                                Estado = reader.GetString(9),
                                Ano = reader.GetInt32(15),
                                Familia = reader.GetString(16),
                                Bloqueado = reader.GetBoolean(28)
                            };

                            alunos.Add(aluno);
                        }
                    }
                }
            }

            return alunos;
        }
    }
}
