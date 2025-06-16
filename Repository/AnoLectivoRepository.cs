using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using System.Data.SqlClient;

namespace SistemasdeTarefas.Repository
{
    public class AnoLectivoRepository : IAnoLectivo
    {
        private readonly string _connectionString;
        public AnoLectivoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<AnoLectivo> GetAnoLectivo()
        {
            List<AnoLectivo> anos = new List<AnoLectivo>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @$"SELECT * FROM TABANOSLECTIVOS";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AnoLectivo ano = new AnoLectivo
                            {
                                ANO = reader.IsDBNull(reader.GetOrdinal("ANO")) ? 0 : reader.GetInt32(reader.GetOrdinal("ANO")),
                                IDANO = reader.IsDBNull(reader.GetOrdinal("IDANO")) ? 0 : reader.GetInt32(reader.GetOrdinal("IDANO")),
                            };

                            anos.Add(ano);
                        }
                    }
                }
            }

            return anos;
        }
    }
}
