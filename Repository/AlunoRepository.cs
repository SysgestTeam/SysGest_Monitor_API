using System.Data;
using System.Data.SqlClient;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Repository
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly string _connectionString;

        public AlunoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<TabAluno> GetAlunos()
        {
            List<TabAluno> alunos = new List<TabAluno>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("sp_SaidaEntrada", connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TabAluno aluno = new TabAluno
                            {
                                Nome = reader.GetString(0),
                                Hora = reader.GetString(1),
                                Descricao = reader.GetString(2),
                                foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3)
                            };

                            if (reader["foto"] != DBNull.Value)
                            {
                                aluno.foto = (byte[])reader["foto"];
                            }

                            alunos.Add(aluno);
                        }
                    }
                }
            }

            return alunos;
        }


        public IEnumerable<TabAluno> GetAlunosFiltro(int? idclasse = null, int? idturma = null)
        {
            List<TabAluno> alunos = new List<TabAluno>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Chame o procedimento armazenado

                using (SqlCommand cmd = new SqlCommand("sp_ListarAlunosPorClasseETurma", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IDCLASSE", idclasse));
                    cmd.Parameters.Add(new SqlParameter("@IDTURMA", idturma));


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TabAluno aluno = new TabAluno
                            {
                                Nome = reader.GetString(1),
                                Turma = reader.GetString(2),
                                foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
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

        public IEnumerable<Existencia_Card> GetAlunosSemFotos()
        {
            List<Existencia_Card> alunos = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "EXEC sp_ListarAlunosSemFotos";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card aluno = new Existencia_Card
                            {
                                // Mapeie as colunas do resultado para o objeto TabAluno
                                Nome = reader.GetString(3),
                                NomeTurma = reader.GetString(4),
                                NumAluno = reader.GetInt32(2),
                                Foto = reader.IsDBNull(0) ? null : (byte[])reader.GetValue(0) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                aluno.Foto = (byte[])reader.GetValue(0);
                            }

                            alunos.Add(aluno);
                        }
                    }
                }
            }

            return alunos;
        }

        public IEnumerable<Existencia_Card> GetAlunosSemFotosFiltro(int? idclasse = null, int? idturma = null)
        {
            List<Existencia_Card> alunos = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ListarAlunosSemFotosClasseETurma", connection))
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
                                // Mapeie as colunas do resultado para o objeto TabAluno
                                Nome = reader.GetString(3),
                                NomeTurma = reader.GetString(4),
                                NumAluno = reader.GetInt32(2),
                                Foto = reader.IsDBNull(0) ? null : (byte[])reader.GetValue(0) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                aluno.Foto = (byte[])reader.GetValue(0);
                            }

                            alunos.Add(aluno);
                        }
                    }
                }
            }

            return alunos;
        }

        public IEnumerable<Classes> GetClasses()
        {
            List<Classes> classes = new List<Classes>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = "SELECT IDCLASSE, NOME, IDCICLO, IDANO FROM TABCLASSES WHERE INACTIVO <> 1";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Classes classe = new Classes
                            {
                                IDCLASSE = reader.GetInt32(0),  
                                NOME = reader.GetString(1),     
                                IDCICLO = reader.GetInt32(2),  
                                IDANO = reader.GetInt32(3)     
                            };

                            classes.Add(classe);
                        }
                    }
                }
            }

            return classes;
        }
        public IEnumerable<Turmas> GetTurmas(int classe)
        {
            List<Turmas> turmas = new List<Turmas>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = $"SELECT IDTURMA, NOME, IDCLASSE, NUMVAGAS, ANO FROM TABTURMAS WHERE INACTIVO <> 1 AND IDCLASSE = {classe}";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Turmas turma = new Turmas
                            {

                                IDTURMA = reader.GetInt32(0),
                                NOME = reader.GetString(1),
                                IDCLASSE = reader.GetInt32(2),
                                NUMVAGAS = reader.GetInt32(3),
                                ANO = reader.GetInt32(4),
                            };

                            turmas.Add(turma);
                        }
                    }
                }
            }

            return turmas;
        }
    }
}
