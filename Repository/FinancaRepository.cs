using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemasdeTarefas.Repository
{
    public class FinancaRepository : IFinancaRepository
    {
        private readonly string _connectionString;

        public FinancaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<AlunoDossierCT>> GetAlunoDossierCTByIdAlunoDossier(int idAlunoDossier)
        {
            var lista = new List<AlunoDossierCT>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                SELECT 
                    IdAlunoDossierCT,
                    IdAlunoDossier,
                    IdAnoLectivo,
                    IdContrato,
                    NomeContrato,
                    ValorEmissao,
                    DataIni,
                    DataFim,
                    ValorPrevisto,
                    IsBolseiro,
                    IdBolsa,
                    TemOutroDesc,
                    TxDesc,
                    NomeDesc,
                    RemoveDesc,
                    IsAnoAnterior,
                    DataRegisto,
                    DataAlter,
                    TimeRegist,
                    TimeAlter,
                    IdUserRegisto,
                    IdUserAlter,
                    DateDeleted,
                    IdUserDel,
                    Deleted,
                    TimeDeleted,
                    IsMulta
                FROM AlunoDossierCT
                WHERE IdAlunoDossier = @IdAlunoDossier AND Deleted <> 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdAlunoDossier", idAlunoDossier);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lista.Add(new AlunoDossierCT
                                {
                                    IdAlunoDossierCT = reader.GetInt32(0),
                                    IdAlunoDossier = reader.GetInt32(1),
                                    IdAnoLectivo = reader.GetInt32(2),
                                    IdContrato = reader.GetInt32(3),
                                    NomeContrato = reader.GetString(4),
                                    ValorEmissao = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                                    DataIni = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                    DataFim = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                    ValorPrevisto = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                                    IsBolseiro = reader.IsDBNull(9) ? (bool?)null : reader.GetBoolean(9),
                                    IdBolsa = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                    TemOutroDesc = reader.IsDBNull(11) ? (bool?)null : reader.GetBoolean(11),
                                    TxDesc = reader.IsDBNull(12) ? (decimal?)null : reader.GetDecimal(12),
                                    NomeDesc = reader.IsDBNull(13) ? null : reader.GetString(13),
                                    RemoveDesc = reader.IsDBNull(14) ? (bool?)null : reader.GetBoolean(14),
                                    IsAnoAnterior = reader.IsDBNull(15) ? (bool?)null : reader.GetBoolean(15),
                                    DataRegisto = reader.IsDBNull(16) ? (DateTime?)null : reader.GetDateTime(16),
                                    DataAlter = reader.IsDBNull(17) ? (DateTime?)null : reader.GetDateTime(17),
                                    TimeRegist = reader.IsDBNull(18) ? null : reader.GetString(18),
                                    TimeAlter = reader.IsDBNull(19) ? null : reader.GetString(19),
                                    IdUserRegisto = reader.IsDBNull(20) ? (int?)null : reader.GetInt32(20),
                                    IdUserAlter = reader.IsDBNull(21) ? (int?)null : reader.GetInt32(21),
                                    DateDeleted = reader.IsDBNull(22) ? (DateTime?)null : reader.GetDateTime(22),
                                    IdUserDel = reader.IsDBNull(23) ? (int?)null : reader.GetInt32(23),
                                    Deleted = reader.GetBoolean(24), // se for NOT NULL no banco
                                    TimeDeleted = reader.IsDBNull(25) ? null : reader.GetString(25),
                                    IsMulta = reader.IsDBNull(26) ? (bool?)null : reader.GetBoolean(26)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar AlunoDossierCT.", ex);
            }

            return lista;
        }

        public async Task<IEnumerable<AlunoDossierLin>> GetAlunoDossierLinCTByIdAlunoDossierCT(int IdAlunoDossierCT)
        {
            var lista = new List<AlunoDossierLin>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                SELECT 
                    IdAlunoDossierLin,
                    IdAlunoDossierCT,
                    IdAlunoDossier,
                    IsMatricula,
                    IsConfirmacao,
                    IsContrato,
                    IsArtigo,
                    IsMulta,
                    IdContrato,
                    NomeContrato,
                    IdArtigo,
                    ValorEmissao,
                    Quant,
                    ValorUnitPrevisto,
                    SubTotal,
                    IsBolseiro,
                    IdBolsa,
                    TemOutroDesc,
                    TxDesc,
                    ValorDesc,
                    TxDesc2,
                    ValorDesc2,
                    NomeDesc,
                    DataLimite,
                    ValorPago,
                    Pago,
                    DataPag,
                    Anulado,
                    Inactivo,
                    Facturado,
                    IdAlunoFacturacaoLin,
                    IdLinFactura,
                    NumFactura,
                    AplicouMulta,
                    IdMultaCond,
                    ValorMulta,
                    IdRefCTIfMulta,
                    IdAluno,
                    IdDocOrigemCC,
                    DataRegisto,
                    DataAlter,
                    TimeRegist,
                    TimeAlter,
                    IdUserRegisto,
                    IdUserAlter,
                    DateDeleted,
                    IdUserDel,
                    Deleted,
                    TimeDeleted,
                    IsFromRecibo,
                    IdRecibo
                FROM AlunoDossierLin
                WHERE IdAlunoDossierCT = @IdAlunoDossierCT AND Anulado <> 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdAlunoDossierCT", IdAlunoDossierCT);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lista.Add(new AlunoDossierLin
                                {
                                    IdAlunoDossierLin = reader.GetInt32(0),
                                    IdAlunoDossierCT = reader.GetInt32(1),
                                    IdAlunoDossier = reader.GetInt32(2),
                                    IsMatricula = reader.GetBoolean(3),
                                    IsConfirmacao = reader.GetBoolean(4),
                                    IsContrato = reader.GetBoolean(5),
                                    IsArtigo = reader.GetBoolean(6),
                                    IsMulta = reader.GetBoolean(7),
                                    IdContrato = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    NomeContrato = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    IdArtigo = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                    ValorEmissao = reader.IsDBNull(11) ? (decimal?)null : reader.GetDecimal(11),
                                    Quant = reader.GetInt32(12),
                                    ValorUnitPrevisto = reader.IsDBNull(13) ? (decimal?)null : reader.GetDecimal(13),
                                    SubTotal = reader.IsDBNull(14) ? (decimal?)null : reader.GetDecimal(14),
                                    IsBolseiro = reader.GetBoolean(15),
                                    IdBolsa = reader.IsDBNull(16) ? (int?)null : reader.GetInt32(16),
                                    TemOutroDesc = reader.GetBoolean(17),
                                    TxDesc = reader.GetDecimal(18),
                                    ValorDesc = reader.GetDecimal(19),
                                    TxDesc2 = reader.GetDecimal(20),
                                    ValorDesc2 = reader.GetDecimal(21),
                                    NomeDesc = reader.IsDBNull(22) ? null : reader.GetString(22),
                                    DataLimite = reader.GetDateTime(23),
                                    ValorPago = reader.IsDBNull(24) ? (decimal?)null : reader.GetDecimal(24),
                                    Pago = reader.GetBoolean(25),
                                    DataPag = reader.IsDBNull(26) ? (DateTime?)null : reader.GetDateTime(26),
                                    Anulado = reader.GetBoolean(27),
                                    Inactivo = reader.GetBoolean(28),
                                    Facturado = reader.GetBoolean(29),
                                    IdAlunoFacturacaoLin = reader.IsDBNull(30) ? (int?)null : reader.GetInt32(30),
                                    IdLinFactura = reader.IsDBNull(31) ? (int?)null : reader.GetInt32(31),
                                    NumFactura = reader.IsDBNull(32) ? null : reader.GetString(32),
                                    AplicouMulta = reader.GetBoolean(33),
                                    IdMultaCond = reader.IsDBNull(34) ? (int?)null : reader.GetInt32(34),
                                    ValorMulta = reader.IsDBNull(35) ? (decimal?)null : reader.GetDecimal(35),
                                    IdRefCTIfMulta = reader.GetInt32(36),
                                    IdAluno = reader.GetInt32(37),
                                    IdDocOrigemCC = reader.GetInt32(38),
                                    DataRegisto = reader.IsDBNull(39) ? (DateTime?)null : reader.GetDateTime(39),
                                    DataAlter = reader.IsDBNull(40) ? (DateTime?)null : reader.GetDateTime(40),
                                    TimeRegist = reader.IsDBNull(41) ? null : reader.GetString(41),
                                    TimeAlter = reader.IsDBNull(42) ? null : reader.GetString(42),
                                    IdUserRegisto = reader.IsDBNull(43) ? (int?)null : reader.GetInt32(43),
                                    IdUserAlter = reader.IsDBNull(44) ? (int?)null : reader.GetInt32(44),
                                    DateDeleted = reader.IsDBNull(45) ? (DateTime?)null : reader.GetDateTime(45),
                                    IdUserDel = reader.IsDBNull(46) ? (int?)null : reader.GetInt32(46),
                                    Deleted = reader.IsDBNull(47) ? (bool?)null : reader.GetBoolean(47),
                                    TimeDeleted = reader.IsDBNull(48) ? null : reader.GetString(48),
                                    IsFromRecibo = reader.GetBoolean(49),
                                    IdRecibo = reader.IsDBNull(50) ? (int?)null : reader.GetInt32(50)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar AlunoDossierLin.", ex);
            }

            return lista;
        }
        public async Task<IEnumerable<AlunoDossierLin>> GetContratosPagosOUNaoPagos(int IdAlunoDossierCT, bool pago)
        {
            var lista = new List<AlunoDossierLin>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                SELECT 
                    IdAlunoDossierLin,
                    IdAlunoDossierCT,
                    IdAlunoDossier,
                    IsMatricula,
                    IsConfirmacao,
                    IsContrato,
                    IsArtigo,
                    IsMulta,
                    IdContrato,
                    NomeContrato,
                    IdArtigo,
                    ValorEmissao,
                    Quant,
                    ValorUnitPrevisto,
                    SubTotal,
                    IsBolseiro,
                    IdBolsa,
                    TemOutroDesc,
                    TxDesc,
                    ValorDesc,
                    TxDesc2,
                    ValorDesc2,
                    NomeDesc,
                    DataLimite,
                    ValorPago,
                    Pago,
                    DataPag,
                    Anulado,
                    Inactivo,
                    Facturado,
                    IdAlunoFacturacaoLin,
                    IdLinFactura,
                    NumFactura,
                    AplicouMulta,
                    IdMultaCond,
                    ValorMulta,
                    IdRefCTIfMulta,
                    IdAluno,
                    IdDocOrigemCC,
                    DataRegisto,
                    DataAlter,
                    TimeRegist,
                    TimeAlter,
                    IdUserRegisto,
                    IdUserAlter,
                    DateDeleted,
                    IdUserDel,
                    Deleted,
                    TimeDeleted,
                    IsFromRecibo,
                    IdRecibo
                FROM AlunoDossierLin
                WHERE IdAlunoDossierCT = @IdAlunoDossierCT AND Anulado <> 1 AND Pago = @Pago";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdAlunoDossierCT", IdAlunoDossierCT);
                        cmd.Parameters.AddWithValue("@Pago", pago);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lista.Add(new AlunoDossierLin
                                {
                                    IdAlunoDossierLin = reader.GetInt32(0),
                                    IdAlunoDossierCT = reader.GetInt32(1),
                                    IdAlunoDossier = reader.GetInt32(2),
                                    IsMatricula = reader.GetBoolean(3),
                                    IsConfirmacao = reader.GetBoolean(4),
                                    IsContrato = reader.GetBoolean(5),
                                    IsArtigo = reader.GetBoolean(6),
                                    IsMulta = reader.GetBoolean(7),
                                    IdContrato = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    NomeContrato = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    IdArtigo = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                    ValorEmissao = reader.IsDBNull(11) ? (decimal?)null : reader.GetDecimal(11),
                                    Quant = reader.GetInt32(12),
                                    ValorUnitPrevisto = reader.IsDBNull(13) ? (decimal?)null : reader.GetDecimal(13),
                                    SubTotal = reader.IsDBNull(14) ? (decimal?)null : reader.GetDecimal(14),
                                    IsBolseiro = reader.GetBoolean(15),
                                    IdBolsa = reader.IsDBNull(16) ? (int?)null : reader.GetInt32(16),
                                    TemOutroDesc = reader.GetBoolean(17),
                                    TxDesc = reader.GetDecimal(18),
                                    ValorDesc = reader.GetDecimal(19),
                                    TxDesc2 = reader.GetDecimal(20),
                                    ValorDesc2 = reader.GetDecimal(21),
                                    NomeDesc = reader.IsDBNull(22) ? null : reader.GetString(22),
                                    DataLimite = reader.GetDateTime(23),
                                    ValorPago = reader.IsDBNull(24) ? (decimal?)null : reader.GetDecimal(24),
                                    Pago = reader.GetBoolean(25),
                                    DataPag = reader.IsDBNull(26) ? (DateTime?)null : reader.GetDateTime(26),
                                    Anulado = reader.GetBoolean(27),
                                    Inactivo = reader.GetBoolean(28),
                                    Facturado = reader.GetBoolean(29),
                                    IdAlunoFacturacaoLin = reader.IsDBNull(30) ? (int?)null : reader.GetInt32(30),
                                    IdLinFactura = reader.IsDBNull(31) ? (int?)null : reader.GetInt32(31),
                                    NumFactura = reader.IsDBNull(32) ? null : reader.GetString(32),
                                    AplicouMulta = reader.GetBoolean(33),
                                    IdMultaCond = reader.IsDBNull(34) ? (int?)null : reader.GetInt32(34),
                                    ValorMulta = reader.IsDBNull(35) ? (decimal?)null : reader.GetDecimal(35),
                                    IdRefCTIfMulta = reader.GetInt32(36),
                                    IdAluno = reader.GetInt32(37),
                                    IdDocOrigemCC = reader.GetInt32(38),
                                    DataRegisto = reader.IsDBNull(39) ? (DateTime?)null : reader.GetDateTime(39),
                                    DataAlter = reader.IsDBNull(40) ? (DateTime?)null : reader.GetDateTime(40),
                                    TimeRegist = reader.IsDBNull(41) ? null : reader.GetString(41),
                                    TimeAlter = reader.IsDBNull(42) ? null : reader.GetString(42),
                                    IdUserRegisto = reader.IsDBNull(43) ? (int?)null : reader.GetInt32(43),
                                    IdUserAlter = reader.IsDBNull(44) ? (int?)null : reader.GetInt32(44),
                                    DateDeleted = reader.IsDBNull(45) ? (DateTime?)null : reader.GetDateTime(45),
                                    IdUserDel = reader.IsDBNull(46) ? (int?)null : reader.GetInt32(46),
                                    Deleted = reader.IsDBNull(47) ? (bool?)null : reader.GetBoolean(47),
                                    TimeDeleted = reader.IsDBNull(48) ? null : reader.GetString(48),
                                    IsFromRecibo = reader.GetBoolean(49),
                                    IdRecibo = reader.IsDBNull(50) ? (int?)null : reader.GetInt32(50)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar AlunoDossierLin.", ex);
            }

            return lista;
        }
        public async Task<IEnumerable<AlunoDossierLin>> GetContratosPagosOUNaoPagosByYear(
             int? ano = null,
             bool? pago = null,
             int? idTurma = null,
             int? idClasse = null,
             int? numAluno = null)
        {
            var lista = new List<AlunoDossierLin>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_GetAlunoDossierLinPagamentos", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Ano", (object?)ano ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Pago", (object?)pago ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IdTurma", (object?)idTurma ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IdClasse", (object?)idClasse ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NumAluno", (object?)numAluno ?? DBNull.Value);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lista.Add(new AlunoDossierLin
                                {
                                    // colunas finais (Nome, NumAluno, NomeClasse, IdTurma, IdClasse)
                                    Nome = reader.IsDBNull(51) ? null : reader.GetString(51),
                                    NumALuno = reader.IsDBNull(52) ? 0 : reader.GetInt32(52),
                                    NomeClasse = reader.IsDBNull(53) ? null : reader.GetString(53),
                                   

                                    // resto das colunas (em ordem)
                                    IdAlunoDossierLin = reader.GetInt32(0),
                                    IdAlunoDossierCT = reader.GetInt32(1),
                                    IdAlunoDossier = reader.GetInt32(2),
                                    IsMatricula = reader.GetBoolean(3),
                                    IsConfirmacao = reader.GetBoolean(4),
                                    IsContrato = reader.GetBoolean(5),
                                    IsArtigo = reader.GetBoolean(6),
                                    IsMulta = reader.GetBoolean(7),
                                    IdContrato = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    NomeContrato = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    IdArtigo = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                    ValorEmissao = reader.IsDBNull(11) ? (decimal?)null : reader.GetDecimal(11),
                                    Quant = reader.GetInt32(12),
                                    ValorUnitPrevisto = reader.IsDBNull(13) ? (decimal?)null : reader.GetDecimal(13),
                                    SubTotal = reader.IsDBNull(14) ? (decimal?)null : reader.GetDecimal(14),
                                    IsBolseiro = reader.GetBoolean(15),
                                    IdBolsa = reader.IsDBNull(16) ? (int?)null : reader.GetInt32(16),
                                    TemOutroDesc = reader.GetBoolean(17),
                                    TxDesc = reader.IsDBNull(18) ? (decimal?)null : reader.GetDecimal(18),
                                    ValorDesc = reader.IsDBNull(19) ? (decimal?)null : reader.GetDecimal(19),
                                    TxDesc2 = reader.IsDBNull(20) ? (decimal?)null : reader.GetDecimal(20),
                                    ValorDesc2 = reader.IsDBNull(21) ? (decimal?)null : reader.GetDecimal(21),
                                    NomeDesc = reader.IsDBNull(22) ? null : reader.GetString(22),
                                    DataLimite = reader.GetDateTime(23),
                                    ValorPago = reader.IsDBNull(24) ? (decimal?)null : reader.GetDecimal(24),
                                    Pago = reader.GetBoolean(25),
                                    DataPag = reader.IsDBNull(26) ? (DateTime?)null : reader.GetDateTime(26),
                                    Anulado = reader.GetBoolean(27),
                                    Inactivo = reader.GetBoolean(28),
                                    Facturado = reader.GetBoolean(29),
                                    IdAlunoFacturacaoLin = reader.IsDBNull(30) ? (int?)null : reader.GetInt32(30),
                                    IdLinFactura = reader.IsDBNull(31) ? (int?)null : reader.GetInt32(31),
                                    NumFactura = reader.IsDBNull(32) ? null : reader.GetString(32),
                                    AplicouMulta = reader.GetBoolean(33),
                                    IdMultaCond = reader.IsDBNull(34) ? (int?)null : reader.GetInt32(34),
                                    ValorMulta = reader.IsDBNull(35) ? (decimal?)null : reader.GetDecimal(35),
                                    IdRefCTIfMulta = reader.GetInt32(36),
                                    IdAluno = reader.GetInt32(37),
                                    IdDocOrigemCC = reader.GetInt32(38),
                                    DataRegisto = reader.IsDBNull(39) ? (DateTime?)null : reader.GetDateTime(39),
                                    DataAlter = reader.IsDBNull(40) ? (DateTime?)null : reader.GetDateTime(40),
                                    TimeRegist = reader.IsDBNull(41) ? null : reader.GetString(41),
                                    TimeAlter = reader.IsDBNull(42) ? null : reader.GetString(42),
                                    IdUserRegisto = reader.IsDBNull(43) ? (int?)null : reader.GetInt32(43),
                                    IdUserAlter = reader.IsDBNull(44) ? (int?)null : reader.GetInt32(44),
                                    DateDeleted = reader.IsDBNull(45) ? (DateTime?)null : reader.GetDateTime(45),
                                    IdUserDel = reader.IsDBNull(46) ? (int?)null : reader.GetInt32(46),
                                    Deleted = reader.IsDBNull(47) ? (bool?)null : reader.GetBoolean(47),
                                    TimeDeleted = reader.IsDBNull(48) ? null : reader.GetString(48),
                                    IsFromRecibo = reader.GetBoolean(49),
                                    IdRecibo = reader.IsDBNull(50) ? (int?)null : reader.GetInt32(50)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar AlunoDossierLin.", ex);
            }

            return lista;
        }

        public async Task<IEnumerable<CustomerInvoice>> GetCustomerInvoicesByAnoAndNumAluno(int? ano = null, int? numAluno = null)
        {
            var lista = new List<CustomerInvoice>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {

                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_GetCustomerInvoice", connection))

                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Ano", ano);
                        cmd.Parameters.AddWithValue("@NumAluno", numAluno);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var invoice = new CustomerInvoice
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Ano = reader.GetInt32(reader.GetOrdinal("Ano")),
                                    DocNo = reader.IsDBNull(reader.GetOrdinal("DocNo")) ? null : reader.GetString(reader.GetOrdinal("DocNo")),
                                    NomeAluno = reader.IsDBNull(reader.GetOrdinal("NomeAluno")) ? null : reader.GetString(reader.GetOrdinal("NomeAluno")),
                                    SerieId = reader.IsDBNull(reader.GetOrdinal("SerieId")) ? null : reader.GetString(reader.GetOrdinal("SerieId")),
                                    SerieType = reader.IsDBNull(reader.GetOrdinal("SerieType")) ? null : reader.GetString(reader.GetOrdinal("SerieType")),
                                    CreditAmount = reader.GetDecimal("CreditAmount"),
                                    Ciclo = reader.IsDBNull(reader.GetOrdinal("Ciclo")) ? null : reader.GetString(reader.GetOrdinal("Ciclo")),
                                    Classe = reader.IsDBNull(reader.GetOrdinal("Classe")) ? null : reader.GetString(reader.GetOrdinal("Classe")),
                                    DateChange = reader.IsDBNull(reader.GetOrdinal("DateChange")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateChange")),
                                    Deleted = reader.IsDBNull(reader.GetOrdinal("Deleted")) ? false : reader.GetBoolean(reader.GetOrdinal("Deleted"))
                                };

                                lista.Add(invoice);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar CustomerInvoice por Ano e NumAluno.", ex);
            }

            return lista;
        }


        public async Task<IEnumerable<AlunoDossier>> ListAlunoDossier(int number)
        {
            var alunos = new List<AlunoDossier>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @$"
                            SELECT 
                                IdAlunoDossier,
                                IdAluno,
                                NumAluno,
                                Nome,
                                IdCustomer,
                                IdStatusAluno,
                                IdAno,
                                Ano,
                                IdBolsa,
                                IdMatricula,
                                IdCiclo,
                                NomeCiclo,
                                IdClasse,
                                NomeClasse
                            FROM AlunoDossier
                            WHERE NumAluno = {number} AND Deleted <> 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            alunos.Add(new AlunoDossier
                            {
                                IdAlunoDossier = reader.GetInt32(0),
                                IdAluno = reader.GetInt32(1),
                                NumAluno = reader.GetInt32(2),
                                Nome = reader.GetString(3),
                                IdCustomer = reader.GetInt32(4),
                                IdStatusAluno = reader.GetInt32(5),
                                IdAno = reader.GetInt32(6),
                                Ano = reader.GetInt32(7),
                                IdBolsa = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                                IdMatricula = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                                IdCiclo = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                                NomeCiclo = reader.IsDBNull(11) ? null : reader.GetString(11),
                                IdClasse = reader.IsDBNull(12) ? null : reader.GetInt32(12),
                                NomeClasse = reader.IsDBNull(13) ? null : reader.GetString(13),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar AlunoDossier.", ex);
            }

            return alunos;
        }


    }
}
