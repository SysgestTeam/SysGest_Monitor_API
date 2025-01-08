﻿using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;

namespace SistemasdeTarefas.Repository
{
    public class SaldoConsumoRepository : ISaldoConsumo
    {
        private readonly string _connectionString;

        public SaldoConsumoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Consumo(int numAluno, decimal usedValue)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Query SQL para lançar o consumo
                        string sqlQuery = @"
                        DECLARE @IdAluno INT;
                        SET @IdAluno = (SELECT IDALUNO FROM TABALUNOS WHERE NUMALUNO = @NumAluno);
                        INSERT INTO SaldosConsumos (IDALUNO, UsedValue, Anulado, Deleted, DataRegisto, DataAlter, TimeRegist)
                        VALUES (@IdAluno, @UsedValue, 0, 0, @DataRegisto, @DataAlter, @TimeRegist);
                    ";

                        using (SqlCommand cmd = new SqlCommand(sqlQuery, connection, transaction))
                        {
                            // Adicionar parâmetros
                            cmd.Parameters.Add(new SqlParameter("@NumAluno", SqlDbType.Int) { Value = numAluno });
                            cmd.Parameters.Add(new SqlParameter("@UsedValue", SqlDbType.Decimal) { Value = usedValue });
                            cmd.Parameters.Add(new SqlParameter("@DataRegisto", SqlDbType.DateTime) { Value = DateTime.Now });
                            cmd.Parameters.Add(new SqlParameter("@DataAlter", SqlDbType.DateTime) { Value = DateTime.Now });
                            cmd.Parameters.Add(new SqlParameter("@TimeRegist", SqlDbType.Time) { Value = DateTime.Now.TimeOfDay });

                            // Executar a query
                            cmd.ExecuteNonQuery();
                        }

                        // Gerar o ticket
                        GerarTicket(numAluno);

                        // Confirmar a transação
                        transaction.Commit();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Reverter a transação em caso de erro
                        transaction.Rollback();

                        // Log detalhado do erro da SP
                        var errorMessage = "Erro ao realizar o lançamento de consumo ou gerar ticket. Detalhes do erro SQL: ";
                        foreach (SqlError error in sqlEx.Errors)
                        {
                            errorMessage += $"\nMensagem: {error.Message}, Linha: {error.LineNumber}, Origem: {error.Procedure}";
                        }

                        // Lançar uma exceção personalizada com os detalhes
                        throw new ApplicationException(errorMessage, sqlEx);
                    }
                    catch (Exception ex)
                    {
                        // Reverter a transação para erros genéricos
                        transaction.Rollback();

                        // Lançar uma exceção padrão
                        throw new ApplicationException("Erro ao realizar o lançamento de consumo ou gerar ticket.", ex);
                    }
                }
            }
        }



        public void GerarTicket(int numAluno)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_CriarTicket", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adicionar parâmetros
                        cmd.Parameters.Add(new SqlParameter("@NumAluno", SqlDbType.Int) { Value = numAluno });

                        try
                        {
                            // Executar a stored procedure
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException sqlEx)
                        {
                            // Captura e detalha os erros gerados pela stored procedure
                            var errorMessage = "Erro ao executar a stored procedure 'sp_CriarTicket'. Detalhes do erro SQL:";
                            foreach (SqlError error in sqlEx.Errors)
                            {
                                errorMessage += $"\nMensagem: {error.Message}, Linha: {error.LineNumber}, Origem: {error.Procedure}";
                            }

                            // Lançar uma exceção personalizada com os detalhes
                            throw new ApplicationException(errorMessage, sqlEx);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções genéricas
                throw new ApplicationException("Erro ao gerar o ticket.", ex);
            }
        }


        public IEnumerable<SaldoConsumo> GetHistóricoConsumo(int NumeroAluno)
        {
            List<SaldoConsumo> SaldoConsumos = new List<SaldoConsumo>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = $@"SELECT Id, IdAluno, UsedValue, DataRegisto,DataAlter FROM SaldosConsumos 
                                    WHERE IdAluno = (SELECT IdAluno FROM TABALUNOS WHERE NUMALUNO = {NumeroAluno})";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SaldoConsumo saldoConsumo = new SaldoConsumo
                            {
                                Id = reader.GetInt32(0),
                                IdAluno = reader.GetInt32(1),
                                UsedValue = reader.GetDecimal(2),
                                DataRegisto = reader.GetDateTime(3),
                                DataAlter = reader.GetDateTime(4),
                            };

                            SaldoConsumos.Add(saldoConsumo);
                        }
                    }
                }

                return SaldoConsumos;
            }
        }

        public IEnumerable<SaldoConsumo> GetHistóricoConsumoById(int id)
        {
            List<SaldoConsumo> SaldoConsumos = new List<SaldoConsumo>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = $@"SELECT Id, IdAluno, UsedValue, DataRegisto,DataAlter FROM SaldosConsumos 
                                    WHERE Id = {id}";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SaldoConsumo saldoConsumo = new SaldoConsumo
                            {
                                Id = reader.GetInt32(0),
                                IdAluno = reader.GetInt32(1),
                                UsedValue = reader.GetDecimal(2),
                                DataRegisto = reader.GetDateTime(3),
                                DataAlter = reader.GetDateTime(4),
                            };

                            SaldoConsumos.Add(saldoConsumo);
                        }
                    }
                }

                return SaldoConsumos;
            }
        }

        public IEnumerable<SaldoConsumo> GetSaldoConsumo(int NumeroAluno)
        {
            List<SaldoConsumo> SaldoConsumos = new List<SaldoConsumo>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                string sqlQuery = $@"
                            declare @TOTALDEP AS NUMERIC(18,2), @TOTALCONSUMOS AS NUMERIC(18,2), @IDALUNO AS INT, @ValorArtigo  AS NUMERIC(18,2)   
                            SET @IDALUNO =  ( SELECT  IdALuno FROM  TABALUNOS WHERE NUMALUNO = {NumeroAluno} )
                            SET @ValorArtigo = (SELECT PRCVENDA FROM TABARTIGOS  WHERE CODIGO = 9722)
                            SET @TOTALDEP = (select isnull((select sum(DepValue) from DepSaldos where idaluno = @IDALUNO and Anulado = 0 and Deleted = 0), 0)) 
                            SET @TOTALCONSUMOS = (select isnull((select sum(UsedValue) from SaldosConsumos where idaluno = @IDALUNO and Anulado = 0 and Deleted = 0), 0)) 
                            select SALDO = @TOTALDEP - @TOTALCONSUMOS, ValorAlmoco = @ValorArtigo";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SaldoConsumo saldoConsumo = new SaldoConsumo
                            {
                                UsedValue = reader.GetDecimal(0),
                                ValorAlmo = reader.GetDecimal(1)
                            };

                            SaldoConsumos.Add(saldoConsumo);
                        }
                    }
                }
            }

            return SaldoConsumos;
        }

        public IEnumerable<Ticket> ListTicket(int numAluno)
        {
            List<Ticket> SaldoConsumos = new List<Ticket>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para buscar as classes
                        string sqlQuery = $@"
                        DECLARE  @ValorArtigo  AS NUMERIC(18,2)
                        SET @ValorArtigo = (SELECT PRCVENDA FROM TABARTIGOS  WHERE CODIGO = 9722)
                        SELECT TOP 1 *,ValorAlmoco = @ValorArtigo FROM TABTICKET
                        WHERE IdAluno =  (SELECT IDALUNO FROM TABALUNOS WHERE NUMALUNO = {numAluno})
                        ORDER BY Data DESC";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ticket saldoConsumo = new Ticket
                            {
                                Id = reader.GetInt32(0),
                                IdAluno = reader.GetInt32(1),
                                Nome = reader.GetString(2),
                                Data = reader.GetDateTime(3),
                                NumeroTicket = reader.GetInt32(4),
                                ValorAlmo = reader.GetDecimal(5)

                            };

                            SaldoConsumos.Add(saldoConsumo);
                        }
                    }
                }
            }

            return SaldoConsumos;
        }

        public void PrintTicket(int numAluno)
        {
            IEnumerable<Ticket> tickets = ListTicket(numAluno);

            // Verifica se há pelo menos um ticket
            Ticket ticket = tickets.FirstOrDefault();
            if (ticket == null) return;  // Se não houver tickets, não faz nada

            // Dados do ticket
            string ticketDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string ticketNumber = ticket.NumeroTicket.ToString();
            string studentName = ticket.Nome.ToString();
            string studentId = ticket.IdAluno.ToString();

            // Prepara o conteúdo do ticket
            string ticketContent = $@"
-----------------------------------
     Detalhe do Ticket 
-----------------------------------
     Sabor do Brasil 
-----------------------------------
Data: {ticketDate}
Número do Ticket: {ticketNumber}
Estudante: {ManipulateName(studentName)}
ID Aluno: {studentId}
Descrição: Almoço
Valor: 4.000 AOA
------------------------------
";

            // Configura o objeto PrintDocument
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                // Define a fonte para o texto
                Font font = new Font("Consolas", 12, FontStyle.Regular);

                // Define margens
                float topMargin = 20; // Ajuste o topo conforme necessário
                float leftMargin = e.MarginBounds.Left;  // Usa as margens definidas pela impressora

                // Divide o conteúdo do ticket em linhas (usando quebra de linha)
                string[] lines = ticketContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                // Define a altura inicial para o texto (topo ajustado)
                float y = topMargin;

                // Para cada linha, centraliza e desenha o texto
                foreach (string line in lines)
                {
                    // Mede a largura da linha
                    float textWidth = e.Graphics.MeasureString(line, font).Width;
                    float x = (e.PageBounds.Width - textWidth) / 2;  // Centraliza a linha horizontalmente

                    // Desenha a linha de texto
                    e.Graphics.DrawString(line, font, Brushes.Black, x, y);

                    // Ajusta a posição vertical para a próxima linha
                    y += font.GetHeight();  // Move para a linha seguinte
                }
            };

            // Define a impressora padrão
            string printerName = printDocument.PrinterSettings.PrinterName;
            if (!string.IsNullOrEmpty(printerName))
            {
                Console.WriteLine($"Imprimindo na impressora: {printerName}");
                printDocument.Print();  // Envia diretamente para a impressora
            }
            else
            {
                Console.WriteLine("Nenhuma impressora padrão configurada.");
            }
        }

        // Função para manipular o nome (pegar primeiro e último nome)
        private string ManipulateName(string fullName)
        {
            var nameParts = fullName.Split(' ');
            if (nameParts.Length > 1)
            {
                return $"{nameParts[0]} {nameParts[nameParts.Length - 1]}";
            }
            return fullName;  // Retorna o nome completo se não houver espaço
        }

    }
}