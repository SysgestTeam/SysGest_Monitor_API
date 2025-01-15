using System.Data.SqlClient;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Repository
{
    public class ProfessoresRepository : IProfessoresRepository
    {
        private readonly string _connectionString;

        public ProfessoresRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Funcionario> GetAll()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = "SELECT NUMDOCENTE,NOME,Profissao, Foto FROM TDocentes WHERE INACTIVO = 0";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Funcionario funcionario = new Funcionario
                            {
                                Numero = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Nome = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Profissao = reader.IsDBNull(2) ? null : reader.GetString(2),
                                foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                funcionario.foto = (byte[])reader.GetValue(3);
                            }

                            funcionarios.Add(funcionario);
                        }
                    }
                }
            }

            return funcionarios;
        }

        public IEnumerable<Funcionario> GetComCartao()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = @"SELECT 
                            TDocentes.NUMDOCENTE,
                            TDocentes.NOME, 
                            TDocentes.Profissao, 
                            TDocentes.Foto 
                        FROM TDocentes
                        LEFT JOIN CartaoDocente
                        ON CartaoDocente.NumInterno = TDocentes.NUMDOCENTE
                        WHERE CartaoDocente.NumInterno IS NOT NULL AND TDocentes.Inactivo = 0;";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Funcionario funcionario = new Funcionario
                            {
                                Numero = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Profissao = reader.GetString(2),
                                foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                funcionario.foto = (byte[])reader.GetValue(3);
                            }

                            funcionarios.Add(funcionario);
                        }
                    }
                }
            }

            return funcionarios;
        }

        public IEnumerable<Funcionario> GetProfessoresComFotos()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = "SELECT NUMDOCENTE,NOME,Profissao, Foto FROM TDocentes WHERE FOTO IS NOT NULL AND INACTIVO = 0";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Funcionario funcionario = new Funcionario
                            {
                                Numero = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Profissao = reader.GetString(2),
                                foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                funcionario.foto = (byte[])reader.GetValue(3);
                            }

                            funcionarios.Add(funcionario);
                        }
                    }
                }
            }

            return funcionarios;
        }

        public IEnumerable<Funcionario> GetProfessoresSemFoto()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = "SELECT NUMDOCENTE,NOME,Profissao, Foto FROM TDocentes WHERE FOTO IS NULL AND INACTIVO = 0";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Funcionario funcionario = new Funcionario
                            {
                                Numero = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Nome = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Profissao = reader.IsDBNull(2) ? null : reader.GetString(2),
                                foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                funcionario.foto = (byte[])reader.GetValue(3);
                            }

                            funcionarios.Add(funcionario);
                        }
                    }
                }
            }

            return funcionarios;
        }

        public IEnumerable<Funcionario> GetSemCartao()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = @"SELECT 
                            TDocentes.NUMDOCENTE,
                            TDocentes.NOME, 
                            TDocentes.Profissao, 
                            TDocentes.Foto 
                        FROM TDocentes
                        LEFT JOIN CartaoDocente
                        ON CartaoDocente.NumInterno = TDocentes.NUMDOCENTE
                        WHERE CartaoDocente.NumInterno IS NULL AND TDocentes.Inactivo = 0;";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Funcionario funcionario = new Funcionario
                            {
                                Numero = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Nome = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Profissao = reader.IsDBNull(2) ? null : reader.GetString(2),
                                foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3) // Verifica se a coluna é DBNull
                            };
                            if (reader["foto"] != DBNull.Value)
                            {
                                funcionario.foto = (byte[])reader.GetValue(3);
                            }

                            funcionarios.Add(funcionario);
                        }
                    }
                }
            }

            return funcionarios;
        }
    }
}
