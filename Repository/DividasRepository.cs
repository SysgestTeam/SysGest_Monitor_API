﻿using SistemasdeTarefas.Interface;
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
        public int BloqueioCartao(int[] numAluno = null, bool emMassa = false)
        {
            if (numAluno == null && !emMassa)
            {
                throw new ArgumentException("A lista de alunos não pode ser nula, a menos que seja um bloqueio em massa.");
            }

            int totalLinhasAfetadas = 0;

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
                            if (emMassa)
                            {
                                // Bloqueio em massa
                                string queryEmMassa = "UPDATE CartaoAluno SET Bloqueado = 1 WHERE IdAno = @idAno";
                                using (SqlCommand cmd = new SqlCommand(queryEmMassa, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@idAno", idAno);
                                    totalLinhasAfetadas += cmd.ExecuteNonQuery();
                                }

                                // Chamada da Stored Procedure para o log
                                using (SqlCommand logCmd = new SqlCommand("sp_InserirLogBloqueioCartao", connection, transaction))
                                {
                                    logCmd.CommandType = CommandType.StoredProcedure;
                                    logCmd.Parameters.AddWithValue("@IsAluno", DBNull.Value);
                                    logCmd.Parameters.AddWithValue("@IdEntidade", DBNull.Value);
                                    logCmd.Parameters.AddWithValue("@TipoBloqueio", "Manual");
                                    logCmd.Parameters.AddWithValue("@AcaoBloqueio", "Bloqueio");
                                    logCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                if (numAluno.Length == 0)
                                {
                                    throw new ArgumentException("A lista de alunos não pode estar vazia.");
                                }

                                int chunkSize = 1000;
                                for (int i = 0; i < numAluno.Length; i += chunkSize)
                                {
                                    var chunk = numAluno.Skip(i).Take(chunkSize).ToList();

                                    string query = "UPDATE CartaoAluno SET Bloqueado = 1 WHERE IdAno = @idAno AND NumAluno IN (" +
                                                   string.Join(",", chunk.Select((_, idx) => $"@aluno{idx}")) + ")";

                                    using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@idAno", idAno);
                                        for (int j = 0; j < chunk.Count; j++)
                                        {
                                            cmd.Parameters.AddWithValue($"@aluno{j}", chunk[j]);
                                        }
                                        totalLinhasAfetadas += cmd.ExecuteNonQuery();
                                    }

                                    // Inserção dos logs para cada aluno
                                    foreach (var aluno in chunk)
                                    {
                                        using (SqlCommand logCmd = new SqlCommand("sp_InserirLogBloqueioCartao", connection, transaction))
                                        {
                                            logCmd.CommandType = CommandType.StoredProcedure;
                                            logCmd.Parameters.AddWithValue("@IsAluno", 1);
                                            logCmd.Parameters.AddWithValue("@IdEntidade", aluno);
                                            logCmd.Parameters.AddWithValue("@TipoBloqueio", "Manual");
                                            logCmd.Parameters.AddWithValue("@AcaoBloqueio", "Bloqueio");
                                            logCmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                            return totalLinhasAfetadas;
                        }
                        catch (Exception ex)
                        {
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

        public int BloquearDevedoresPorMes(DateTime dataInicial, DateTime dataFinal)
        {
            List<int> alunosDevedores = new List<int>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_DEVEDORES", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@DATAINI", SqlDbType.DateTime) { Value = dataInicial });
                    cmd.Parameters.Add(new SqlParameter("@DATAFIM", SqlDbType.DateTime) { Value = dataFinal });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alunosDevedores.Add(reader.GetInt32(4)); // Supondo que a primeira coluna seja NumAluno
                        }
                    }
                }
            }

            if (alunosDevedores.Any())
            {
                 return BloqueioCartao(alunosDevedores.ToArray(), false);
            }

            return 0;
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
        public void DesbloqueioCartao(int[] numAluno = null)
        {
            if (numAluno == null)
            {
                throw new ArgumentException("A lista de alunos não pode ser nula");
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
                                    SET Bloqueado = 0 
                                    WHERE NumAluno IN ({alunoList}) AND IdAno = @idAno";

                                    using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@idAno", idAno);
                                        cmd.ExecuteNonQuery();
                                    }

                                foreach (var aluno in chunk)
                                {
                                    using (SqlCommand logCmd = new SqlCommand("sp_InserirLogBloqueioCartao", connection, transaction))
                                    {
                                        logCmd.CommandType = CommandType.StoredProcedure;
                                        logCmd.Parameters.AddWithValue("@IsAluno", 1);
                                        logCmd.Parameters.AddWithValue("@IdEntidade", aluno);
                                        logCmd.Parameters.AddWithValue("@TipoBloqueio", "Manual");
                                        logCmd.Parameters.AddWithValue("@AcaoBloqueio", "Desloqueio");
                                        logCmd.ExecuteNonQuery();
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
        public void LogBloqueio(int IsAluno, int IdEntidade, string TipoBloqueio, string AcaoBloqueio)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {

                        using (SqlCommand cmd = new SqlCommand("sp_InserirLogBloqueioCartao", connection, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@IsAluno", SqlDbType.Bit) { Value = IsAluno });
                            cmd.Parameters.Add(new SqlParameter("@IdEntidade", SqlDbType.Int) { Value = IdEntidade });
                            cmd.Parameters.Add(new SqlParameter("@TipoBloqueio", SqlDbType.VarChar) { Value = TipoBloqueio });
                            cmd.Parameters.Add(new SqlParameter("@AcaoBloqueio", SqlDbType.VarChar) { Value = AcaoBloqueio });

                            cmd.ExecuteScalar();
                        }


                        transaction.Commit();
                    }
                    catch (SqlException sqlEx)
                    {
                        transaction.Rollback();

                        var errorMessage = "Erro ao cadastrar o log ";
                        foreach (SqlError error in sqlEx.Errors)
                        {
                            errorMessage += $"\nMensagem: {error.Message}, Linha: {error.LineNumber}, Origem: {error.Procedure}";
                        }

                        throw new ApplicationException(errorMessage, sqlEx);
                    }
                }
            }


        }
        public IEnumerable<LogBloqueio> LogBloqueio(DateTime? dataInicial = null, DateTime? dataFinal = null)
        {
            List<LogBloqueio> alunos = new List<LogBloqueio>();     

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {   
                connection.Open();

                string query = @"
                    SELECT TOP 2000 
                       [IdLogCartao],
                       [IdCartaoAcesso],
                       [Codigo],
                       [IsAluno],
                       [IdEntidade],
                       [NumInterno],
                       [NomeEntidade],
                       [IsExterno],
                       [DataRegisto],
                       [HoraRegisto],
                       [TipoBloqueio],
                       [AcaoBloqueio]
                    FROM [LogBloqueioCartoes]";
                    

                List<string> filtros = new List<string>();
                if (dataInicial.HasValue)
                    filtros.Add("DataRegisto >= @DataInicial");
                if (dataFinal.HasValue)
                    filtros.Add("DataRegisto <= @DataFinal");

                if (filtros.Count > 0)
                    query += " WHERE " + string.Join(" AND ", filtros);

                query += " ORDER BY DataRegisto DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    if (dataInicial.HasValue)
                        cmd.Parameters.AddWithValue("@DataInicial", dataInicial.Value);
                    if (dataFinal.HasValue)
                        cmd.Parameters.AddWithValue("@DataFinal", dataFinal.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LogBloqueio aluno = new LogBloqueio
                            {
                                IdLogCartao = reader.GetInt32(0),
                                IsAluno = reader.GetBoolean(3),
                                Codigo = reader.IsDBNull(2) ? null : reader.GetString(2),
                                NomeEntidade = reader.IsDBNull(6) ? null : reader.GetString(6),
                                IsExterno = reader.GetBoolean(7),
                                DataRegisto =  reader.GetDateTime(8),
                                TipoBloqueio = reader.IsDBNull(10) ? null : reader.GetString(10),
                                AcaoBloqueio = reader.IsDBNull(11) ? null : reader.GetString(11)
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
