﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;

namespace SistemasdeTarefas.Repository
{
    public class CartaoRepository : ICartaoRepository
    {
        private readonly string _connectionString;

        public CartaoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Cartao> GetEncarregados( string codigo)
        {
            List<Cartao> encarregados = new List<Cartao>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string storedProcedureName = "sp_CatracaPesquisaEncarregadoCartao";

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Adicionando parâmetros ao comando SQL
                    cmd.Parameters.Add(new SqlParameter("@codigo", codigo));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cartao cartao = new Cartao
                            {
                                Nome = reader.GetString(0),
                                CodigoCartao = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3)
                            };

                            encarregados.Add(cartao);
                        }
                    }
                }
            }

            return encarregados;
        }

        public IEnumerable<Cartao> GetEstudantes(string codigo)
        {
            List<Cartao> estudantes = new List<Cartao>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string storedProcedureName = "sp_CatracaPesquisaEstudanteCartao";

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Adicionando parâmetros ao comando SQL
                    cmd.Parameters.Add(new SqlParameter("@codigo", codigo));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cartao cartao = new Cartao
                            {
                                Nome = reader.GetString(0),
                                CodigoCartao = reader.GetString(1),
                                NomeTurma = reader.GetString(2),
                                Foto = reader.IsDBNull(3) ? null : (byte[])reader.GetValue(3)
                            };

                            estudantes.Add(cartao);
                        }
                    }
                }
            }

            return estudantes;
        }
    }
}