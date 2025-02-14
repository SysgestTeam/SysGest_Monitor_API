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

        public void BloqueioCartao(int[] numAluno = null, bool emMassa = false)
        {
            if (numAluno == null && !emMassa)
            {
                throw new ArgumentException("A lista de alunos não pode ser nula, a menos que seja um bloqueio em massa.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Obtém o ID do último ano letivo
                    string anoQuery = "SELECT MAX(IDANO) FROM TABANOSLECTIVOS";
                    int idAno;
                    using (SqlCommand anoCmd = new SqlCommand(anoQuery, connection))
                    {
                        idAno = (int)anoCmd.ExecuteScalar();
                    }

                    // Inicia a transação
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Se for em massa, bloqueia todos os alunos
                            if (emMassa)
                            {
                                string queryEmMassa = "UPDATE CartaoAluno SET Bloqueado = 1 WHERE IdAno = @idAno";
                                using (SqlCommand cmd = new SqlCommand(queryEmMassa, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@idAno", idAno);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Se emMassa for false, garante que a lista tenha pelo menos 1 aluno
                                if (numAluno.Length == 0)
                                {
                                    throw new ArgumentException("A lista de alunos não pode estar vazia.");
                                }

                                // Divide os alunos em lotes de 1000 para evitar problemas com IN
                                int chunkSize = 1000;
                                for (int i = 0; i < numAluno.Length; i += chunkSize)
                                {
                                    var chunk = numAluno.Skip(i).Take(chunkSize);
                                    string alunoList = string.Join(",", chunk);

                                    if (string.IsNullOrEmpty(alunoList))
                                        continue;

                                    string query = @$"
                                UPDATE CartaoAluno 
                                SET Bloqueado = 1 
                                WHERE NumAluno IN ({alunoList}) AND IdAno = @idAno";

                                    using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@idAno", idAno);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            // Commit da transação
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Caso haja erro, faz o rollback
                            transaction.Rollback();
                            throw new ApplicationException("Erro ao bloquear os cartões. Operação revertida.", ex);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                var errorMessage = "Erro ao executar o bloqueio dos cartões. Detalhes do erro SQL:";
                foreach (SqlError error in sqlEx.Errors)
                {
                    errorMessage += $"\nMensagem: {error.Message}, Linha: {error.LineNumber}, Origem: {error.Procedure}";
                }
                throw new ApplicationException(errorMessage, sqlEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao bloquear os cartões.", ex);
            }
        }

        public void BloquearDevedoresPorMes(DateTime dataInicial, DateTime dataFinal, int numeroMeses)
        {
            List<int> alunosDevedores = new List<int>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_DEVEDORES_MES", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@DATAINI", SqlDbType.DateTime) { Value = dataInicial });
                    cmd.Parameters.Add(new SqlParameter("@DATAFIM", SqlDbType.DateTime) { Value = dataFinal });
                    cmd.Parameters.Add(new SqlParameter("@NUM_MESES", SqlDbType.Int) { Value = numeroMeses });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alunosDevedores.Add(reader.GetInt32(1)); // Supondo que a primeira coluna seja NumAluno
                        }
                    }
                }
            }

            if (alunosDevedores.Any())
            {
                BloqueioCartao(alunosDevedores.ToArray(), false);
            }
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
