﻿using System.Data;
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
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3),
                                CodigoCartao = reader.GetString(4),// Verifica se a coluna é DBNull
                                saldo = reader.GetDecimal(5),// Verifica se a coluna é DBNull
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
        public IEnumerable<Existencia_Card> GetInexistenciaCardFiltro(int? idclasse = null, int? idturma = null)
        {
            List<Existencia_Card> alunos = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Chame o procedimento armazenado

                using (SqlCommand cmd = new SqlCommand("sp_ListarAlunosComCartaoPorClasseETurma", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IDCLASSE", idclasse));
                    cmd.Parameters.Add(new SqlParameter("@IDTURMA", idturma));


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card aluno = new Existencia_Card
                            {
                                Nome = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                aluno.Foto = (byte[])reader.GetValue(3);
                            }

                            alunos.Add(aluno);
                        }
                    }
                }
            }

            return alunos;
        }
        public IEnumerable<Existencia_Card> GetInexistenciaCardBloqueadoFiltro(int? idclasse = null, int? idturma = null)
        {
            List<Existencia_Card> alunos = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Chame o procedimento armazenado

                using (SqlCommand cmd = new SqlCommand("sp_ListarAlunosComCartaoBloqueadoPorClasseETurma", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IDCLASSE", idclasse));
                    cmd.Parameters.Add(new SqlParameter("@IDTURMA", idturma));


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card aluno = new Existencia_Card
                            {
                                Nome = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                aluno.Foto = (byte[])reader.GetValue(3);
                            }

                            alunos.Add(aluno);
                        }
                    }
                }
            }

            return alunos;
        }
        public IEnumerable<Existencia_Card> GetInexistenciaCardSemAcompanhateFiltro(int? idclasse = null, int? idturma = null)
        {
            List<Existencia_Card> alunos = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Chame o procedimento armazenado

                using (SqlCommand cmd = new SqlCommand("sp_ListarAlunosComCartaoSemAcompanhantePorClasseETurma", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IDCLASSE", idclasse));
                    cmd.Parameters.Add(new SqlParameter("@IDTURMA", idturma));


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card aluno = new Existencia_Card
                            {
                                Nome = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                aluno.Foto = (byte[])reader.GetValue(3);
                            }

                            alunos.Add(aluno);
                        }
                    }
                }
            }

            return alunos;
        }
        public IEnumerable<Existencia_Card> GetTodosCartoes(int ano)
        {
            List<Existencia_Card> inexistenciaCards = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Nome do procedimento armazenado
                string query = @$"SELECT * FROM CartaoAluno
                        WHERE IdAno = (SELECT IdAno FROM TABANOSLECTIVOS WHERE ANO = {ano})";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card card = new Existencia_Card
                            {
                                NumAluno = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                Nome = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                NomeTurma = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                CodigoCartao = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                Bloqueado = reader.IsDBNull(12) ? false : reader.GetBoolean(12),
                                NaoBloqueavel = reader.IsDBNull(24) ? (bool?)null : reader.GetBoolean(24),
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
