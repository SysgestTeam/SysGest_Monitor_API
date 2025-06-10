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

        public IEnumerable<Class> GetAllClass(int ano)
        {
            var classesDict = new Dictionary<int, Class>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
        SELECT TABCLASSES.IDCLASSE AS Class_id,
               TABCLASSES.NOME AS Class, 
               TABTURMAS.NOME AS batch, 
               TABTURMAS.IDTURMA AS batch_id  
        FROM TABCLASSES
        JOIN TABTURMAS ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
        WHERE TABCLASSES.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = @Ano)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ano", ano);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int classId = reader.GetInt32(reader.GetOrdinal("Class_id"));

                            // Verifica se a classe já existe no dicionário
                            if (!classesDict.TryGetValue(classId, out var classObj))
                            {
                                classObj = new Class
                                {
                                    id = classId,
                                    name = reader.GetString(reader.GetOrdinal("Class")),
                                    batche = new List<batch>() // Inicializa a lista de turmas
                                };

                                classesDict[classId] = classObj;
                            }

                            // Adiciona o batch à lista da classe
                            classObj.batche.Add(new batch
                            {
                                id = reader.GetInt32(reader.GetOrdinal("batch_id")),
                                name = reader.GetString(reader.GetOrdinal("batch"))
                            });
                        }
                    }
                }
            }

            return classesDict.Values;
        }
        public IEnumerable<Student> GetAllStudents(int ano, int status)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @$"
                    SELECT 
		                TABALUNOS.NUMALUNO  AS User_id, 
		                CINUMERO UserName,
		                TABALUNOS.NOME Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
	                    RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
		                EMAIL ,
		                CASE 
                        WHEN SEXO = 1 THEN 'Masculine' 
                        WHEN SEXO = 0 THEN 'Feminine' 
                        ELSE 'Not informed' 
                    END AS gender,
		                FOTO user_photo,
		                BAIRRO neighborhood,
		                MORADA address,
		                MUNICIPIO municipality,
		                COMUNA commune,
		                DATANASC date_of_birth,
		                OIEMAILMAE mother_email,
		                OINOMEMAE mother_name,
		                OITELFMAE mother_phone,
		                OIEMAILPAI father_email,
		                OINOMEPAI father_name,
		                OITELFPAI father_phone,
		                TABCLASSES.NOME class,
		                TABTURMAS.IDTURMA batch_id,
		                TABTURMAS.NOME batch,
		                CartaoAluno.Bloqueado is_blocked,
		                TABSTATUS.NOME status
		                FROM TABALUNOS
			                JOIN TABMATRICULAS
		                ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO
			                JOIN TABTURMAS
		                ON TABTURMAS.IDTURMA =  TABMATRICULAS.IDTURMA
			                LEFT JOIN CartaoAluno
		                ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
			                JOIN TABSTATUS
		                ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
			                JOIN  TABCLASSES
		                 ON TABCLASSES.IDCLASSE =  TABTURMAS.IDCLASSE
			                WHERE TABMATRICULAS.IDANOLECTIVO =(SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                            AND TABMATRICULAS.IDSTATUS = {status}
		                ORDER BY TABALUNOS.NUMALUNO ASC";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student
                            {
                                UserId = reader.IsDBNull(reader.GetOrdinal("User_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("User_id")),
                                UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserName")),
                                gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? string.Empty : reader.GetString(reader.GetOrdinal("gender")),
                                FullName = reader.IsDBNull(reader.GetOrdinal("Full_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Full_name")),
                                first_name = reader.IsDBNull(reader.GetOrdinal("first_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.IsDBNull(reader.GetOrdinal("last_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("last_name")),
                                Email = reader.IsDBNull(reader.GetOrdinal("EMAIL")) ? null : reader.GetString(reader.GetOrdinal("EMAIL")),
                                UserPhoto = reader.IsDBNull(reader.GetOrdinal("user_photo")) ? null : (byte[])reader["user_photo"],
                                Neighborhood = reader.IsDBNull(reader.GetOrdinal("neighborhood")) ? null : reader.GetString(reader.GetOrdinal("neighborhood")),
                                Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address")),
                                Municipality = reader.IsDBNull(reader.GetOrdinal("municipality")) ? null : reader.GetString(reader.GetOrdinal("municipality")),
                                Commune = reader.IsDBNull(reader.GetOrdinal("commune")) ? null : reader.GetString(reader.GetOrdinal("commune")),
                                DateOfBirth = (DateTime)(reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                                MotherEmail = reader.IsDBNull(reader.GetOrdinal("mother_email")) ? null : reader.GetString(reader.GetOrdinal("mother_email")),
                                MotherName = reader.IsDBNull(reader.GetOrdinal("mother_name")) ? null : reader.GetString(reader.GetOrdinal("mother_name")),
                                MotherPhone = reader.IsDBNull(reader.GetOrdinal("mother_phone")) ? null : reader.GetString(reader.GetOrdinal("mother_phone")),
                                FatherEmail = reader.IsDBNull(reader.GetOrdinal("father_email")) ? null : reader.GetString(reader.GetOrdinal("father_email")),
                                FatherName = reader.IsDBNull(reader.GetOrdinal("father_name")) ? null : reader.GetString(reader.GetOrdinal("father_name")),
                                FatherPhone = reader.IsDBNull(reader.GetOrdinal("father_phone")) ? null : reader.GetString(reader.GetOrdinal("father_phone")),
                                Class = reader.IsDBNull(reader.GetOrdinal("class")) ? string.Empty : reader.GetString(reader.GetOrdinal("class")),
                         IsBlocked = reader.IsDBNull(reader.GetOrdinal("is_blocked"))? (bool?)null: reader.GetBoolean(reader.GetOrdinal("is_blocked")),
                                batch_id = reader.IsDBNull(reader.GetOrdinal("batch_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("batch_id")),
                                batch = reader.IsDBNull(reader.GetOrdinal("batch")) ? string.Empty : reader.GetString(reader.GetOrdinal("batch")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status"))
                            };

                            students.Add(student);
                        }
                    }
                }
            }

            return students;
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
