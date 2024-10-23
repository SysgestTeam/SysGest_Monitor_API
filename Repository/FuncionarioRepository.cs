using System.Data;
using System.Data.SqlClient;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Repository
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly string _connectionString;

        public FuncionarioRepository(IConfiguration configuration)
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
                string sqlQuery = "SELECT NumInterno,NomeFuncionario, Profissao, Foto FROM Funcionario WHERE INACTIVO = 0";

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

        public IEnumerable<Funcionario> GetComCartao()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Funcionario> GetFuncionarioComFotos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Funcionario> GetFuncionarioSemFoto()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Funcionario> GetSemCartao()
        {
            throw new NotImplementedException();
        }
    }
}
