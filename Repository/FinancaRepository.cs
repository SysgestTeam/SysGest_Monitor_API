using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using System.Data.SqlClient;

namespace SistemasdeTarefas.Repository
{
    public class FinancaRepository : IFinancaRepository
    {
        private readonly string _connectionString;

        public FinancaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<AlunoDossier>> ListAlunoDossier(int number)
        {
            var alunos = new List<AlunoDossier>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @$"
                SELECT 
                    IdAlunoDossier,
                    IdAluno,
                    NumAluno,
                    Nome,
                    IdCustomer,
                    IdStatusAluno,
                    IdAno,
                    Ano,
                    IdBolsa,
                    IdMatricula,
                    IdCiclo,
                    NomeCiclo,
                    IdClasse,
                    NomeClasse
                FROM AlunoDossier
                WHERE NumAluno = {number}";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            alunos.Add(new AlunoDossier
                            {
                                IdAlunoDossier = reader.GetInt32(0),
                                IdAluno = reader.GetInt32(1),
                                NumAluno = reader.GetInt32(2),
                                Nome = reader.GetString(3),
                                IdCustomer = reader.GetInt32(4),
                                IdStatusAluno = reader.GetInt32(5),
                                IdAno = reader.GetInt32(6),
                                Ano = reader.GetInt32(7),
                                IdBolsa = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                                IdMatricula = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                                IdCiclo = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                                NomeCiclo = reader.IsDBNull(11) ? null : reader.GetString(11),
                                IdClasse = reader.IsDBNull(12) ? null : reader.GetInt32(12),
                                NomeClasse = reader.IsDBNull(13) ? null : reader.GetString(13),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar AlunoDossier.", ex);
            }

            return alunos;
        }


    }
}
