using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
    }
}
