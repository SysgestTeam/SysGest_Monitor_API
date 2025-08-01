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
                string query = "";
                if (ano == 2025)
                {
                 query = @"
                    SELECT 
                        CASE TABCLASSES.IDCLASSE
                            WHEN 17 THEN 4
                            WHEN 4  THEN 3
                            WHEN 3  THEN 2
                            WHEN 2  THEN 1 
                        ELSE TABCLASSES.IDCLASSE
                        END AS Class_id,
                           TABCLASSES.NOME AS Class, 
                           TABTURMAS.NOME AS batch, 
                           TABTURMAS.IDTURMA AS batch_id  
                    FROM TABCLASSES
                    JOIN TABTURMAS ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                    WHERE TABCLASSES.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = @Ano)";
                }
                else
                {
                    query = @$"SELECT
                        cla_atu.NOME AS Class,
                        tur_atu.NOME AS batch,
                        tur_atu.IDTURMA AS batch_id,
                       CASE TABCLASSES.IDCLASSE
                            WHEN 17 THEN 4
                            WHEN 4  THEN 3
                            WHEN 3  THEN 2
                            WHEN 2  THEN 1 
                        ELSE TABCLASSES.IDCLASSE
                        END AS Class_id,
                    FROM
                        TABTURMAS AS tur_atu
                    JOIN
                        TABCLASSES AS cla_atu ON tur_atu.IDCLASSE = cla_atu.IDCLASSE
                    JOIN
                        TABCLASSES AS cla_ant ON
                            LOWER(cla_ant.NOME) =
                            CASE LOWER(cla_atu.NOME)
                                WHEN 'jk' THEN 'junior kindergarten'
                                WHEN 'sk' THEN 'senior kindergarten'
                                 WHEN 'TOODLERS' THEN 'Toddlers'
                                ELSE LOWER(cla_atu.NOME)
                            END
                    WHERE
                        cla_atu.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = @ano)
                        AND cla_ant.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = @ano - 1)
                    ORDER BY
                        cla_atu.NOME, batch;";
                }
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

            string sqlQuery = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                if (status == 1)
                {
                    sqlQuery = @$"SELECT 
                        TABALUNOS.NUMALUNO AS User_id, 
                        CINUMERO AS UserName,
                        TABALUNOS.NOME AS Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
                        RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
                        EMAIL,
                        CASE 
                            WHEN SEXO = 1 THEN 'Masculine' 
                            WHEN SEXO = 0 THEN 'Feminine' 
                            ELSE 'Not informed' 
                        END AS gender,
                        FOTO AS user_photo,
                        BAIRRO AS neighborhood,
                        MORADA AS address,
                        MUNICIPIO AS municipality,
                        COMUNA AS commune,
                        DATANASC AS date_of_birth,
                        OIEMAILMAE AS mother_email,
                        OINOMEMAE AS mother_name,
                        OITELFMAE AS mother_phone,
                        OIEMAILPAI AS father_email,
                        OINOMEPAI AS father_name,
                        OITELFPAI AS father_phone,
                        TABCLASSES.NOME AS class,
                        TABTURMAS.IDTURMA AS batch_id,
                        TABTURMAS.NOME AS batch,
                        CartaoAluno.Bloqueado AS is_blocked,
						TABTURMAS.IDCLASSE AS courseid,
                        TABSTATUS.NOME AS status
                    FROM TABALUNOS
                        LEFT JOIN TABMATRICULAS 
                            ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                            AND TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                        LEFT JOIN TABTURMAS 
                            ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA
                        LEFT JOIN TABCLASSES 
                            ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                        LEFT JOIN CartaoAluno 
                            ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
                        JOIN TABSTATUS 
                            ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
                    WHERE TABMATRICULAS.IDALUNO IS NULL
                    ORDER BY TABALUNOS.NUMALUNO ASC;
                    ";
                }
                else
                {


                    if (ano == 2025)
                    {
                        sqlQuery = @$"SELECT 
                        TABALUNOS.NUMALUNO AS User_id, 
                        CINUMERO AS UserName,
                        TABALUNOS.NOME AS Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
                        RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
                        EMAIL,
                        CASE 
                            WHEN SEXO = 1 THEN 'Masculine' 
                            WHEN SEXO = 0 THEN 'Feminine' 
                            ELSE 'Not informed' 
                        END AS gender,
                        FOTO AS user_photo,
                        BAIRRO AS neighborhood,
                        MORADA AS address,
                        MUNICIPIO AS municipality,
                        COMUNA AS commune,
                        DATANASC AS date_of_birth,
                        OIEMAILMAE AS mother_email,
                        OINOMEMAE AS mother_name,
                        OITELFMAE AS mother_phone,
                        OIEMAILPAI AS father_email,
                        OINOMEPAI AS father_name,
                        OITELFPAI AS father_phone,
                        TABCLASSES.NOME AS class,
                        TABTURMAS.IDTURMA AS batch_id,
                        TABTURMAS.NOME AS batch,
                        CartaoAluno.Bloqueado AS is_blocked,
						TABTURMAS.IDCLASSE AS courseid,
                        TABSTATUS.NOME AS status
                    FROM TABALUNOS
                        LEFT JOIN TABMATRICULAS 
                            ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                            AND TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                        LEFT JOIN TABTURMAS 
                            ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA
                        LEFT JOIN TABCLASSES 
                            ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                        LEFT JOIN CartaoAluno 
                            ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
                        JOIN TABSTATUS 
                            ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
                            AND TABMATRICULAS.IDSTATUS = {status}
                                        ORDER BY TABALUNOS.NUMALUNO ASC";
                    }
                    else if (ano > 2025)
                    {
                        sqlQuery = @$"SELECT 
                        TABALUNOS.NUMALUNO AS User_id, 
                        CINUMERO AS UserName,
                        TABALUNOS.NOME AS Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
                        RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
                        EMAIL,
                        CASE 
                            WHEN SEXO = 1 THEN 'Masculine' 
                            WHEN SEXO = 0 THEN 'Feminine' 
                            ELSE 'Not informed' 
                        END AS gender,
                        FOTO AS user_photo,
                        BAIRRO AS neighborhood,
                        MORADA AS address,
                        MUNICIPIO AS municipality,
                        COMUNA AS commune,
                        DATANASC AS date_of_birth,
                        OIEMAILMAE AS mother_email,
                        OINOMEMAE AS mother_name,
                        OITELFMAE AS mother_phone,
                        OIEMAILPAI AS father_email,
                        OINOMEPAI AS father_name,
                        OITELFPAI AS father_phone,
                        TABCLASSES.NOME AS class,
                        TABTURMAS.IDTURMA AS batch_id,
                        TABTURMAS.NOME AS batch,
                        CartaoAluno.Bloqueado AS is_blocked,
						CASE cla_ant.IDCLASSE
                            WHEN 17 THEN 4
                            WHEN 4  THEN 3
                            WHEN 3  THEN 2
                            WHEN 2  THEN 1 
                        ELSE cla_ant.IDCLASSE
                        END AS courseid,
                        TABSTATUS.NOME AS status
                    FROM TABALUNOS
                        LEFT JOIN TABMATRICULAS 
                            ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                            AND TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                        LEFT JOIN TABTURMAS 
                            ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA
                        LEFT JOIN TABCLASSES 
                            ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                        LEFT JOIN CartaoAluno 
                            ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
                        JOIN TABSTATUS 
                            ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
						        JOIN
                    TABCLASSES AS cla_atu ON TABTURMAS.IDCLASSE = cla_atu.IDCLASSE
                JOIN
                    TABCLASSES AS cla_ant ON
                        LOWER(cla_ant.NOME) =
                        CASE LOWER(cla_atu.NOME)
                            WHEN 'jk' THEN 'junior kindergarten'
                            WHEN 'sk' THEN 'senior kindergarten'
			                 WHEN 'TOODLERS' THEN 'Toddlers'
                            ELSE LOWER(cla_atu.NOME)
                        END
                WHERE
                    cla_atu.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                    AND cla_ant.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO =  {ano} - 1)
                                                                AND TABMATRICULAS.IDSTATUS = {status}
                                    ORDER BY TABALUNOS.NUMALUNO ASC";
                    }

                }

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            byte[]? photoBytes = reader.IsDBNull(reader.GetOrdinal("user_photo"))
                            ? null
                            : (byte[])reader["user_photo"];

                            string? base64Photo = photoBytes != null
                            ? Convert.ToBase64String(photoBytes)
                            : null;

                            string? photoLink = base64Photo != null
                            ? $"<a href='data:image/jpeg;base64,{base64Photo}' download='foto.jpg'>Baixar Foto</a>"
                            : null;

                            Student student = new Student
                            {
                                UserId = reader.IsDBNull(reader.GetOrdinal("User_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("User_id")),
                                UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserName")),
                                gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? string.Empty : reader.GetString(reader.GetOrdinal("gender")),
                                FullName = reader.IsDBNull(reader.GetOrdinal("Full_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Full_name")),
                                first_name = reader.IsDBNull(reader.GetOrdinal("first_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.IsDBNull(reader.GetOrdinal("last_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("last_name")),
                                Email = reader.IsDBNull(reader.GetOrdinal("EMAIL")) ? null : reader.GetString(reader.GetOrdinal("EMAIL")),
                                //UserPhoto = photoBytes,
                                //UserPhotoLink = photoLink,
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
                                course_id = reader.IsDBNull(reader.GetOrdinal("courseid")) ? 0 : reader.GetInt32(reader.GetOrdinal("courseid")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status"))
                            };

                            students.Add(student);
                        }
                    }
                }
            }

            return students;
        }

        public IEnumerable<Student> GetAllStudentsConfirmationORMatriculation(int ano, int status)
        {
            List<Student> students = new List<Student>();

            string sqlQuery = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                if(status == 1){
                     sqlQuery = @$"

                       SELECT 
                        TABALUNOS.NUMALUNO AS User_id, 
                        CINUMERO AS UserName,
                        TABALUNOS.NOME AS Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
                        RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
                        EMAIL,
                        CASE 
                            WHEN SEXO = 1 THEN 'Masculine' 
                            WHEN SEXO = 0 THEN 'Feminine' 
                            ELSE 'Not informed' 
                        END AS gender,
                        FOTO AS user_photo,
                        BAIRRO AS neighborhood,
                        MORADA AS address,
                        MUNICIPIO AS municipality,
                        COMUNA AS commune,
                        DATANASC AS date_of_birth,
                        OIEMAILMAE AS mother_email,
                        OINOMEMAE AS mother_name,
                        OITELFMAE AS mother_phone,
                        OIEMAILPAI AS father_email,
                        OINOMEPAI AS father_name,
                        OITELFPAI AS father_phone,
                        TABCLASSES.NOME AS class,
                        TABTURMAS.IDTURMA AS batch_id,
                        TABTURMAS.NOME AS batch,
                        CartaoAluno.Bloqueado AS is_blocked,
					                CASE cla_ant.IDCLASSE
                            WHEN 17 THEN 4
                            WHEN 4  THEN 3
                            WHEN 3  THEN 2
                            WHEN 2  THEN 1 
                        ELSE cla_ant.IDCLASSE
                        END AS courseid,
                        'Confirmation' AS status
                    FROM TABALUNOS
                        LEFT JOIN TABMATRICULAS 
                            ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                            AND TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                        LEFT JOIN TABTURMAS 
                            ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA
                        LEFT JOIN TABCLASSES 
                            ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                        LEFT JOIN CartaoAluno 
                            ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
                        JOIN TABSTATUS 
                            ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
					                        JOIN
                    TABCLASSES AS cla_atu ON TABTURMAS.IDCLASSE = cla_atu.IDCLASSE
                JOIN
                    TABCLASSES AS cla_ant ON
                        LOWER(cla_ant.NOME) =
                        CASE LOWER(cla_atu.NOME)
                            WHEN 'jk' THEN 'junior kindergarten'
                            WHEN 'sk' THEN 'senior kindergarten'
                             WHEN 'TOODLERS' THEN 'Toddlers'
                            ELSE LOWER(cla_atu.NOME)
                        END
                WHERE
                    cla_atu.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                    AND cla_ant.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO =  2025)
                                                                AND TABMATRICULAS.IDSTATUS = 2 AND TABMATRICULAS.CONFMATRICULA = 1 AND TABMATRICULAS.ISMATRICULA= 0
                                    ORDER BY TABALUNOS.NUMALUNO ASC";

                }
                else if(status == 2)
                {
                    sqlQuery = @$"
                       SELECT 
                        TABALUNOS.NUMALUNO AS User_id, 
                        CINUMERO AS UserName,
                        TABALUNOS.NOME AS Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
                        RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
                        EMAIL,
                        CASE 
                            WHEN SEXO = 1 THEN 'Masculine' 
                            WHEN SEXO = 0 THEN 'Feminine' 
                            ELSE 'Not informed' 
                        END AS gender,
                        FOTO AS user_photo,
                        BAIRRO AS neighborhood,
                        MORADA AS address,
                        MUNICIPIO AS municipality,
                        COMUNA AS commune,
                        DATANASC AS date_of_birth,
                        OIEMAILMAE AS mother_email,
                        OINOMEMAE AS mother_name,
                        OITELFMAE AS mother_phone,
                        OIEMAILPAI AS father_email,
                        OINOMEPAI AS father_name,
                        OITELFPAI AS father_phone,
                        TABCLASSES.NOME AS class,
                        TABTURMAS.IDTURMA AS batch_id,
                        TABTURMAS.NOME AS batch,
                        CartaoAluno.Bloqueado AS is_blocked,
					                CASE cla_ant.IDCLASSE
                            WHEN 17 THEN 4
                            WHEN 4  THEN 3
                            WHEN 3  THEN 2
                            WHEN 2  THEN 1 
                        ELSE cla_ant.IDCLASSE
                        END AS courseid,
                        'Matriculation' AS status
                    FROM TABALUNOS
                        LEFT JOIN TABMATRICULAS 
                            ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                            AND TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                        LEFT JOIN TABTURMAS 
                            ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA
                        LEFT JOIN TABCLASSES 
                            ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                        LEFT JOIN CartaoAluno 
                            ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
                        JOIN TABSTATUS 
                            ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
					                        JOIN
                    TABCLASSES AS cla_atu ON TABTURMAS.IDCLASSE = cla_atu.IDCLASSE
                JOIN
                    TABCLASSES AS cla_ant ON
                        LOWER(cla_ant.NOME) =
                        CASE LOWER(cla_atu.NOME)
                            WHEN 'jk' THEN 'junior kindergarten'
                            WHEN 'sk' THEN 'senior kindergarten'
                             WHEN 'TOODLERS' THEN 'Toddlers'
                            ELSE LOWER(cla_atu.NOME)
                        END
                WHERE
                    cla_atu.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                    AND cla_ant.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO =  2025)
                                                                AND TABMATRICULAS.IDSTATUS = 2 AND TABMATRICULAS.CONFMATRICULA = 0 AND TABMATRICULAS.ISMATRICULA= 1
                                    ORDER BY TABALUNOS.NUMALUNO ASC";
                }

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            byte[]? photoBytes = reader.IsDBNull(reader.GetOrdinal("user_photo"))
                            ? null
                            : (byte[])reader["user_photo"];

                            string? base64Photo = photoBytes != null
                            ? Convert.ToBase64String(photoBytes)
                            : null;

                            string? photoLink = base64Photo != null
                            ? $"<a href='data:image/jpeg;base64,{base64Photo}' download='foto.jpg'>Baixar Foto</a>"
                            : null;

                            Student student = new Student
                            {
                                UserId = reader.IsDBNull(reader.GetOrdinal("User_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("User_id")),
                                UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserName")),
                                gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? string.Empty : reader.GetString(reader.GetOrdinal("gender")),
                                FullName = reader.IsDBNull(reader.GetOrdinal("Full_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Full_name")),
                                first_name = reader.IsDBNull(reader.GetOrdinal("first_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.IsDBNull(reader.GetOrdinal("last_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("last_name")),
                                Email = reader.IsDBNull(reader.GetOrdinal("EMAIL")) ? null : reader.GetString(reader.GetOrdinal("EMAIL")),
                                //UserPhoto = photoBytes,
                                //UserPhotoLink = photoLink,
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
                                IsBlocked = reader.IsDBNull(reader.GetOrdinal("is_blocked")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("is_blocked")),
                                batch_id = reader.IsDBNull(reader.GetOrdinal("batch_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("batch_id")),
                                batch = reader.IsDBNull(reader.GetOrdinal("batch")) ? string.Empty : reader.GetString(reader.GetOrdinal("batch")),
                                course_id = reader.IsDBNull(reader.GetOrdinal("courseid")) ? 0 : reader.GetInt32(reader.GetOrdinal("courseid")),
                                status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status"))
                            };

                            students.Add(student);
                        }
                    }
                }
            }

            return students;
        }

        public IEnumerable<Student> GetAllStudentsWithPhoto(int ano, int status)
        {
            List<Student> students = new List<Student>();

            string sqlQuery = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                if (status == 1)
                {
                    sqlQuery = @$"SELECT 
                        TABALUNOS.NUMALUNO AS User_id, 
                        CINUMERO AS UserName,
                        TABALUNOS.NOME AS Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
                        RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
                        EMAIL,
                        CASE 
                            WHEN SEXO = 1 THEN 'Masculine' 
                            WHEN SEXO = 0 THEN 'Feminine' 
                            ELSE 'Not informed' 
                        END AS gender,
                        FOTO AS user_photo,
                        BAIRRO AS neighborhood,
                        MORADA AS address,
                        MUNICIPIO AS municipality,
                        COMUNA AS commune,
                        DATANASC AS date_of_birth,
                        OIEMAILMAE AS mother_email,
                        OINOMEMAE AS mother_name,
                        OITELFMAE AS mother_phone,
                        OIEMAILPAI AS father_email,
                        OINOMEPAI AS father_name,
                        OITELFPAI AS father_phone,
                        TABCLASSES.NOME AS class,
                        TABTURMAS.IDTURMA AS batch_id,
                        TABTURMAS.NOME AS batch,
                        CartaoAluno.Bloqueado AS is_blocked,
						TABTURMAS.IDCLASSE AS courseid,
                        TABSTATUS.NOME AS status
                    FROM TABALUNOS
                        LEFT JOIN TABMATRICULAS 
                            ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                            AND TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                        LEFT JOIN TABTURMAS 
                            ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA
                        LEFT JOIN TABCLASSES 
                            ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                        LEFT JOIN CartaoAluno 
                            ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
                        JOIN TABSTATUS 
                            ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
                    WHERE TABMATRICULAS.IDALUNO IS NULL
                    ORDER BY TABALUNOS.NUMALUNO ASC;
                    ";
                }
                else
                {


                    if (ano == 2025)
                    {
                        sqlQuery = @$"SELECT 
                        TABALUNOS.NUMALUNO AS User_id, 
                        CINUMERO AS UserName,
                        TABALUNOS.NOME AS Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
                        RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
                        EMAIL,
                        CASE 
                            WHEN SEXO = 1 THEN 'Masculine' 
                            WHEN SEXO = 0 THEN 'Feminine' 
                            ELSE 'Not informed' 
                        END AS gender,
                        FOTO AS user_photo,
                        BAIRRO AS neighborhood,
                        MORADA AS address,
                        MUNICIPIO AS municipality,
                        COMUNA AS commune,
                        DATANASC AS date_of_birth,
                        OIEMAILMAE AS mother_email,
                        OINOMEMAE AS mother_name,
                        OITELFMAE AS mother_phone,
                        OIEMAILPAI AS father_email,
                        OINOMEPAI AS father_name,
                        OITELFPAI AS father_phone,
                        TABCLASSES.NOME AS class,
                        TABTURMAS.IDTURMA AS batch_id,
                        TABTURMAS.NOME AS batch,
                        CartaoAluno.Bloqueado AS is_blocked,
						TABTURMAS.IDCLASSE AS courseid,
                        TABSTATUS.NOME AS status
                    FROM TABALUNOS
                        LEFT JOIN TABMATRICULAS 
                            ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                            AND TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                        LEFT JOIN TABTURMAS 
                            ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA
                        LEFT JOIN TABCLASSES 
                            ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                        LEFT JOIN CartaoAluno 
                            ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
                        JOIN TABSTATUS 
                            ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
                            AND TABMATRICULAS.IDSTATUS = {status}
                                        ORDER BY TABALUNOS.NUMALUNO ASC";
                    }
                    else if (ano > 2025){
                        sqlQuery = @$"SELECT 
                        TABALUNOS.NUMALUNO AS User_id, 
                        CINUMERO AS UserName,
                        TABALUNOS.NOME AS Full_name,
                        LEFT(TABALUNOS.NOME, CHARINDEX(' ', TABALUNOS.NOME + ' ') - 1) AS first_name,
                        RIGHT(TABALUNOS.NOME, CHARINDEX(' ', REVERSE(TABALUNOS.NOME)) - 1) AS last_name,
                        EMAIL,
                        CASE 
                            WHEN SEXO = 1 THEN 'Masculine' 
                            WHEN SEXO = 0 THEN 'Feminine' 
                            ELSE 'Not informed' 
                        END AS gender,
                        FOTO AS user_photo,
                        BAIRRO AS neighborhood,
                        MORADA AS address,
                        MUNICIPIO AS municipality,
                        COMUNA AS commune,
                        DATANASC AS date_of_birth,
                        OIEMAILMAE AS mother_email,
                        OINOMEMAE AS mother_name,
                        OITELFMAE AS mother_phone,
                        OIEMAILPAI AS father_email,
                        OINOMEPAI AS father_name,
                        OITELFPAI AS father_phone,
                        TABCLASSES.NOME AS class,
                        TABTURMAS.IDTURMA AS batch_id,
                        TABTURMAS.NOME AS batch,
                        CartaoAluno.Bloqueado AS is_blocked,
						CASE cla_ant.IDCLASSE
                            WHEN 17 THEN 4
                            WHEN 4  THEN 3
                            WHEN 3  THEN 2
                            WHEN 2  THEN 1 
                        ELSE cla_ant.IDCLASSE
                        END AS courseid,
                        TABSTATUS.NOME AS status
                    FROM TABALUNOS
                        LEFT JOIN TABMATRICULAS 
                            ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                            AND TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                        LEFT JOIN TABTURMAS 
                            ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA
                        LEFT JOIN TABCLASSES 
                            ON TABCLASSES.IDCLASSE = TABTURMAS.IDCLASSE
                        LEFT JOIN CartaoAluno 
                            ON CartaoAluno.IdAluno = TABALUNOS.IDALUNO
                        JOIN TABSTATUS 
                            ON TABSTATUS.IDSTATUS = TABALUNOS.IDSTATUS
						        JOIN
                    TABCLASSES AS cla_atu ON TABTURMAS.IDCLASSE = cla_atu.IDCLASSE
                JOIN
                    TABCLASSES AS cla_ant ON
                        LOWER(cla_ant.NOME) =
                        CASE LOWER(cla_atu.NOME)
                            WHEN 'jk' THEN 'junior kindergarten'
                            WHEN 'sk' THEN 'senior kindergarten'
			                 WHEN 'TOODLERS' THEN 'Toddlers'
                            ELSE LOWER(cla_atu.NOME)
                        END
                WHERE
                    cla_atu.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = {ano})
                    AND cla_ant.IDANO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO =  {ano} - 1)
                                                                AND TABMATRICULAS.IDSTATUS = {status}
                                    ORDER BY TABALUNOS.NUMALUNO ASC";
                    }
                   
                }
               
                
                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            byte[]? photoBytes = reader.IsDBNull(reader.GetOrdinal("user_photo"))
                            ? null
                            : (byte[])reader["user_photo"];

                            string? base64Photo = photoBytes != null
                            ? Convert.ToBase64String(photoBytes)
                            : null;

                            string? photoLink = base64Photo != null
                            ? $"<a href='data:image/jpeg;base64,{base64Photo}' download='foto.jpg'>Baixar Foto</a>"
                            : null;

                            Student student = new Student
                            {
                                UserId = reader.IsDBNull(reader.GetOrdinal("User_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("User_id")),
                                UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserName")),
                                gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? string.Empty : reader.GetString(reader.GetOrdinal("gender")),
                                FullName = reader.IsDBNull(reader.GetOrdinal("Full_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Full_name")),
                                first_name = reader.IsDBNull(reader.GetOrdinal("first_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.IsDBNull(reader.GetOrdinal("last_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("last_name")),
                                Email = reader.IsDBNull(reader.GetOrdinal("EMAIL")) ? null : reader.GetString(reader.GetOrdinal("EMAIL")),
                                UserPhoto = photoBytes,
                                UserPhotoLink = photoLink,
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
                                IsBlocked = reader.IsDBNull(reader.GetOrdinal("is_blocked")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("is_blocked")),
                                batch_id = reader.IsDBNull(reader.GetOrdinal("batch_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("batch_id")),
                                batch = reader.IsDBNull(reader.GetOrdinal("batch")) ? string.Empty : reader.GetString(reader.GetOrdinal("batch")),
                                course_id = reader.IsDBNull(reader.GetOrdinal("courseid")) ? 0 : reader.GetInt32(reader.GetOrdinal("courseid")),
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

        public IEnumerable<Existencia_Card> GetAlunosPorNumeroTelefonePai(int numeroTelefone)
        {
            List<Existencia_Card> classes = new List<Existencia_Card>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @$"SSELECT 
                                        TABALUNOS.FOTO, 
                                        TABALUNOS.IDALUNO, 
                                        NUMALUNO, 
                                        TABALUNOS.NOME AS [ALUNO], 
                                        TABTURMAS.NOME AS [NomeTurma], 
                                        IDANOLECTIVO
                                    FROM 
                                        TABALUNOS 
                                        INNER JOIN TABMATRICULAS ON TABMATRICULAS.IDALUNO = TABALUNOS.IDALUNO 
                                        INNER JOIN TABTURMAS ON TABTURMAS.IDTURMA = TABMATRICULAS.IDTURMA    
                                        INNER JOIN TABSTATUS s ON TABALUNOS.IDSTATUS = s.IDSTATUS 
                                    WHERE 
                                        TABALUNOS.INACTIVO = 0 
                                        AND TABMATRICULAS.IDSTATUS IN (2, 4) 
                                        AND TABTURMAS.NOME NOT IN ('FUNCIONÁRIO', 'DOCENTE')
                                        AND TABMATRICULAS.IDANOLECTIVO = (SELECT MAX(IDANO) FROM TABANOSLECTIVOS)
                                        AND (
                                            OITELFPAI = '{numeroTelefone}' 
                                            OR OITELFMAE = '{numeroTelefone}' 
                                            OR OITELFENCARG = '{numeroTelefone}'
                                        )
                                        AND UsaAppSync = 1
                                    ";

                                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Existencia_Card classe = new Existencia_Card
                            {
                                Nome = reader.GetString(3),
                                NomeTurma = reader.GetString(4),
                                NumAluno = reader.GetInt32(2),
                                Foto = reader.IsDBNull(0) ? null : (byte[])reader.GetValue(0)
                            };

                            classes.Add(classe);
                        }
                    }
                }
            }

            return classes;
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

        public IEnumerable<StudentCount> GetCoutnAllStudentByYear(int ano, int status)
        {
            var students = new List<StudentCount>();

            string sqlQuery = @"
                    SELECT COUNT(*) AS TOTALMATRICULA, TABANOSLECTIVOS.ANO 
                    FROM TABMATRICULAS 
                    JOIN TABANOSLECTIVOS ON TABANOSLECTIVOS.IDANO = TABMATRICULAS.IDANOLECTIVO
                    WHERE TABMATRICULAS.IDANOLECTIVO = (SELECT IDANO FROM TABANOSLECTIVOS WHERE ANO = @Ano) AND TABMATRICULAS.IDSTATUS = @Status
                    GROUP BY TABANOSLECTIVOS.ANO
                ";

            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sqlQuery, connection);

            cmd.Parameters.AddWithValue("@Ano", ano);
            cmd.Parameters.AddWithValue("@Status", status);

            connection.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var student = new StudentCount
                {
                    Count = reader["TOTALMATRICULA"] as int? ?? 0,
                    Year = reader["ANO"] as int? ?? 0
                };
                students.Add(student);
            }

            return students;
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
