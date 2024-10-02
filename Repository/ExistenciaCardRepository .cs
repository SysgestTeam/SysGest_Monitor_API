using System.Data;
using System.Data.SqlClient;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Repository
{
    public class ExistenciaCardRepository : IExistenciaCardRepository
    {

        private readonly string _connectionString;

        public ExistenciaCardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Existencia_Card> GetExistenciaCard()
        {
            List<Existencia_Card> existenciaCards = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "sp_CatracaAlunosComCartao"; // Chame o procedimento armazenado

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card card = new Existencia_Card
                            {
                                NumAluno = reader.GetInt32(0), // Coluna do tipo int (substitua pelo tipo correto se necessário)
                                Nome = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };

                            existenciaCards.Add(card);
                        }
                    }
                }
            }

            return existenciaCards;
        }
        public IEnumerable<Existencia_Card> GetExistenciaCard_SemAcompanhante()
        {
            List<Existencia_Card> existenciaCards = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "sp_CatracaAlunosComCartaoSemAcompanhante"; // Chame o procedimento armazenado

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card card = new Existencia_Card
                            {
                                NumAluno = reader.GetInt32(0), // Coluna do tipo int (substitua pelo tipo correto se necessário)
                                Nome = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };

                            existenciaCards.Add(card);
                        }
                    }
                }
            }

            return existenciaCards;
        }
        public IEnumerable<Existencia_Card> GetInexistenciaCard()
        {
            List<Existencia_Card> inexistenciaCards = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Nome do procedimento armazenado
                string storedProcedureName = "sp_CatracaAlunosSemCartao";

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card card = new Existencia_Card
                            {
                                NumAluno = reader.GetInt32(0), // Coluna do tipo int (substitua pelo tipo correto se necessário)
                                Nome = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };

                            inexistenciaCards.Add(card);
                        }
                    }
                }
            }

            return inexistenciaCards;
        }
        public IEnumerable<Existencia_Card> GetBloqueados()
        {
            List<Existencia_Card> inexistenciaCards = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Nome do procedimento armazenado
                string storedProcedureName = "sp_CatracaAlunosComCartaoBloqueado";

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card card = new Existencia_Card
                            {
                                NumAluno = reader.GetInt32(0), // Coluna do tipo int (substitua pelo tipo correto se necessário)
                                Nome = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };

                            inexistenciaCards.Add(card);
                        }
                    }
                }
            }

            return inexistenciaCards;
        }

    }
}
