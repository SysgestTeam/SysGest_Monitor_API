using System.Data;
using System.Data.SqlClient;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Repository
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly string _connectionString;

        public RelatorioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<TabAluno> GetAlunos()
        {
            List<TabAluno> alunos = new List<TabAluno>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Chame o procedimento armazenado
                string sqlQuery = "EXEC sp_SaidaEntrada";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TabAluno aluno = new TabAluno
                            {
                                // Mapeie as colunas do resultado para o objeto TabAluno
                                Nome = reader.GetString(0),
                                Hora = reader.GetString(1),
                                Descricao = reader.GetString(2),

                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                aluno.foto = (byte[])reader.GetValue(3);
                            }

                            alunos.Add(aluno);
                        }
                    }
                }
            }

            return alunos;
        }

        public IEnumerable<Relatorio> GetRelatorioPorIntervalo(string dataInicial, string dataFinal)
        {
            List<Relatorio> relatorios = new List<Relatorio>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                        connection.Open();
                        // Chame o procedimento armazenado

                        using (SqlCommand cmd = new SqlCommand("sp_relatorio", connection))
                        {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@dataInicial", dataInicial));
                        cmd.Parameters.Add(new SqlParameter("@dataFinal", dataFinal));


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Relatorio relatorio = new Relatorio
                                {
                                    IDCartao =  reader.GetInt32(reader.GetOrdinal("IDCartao")),
                                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                    Turma = reader.GetString(reader.GetOrdinal("Turma")),
                                    Data = reader.GetString(reader.GetOrdinal("Data")),
                                    Hora = reader.GetString(reader.GetOrdinal("Hora")),
                                    Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                    Torniquete = reader.GetString(reader.GetOrdinal("Torniquete")),
                                    Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                    Acompanhante = reader.GetString(reader.GetOrdinal("Acompanhante"))
                                };

                                relatorios.Add(relatorio);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Relatorio>();
            }

            return relatorios;
        }

    }
}
