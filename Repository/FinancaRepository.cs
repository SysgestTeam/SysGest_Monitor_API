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
                                    IdStudentDossierCT = reader.GetInt32(0),
                                    IdStudentDossier = reader.GetInt32(1),
                                    AcademicYearId = reader.GetInt32(2),
                                    ContractId = reader.GetInt32(3),
                                    ContractName = reader.GetString(4),
                                    IssueValue = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                                    StartDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                    EndDate = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                    ExpectedValue = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                                    IsScholarship = reader.IsDBNull(9) ? (bool?)null : reader.GetBoolean(9),
                                    ScholarshipId = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                    HasOtherDiscount = reader.IsDBNull(11) ? (bool?)null : reader.GetBoolean(11),
                                    DiscountRate = reader.IsDBNull(12) ? (decimal?)null : reader.GetDecimal(12),
                                    DiscountName = reader.IsDBNull(13) ? null : reader.GetString(13),
                                    RemoveDiscount = reader.IsDBNull(14) ? (bool?)null : reader.GetBoolean(14),
                                    IsPreviousYear = reader.IsDBNull(15) ? (bool?)null : reader.GetBoolean(15),
                                    RegisterDate = reader.IsDBNull(16) ? (DateTime?)null : reader.GetDateTime(16),
                                    UpdateDate = reader.IsDBNull(17) ? (DateTime?)null : reader.GetDateTime(17),
                                    RegisterTime = reader.IsDBNull(18) ? null : reader.GetString(18),
                                    UpdateTime = reader.IsDBNull(19) ? null : reader.GetString(19),
                                    RegisterUserId = reader.IsDBNull(20) ? (int?)null : reader.GetInt32(20),
                                    UpdateUserId = reader.IsDBNull(21) ? (int?)null : reader.GetInt32(21),
                                    DeletedDate = reader.IsDBNull(22) ? (DateTime?)null : reader.GetDateTime(22),
                                    DeletedUserId = reader.IsDBNull(23) ? (int?)null : reader.GetInt32(23),
                                    Deleted = reader.GetBoolean(24), // se for NOT NULL no banco
                                    DeletedTime = reader.IsDBNull(25) ? null : reader.GetString(25),
                                    IsFine = reader.IsDBNull(26) ? (bool?)null : reader.GetBoolean(26)
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

        public async Task<IEnumerable<AlunoDossierLin>> GetAlunoDossierLinCTByIdAlunoDossierCT(int StudentDossierCTId)
        {
            var lista = new List<AlunoDossierLin>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT  
                            ADL.IdAlunoDossierLin,
                            ADL.IdAlunoDossierCT,
                            ADL.IdAlunoDossier,
                            ADL.IsMatricula,
                            ADL.IsConfirmacao,
                            ADL.IsContrato,
                            ADL.IsArtigo,
                            ADL.IsMulta,
                            ADL.IdContrato,
                            ADL.NomeContrato,
                            ADL.IdArtigo,
                            ADL.ValorEmissao,
                            ADL.Quant,
                            ADL.ValorUnitPrevisto,
                            ADL.SubTotal,
                            ADL.IsBolseiro,
                            ADL.IdBolsa,
                            ADL.TemOutroDesc,
                            ADL.TxDesc,
                            ADL.ValorDesc,
                            ADL.TxDesc2,
                            ADL.ValorDesc2,
                            ADL.NomeDesc,
                            ADL.DataLimite,
                            ADL.ValorPago,
                            ADL.Pago,
                            ADL.DataPag,
                            ADL.Anulado,
                            ADL.Inactivo,
                            ADL.Facturado,
                            ADL.IdAlunoFacturacaoLin,
                            ADL.IdLinFactura,
                            ADL.NumFactura,
                            ADL.AplicouMulta,
                            ADL.IdMultaCond,
                            ADL.ValorMulta,
                            ADL.IdRefCTIfMulta,
                            ADL.IdAluno,
                            ADL.IdDocOrigemCC,
                            ADL.DataRegisto,
                            ADL.DataAlter,
                            ADL.TimeRegist,
                            ADL.TimeAlter,
                            ADL.IdUserRegisto,
                            ADL.IdUserAlter,
                            ADL.DateDeleted,
                            ADL.IdUserDel,
                            ADL.Deleted,
                            ADL.TimeDeleted,
                            ADL.IsFromRecibo,
                            ADL.IdRecibo,
	                        AD.Nome,
	                        AD.NumAluno,
	                        AD.NomeClasse
                        FROM AlunoDossierLin ADL
                        JOIN AlunoDossier AD
                        ON AD.IdAlunoDossier =  ADL.IdAlunoDossier
                WHERE IdAlunoDossierCT = @IdAlunoDossierCT AND Anulado <> 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdAlunoDossierCT", StudentDossierCTId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                lista.Add(new AlunoDossierLin
                                {
                                    StudentDossierLineId = reader.GetInt32(0),
                                    StudentDossierCTId = reader.GetInt32(1),
                                    StudentDossierId = reader.GetInt32(2),
                                    IsEnrollment = reader.GetBoolean(3),
                                    IsConfirmation = reader.GetBoolean(4),
                                    IsContract = reader.GetBoolean(5),
                                    IsArticle = reader.GetBoolean(6),
                                    IsPenalty = reader.GetBoolean(7),
                                    ContractId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    ContractName = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    ArticleId = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                    IssueAmount = reader.IsDBNull(11) ? (decimal?)null : reader.GetDecimal(11),
                                    Quantity = reader.GetInt32(12),
                                    ExpectedUnitValue = reader.IsDBNull(13) ? (decimal?)null : reader.GetDecimal(13),
                                    SubTotal = reader.IsDBNull(14) ? (decimal?)null : reader.GetDecimal(14),
                                    IsScholarshipHolder = reader.GetBoolean(15),
                                    ScholarshipId = reader.IsDBNull(16) ? (int?)null : reader.GetInt32(16),
                                    HasOtherDiscount = reader.GetBoolean(17),
                                    DiscountRate = reader.GetDecimal(18),
                                    DiscountValue = reader.GetDecimal(19),
                                    DiscountRate2 = reader.GetDecimal(20),
                                    DiscountValue2 = reader.GetDecimal(21),
                                    DiscountName = reader.IsDBNull(22) ? null : reader.GetString(22),
                                    DueDate = reader.GetDateTime(23),
                                    AmountPaid = reader.IsDBNull(24) ? (decimal?)null : reader.GetDecimal(24),
                                    Paid = reader.GetBoolean(25),
                                    PaymentDate = reader.IsDBNull(26) ? (DateTime?)null : reader.GetDateTime(26),
                                    Canceled = reader.GetBoolean(27),
                                    Inactive = reader.GetBoolean(28),
                                    Invoiced = reader.GetBoolean(29),
                                    StudentInvoiceLineId = reader.IsDBNull(30) ? (int?)null : reader.GetInt32(30),
                                    InvoiceLineId = reader.IsDBNull(31) ? (int?)null : reader.GetInt32(31),
                                    InvoiceNumber = reader.IsDBNull(32) ? null : reader.GetString(32),
                                    AppliedPenalty = reader.GetBoolean(33),
                                    PenaltyConditionId = reader.IsDBNull(34) ? (int?)null : reader.GetInt32(34),
                                    PenaltyAmount = reader.IsDBNull(35) ? (decimal?)null : reader.GetDecimal(35),
                                    RefCTIdIfPenalty = reader.GetInt32(36),
                                    StudentId = reader.GetInt32(37),
                                    OriginDocCCId = reader.GetInt32(38),
                                    CreatedAt = reader.IsDBNull(39) ? (DateTime?)null : reader.GetDateTime(39),
                                    UpdatedAt = reader.IsDBNull(40) ? (DateTime?)null : reader.GetDateTime(40),
                                    TimeCreated = reader.IsDBNull(41) ? null : reader.GetString(41),
                                    TimeUpdated = reader.IsDBNull(42) ? null : reader.GetString(42),
                                    CreatedByUserId = reader.IsDBNull(43) ? (int?)null : reader.GetInt32(43),
                                    UpdatedByUserId = reader.IsDBNull(44) ? (int?)null : reader.GetInt32(44),
                                    DeletedAt = reader.IsDBNull(45) ? (DateTime?)null : reader.GetDateTime(45),
                                    DeletedByUserId = reader.IsDBNull(46) ? (int?)null : reader.GetInt32(46),
                                    IsDeleted = reader.IsDBNull(47) ? (bool?)null : reader.GetBoolean(47),
                                    TimeDeleted = reader.IsDBNull(48) ? null : reader.GetString(48),
                                    IsFromReceipt = reader.GetBoolean(49),
                                    ReceiptId = reader.IsDBNull(50) ? (int?)null : reader.GetInt32(50),
                                    Name = reader.GetString(51),
                                    StudentNumber = reader.GetInt32(52),
                                    ClassName = reader.GetString(53)
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
                                    StudentDossierLineId = reader.GetInt32(0),
                                    StudentDossierCTId = reader.GetInt32(1),
                                    StudentDossierId = reader.GetInt32(2),
                                    IsEnrollment = reader.GetBoolean(3),
                                    IsConfirmation = reader.GetBoolean(4),
                                    IsContract = reader.GetBoolean(5),
                                    IsArticle = reader.GetBoolean(6),
                                    IsPenalty = reader.GetBoolean(7),
                                    ContractId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    ContractName = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    ArticleId = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                    IssueAmount = reader.IsDBNull(11) ? (decimal?)null : reader.GetDecimal(11),
                                    Quantity = reader.GetInt32(12),
                                    ExpectedUnitValue = reader.IsDBNull(13) ? (decimal?)null : reader.GetDecimal(13),
                                    SubTotal = reader.IsDBNull(14) ? (decimal?)null : reader.GetDecimal(14),
                                    IsScholarshipHolder = reader.GetBoolean(15),
                                    ScholarshipId = reader.IsDBNull(16) ? (int?)null : reader.GetInt32(16),
                                    HasOtherDiscount = reader.GetBoolean(17),
                                    DiscountRate = reader.GetDecimal(18),
                                    DiscountValue = reader.GetDecimal(19),
                                    DiscountRate2 = reader.GetDecimal(20),
                                    DiscountValue2 = reader.GetDecimal(21),
                                    DiscountName = reader.IsDBNull(22) ? null : reader.GetString(22),
                                    DueDate = reader.GetDateTime(23),
                                    AmountPaid = reader.IsDBNull(24) ? (decimal?)null : reader.GetDecimal(24),
                                    Paid = reader.GetBoolean(25),
                                    PaymentDate = reader.IsDBNull(26) ? (DateTime?)null : reader.GetDateTime(26),
                                    Canceled = reader.GetBoolean(27),
                                    Inactive = reader.GetBoolean(28),
                                    Invoiced = reader.GetBoolean(29),
                                    StudentInvoiceLineId = reader.IsDBNull(30) ? (int?)null : reader.GetInt32(30),
                                    InvoiceLineId = reader.IsDBNull(31) ? (int?)null : reader.GetInt32(31),
                                    InvoiceNumber = reader.IsDBNull(32) ? null : reader.GetString(32),
                                    AppliedPenalty = reader.GetBoolean(33),
                                    PenaltyConditionId = reader.IsDBNull(34) ? (int?)null : reader.GetInt32(34),
                                    PenaltyAmount = reader.IsDBNull(35) ? (decimal?)null : reader.GetDecimal(35),
                                    RefCTIdIfPenalty = reader.GetInt32(36),
                                    StudentId = reader.GetInt32(37),
                                    OriginDocCCId = reader.GetInt32(38),
                                    CreatedAt = reader.IsDBNull(39) ? (DateTime?)null : reader.GetDateTime(39),
                                    UpdatedAt = reader.IsDBNull(40) ? (DateTime?)null : reader.GetDateTime(40),
                                    TimeCreated = reader.IsDBNull(41) ? null : reader.GetString(41),
                                    TimeUpdated = reader.IsDBNull(42) ? null : reader.GetString(42),
                                    CreatedByUserId = reader.IsDBNull(43) ? (int?)null : reader.GetInt32(43),
                                    UpdatedByUserId = reader.IsDBNull(44) ? (int?)null : reader.GetInt32(44),
                                    DeletedAt = reader.IsDBNull(45) ? (DateTime?)null : reader.GetDateTime(45),
                                    DeletedByUserId = reader.IsDBNull(46) ? (int?)null : reader.GetInt32(46),
                                    IsDeleted = reader.IsDBNull(47) ? (bool?)null : reader.GetBoolean(47),
                                    TimeDeleted = reader.IsDBNull(48) ? null : reader.GetString(48),
                                    IsFromReceipt = reader.GetBoolean(49),
                                    ReceiptId = reader.IsDBNull(50) ? (int?)null : reader.GetInt32(50)
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
                                    StudentDossierLineId = reader.GetInt32(0),
                                    StudentDossierCTId = reader.GetInt32(1),
                                    StudentDossierId = reader.GetInt32(2),
                                    IsEnrollment = reader.GetBoolean(3),
                                    IsConfirmation = reader.GetBoolean(4),
                                    IsContract = reader.GetBoolean(5),
                                    IsArticle = reader.GetBoolean(6),
                                    IsPenalty = reader.GetBoolean(7),
                                    ContractId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    ContractName = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    ArticleId = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                    IssueAmount = reader.IsDBNull(11) ? (decimal?)null : reader.GetDecimal(11),
                                    Quantity = reader.GetInt32(12),
                                    ExpectedUnitValue = reader.IsDBNull(13) ? (decimal?)null : reader.GetDecimal(13),
                                    SubTotal = reader.IsDBNull(14) ? (decimal?)null : reader.GetDecimal(14),
                                    IsScholarshipHolder = reader.GetBoolean(15),
                                    ScholarshipId = reader.IsDBNull(16) ? (int?)null : reader.GetInt32(16),
                                    HasOtherDiscount = reader.GetBoolean(17),
                                    DiscountRate = reader.GetDecimal(18),
                                    DiscountValue = reader.GetDecimal(19),
                                    DiscountRate2 = reader.GetDecimal(20),
                                    DiscountValue2 = reader.GetDecimal(21),
                                    DiscountName = reader.IsDBNull(22) ? null : reader.GetString(22),
                                    DueDate = reader.GetDateTime(23),
                                    AmountPaid = reader.IsDBNull(24) ? (decimal?)null : reader.GetDecimal(24),
                                    Paid = reader.GetBoolean(25),
                                    PaymentDate = reader.IsDBNull(26) ? (DateTime?)null : reader.GetDateTime(26),
                                    Canceled = reader.GetBoolean(27),
                                    Inactive = reader.GetBoolean(28),
                                    Invoiced = reader.GetBoolean(29),
                                    StudentInvoiceLineId = reader.IsDBNull(30) ? (int?)null : reader.GetInt32(30),
                                    InvoiceLineId = reader.IsDBNull(31) ? (int?)null : reader.GetInt32(31),
                                    InvoiceNumber = reader.IsDBNull(32) ? null : reader.GetString(32),
                                    AppliedPenalty = reader.GetBoolean(33),
                                    PenaltyConditionId = reader.IsDBNull(34) ? (int?)null : reader.GetInt32(34),
                                    PenaltyAmount = reader.IsDBNull(35) ? (decimal?)null : reader.GetDecimal(35),
                                    RefCTIdIfPenalty = reader.GetInt32(36),
                                    StudentId = reader.GetInt32(37),
                                    OriginDocCCId = reader.GetInt32(38),
                                    CreatedAt = reader.IsDBNull(39) ? (DateTime?)null : reader.GetDateTime(39),
                                    UpdatedAt = reader.IsDBNull(40) ? (DateTime?)null : reader.GetDateTime(40),
                                    TimeCreated = reader.IsDBNull(41) ? null : reader.GetString(41),
                                    TimeUpdated = reader.IsDBNull(42) ? null : reader.GetString(42),
                                    CreatedByUserId = reader.IsDBNull(43) ? (int?)null : reader.GetInt32(43),
                                    UpdatedByUserId = reader.IsDBNull(44) ? (int?)null : reader.GetInt32(44),
                                    DeletedAt = reader.IsDBNull(45) ? (DateTime?)null : reader.GetDateTime(45),
                                    DeletedByUserId = reader.IsDBNull(46) ? (int?)null : reader.GetInt32(46),
                                    IsDeleted = reader.IsDBNull(47) ? (bool?)null : reader.GetBoolean(47),
                                    TimeDeleted = reader.IsDBNull(48) ? null : reader.GetString(48),
                                    IsFromReceipt = reader.GetBoolean(49),
                                    ReceiptId = reader.IsDBNull(50) ? (int?)null : reader.GetInt32(50)
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
                                StudentDossierId = reader.GetInt32(0),
                                StudentId = reader.GetInt32(1),
                                StudentNumber = reader.GetInt32(2),
                                Name = reader.GetString(3),
                                CustomerId = reader.GetInt32(4),
                                StudentStatusId = reader.GetInt32(5),
                                YearId = reader.GetInt32(6),
                                Year = reader.GetInt32(7),
                                ScholarshipId = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                                EnrollmentId = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                                CycleId = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                                CycleName = reader.IsDBNull(11) ? null : reader.GetString(11),
                                ClassId = reader.IsDBNull(12) ? null : reader.GetInt32(12),
                                ClassName = reader.IsDBNull(13) ? null : reader.GetString(13),
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
