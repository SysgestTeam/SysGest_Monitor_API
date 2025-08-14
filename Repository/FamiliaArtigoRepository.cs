using SistemasdeTarefas.Interface;
using SistemasdeTarefas.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace SistemasdeTarefas.Repository
{
    public class FamiliaArtigoRepository : IFamiliaArtigoRepository
    {
        private readonly string _connectionString;

        public FamiliaArtigoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #region FAMILIA

        public async Task CreatFamily(FamiliaDTO familia)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO TABFAMILIAS (NOME, IsMonitor) VALUES (@nome, @isMonitor)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nome", familia.NOME);
                        cmd.Parameters.AddWithValue("@isMonitor", familia.IsMonitor);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro SQL ao criar família.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar família.", ex);
            }
        }

        public async Task<IEnumerable<Familia>> ListFamily()
        {
            var familias = new List<Familia>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT IDFAMILIA, NOME, IsMonitor FROM TABFAMILIAS WHERE IsMonitor = 1 ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            familias.Add(new Familia
                            {
                                IDFAMILIA = reader.GetInt32(0),
                                NOME = reader.GetString(1),
                                IsMonitor = reader.GetBoolean(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar famílias.", ex);
            }

            return familias;
        }

        public async Task<IEnumerable<Familia>> FindByIdAFamily(int idFamily)
        {
            var familias = new List<Familia>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT IDFAMILIA, NOME, IsMonitor FROM TABFAMILIAS WHERE IDFAMILIA = @id AND IsMonitor = 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", idFamily);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                familias.Add(new Familia
                                {
                                    IDFAMILIA = reader.GetInt32(0),
                                    NOME = reader.GetString(1),
                                    IsMonitor = reader.GetBoolean(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao buscar família por ID.", ex);
            }

            return familias;
        }

        public async Task UpdateFamily(Familia familia)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "UPDATE TABFAMILIAS SET NOME = @nome, IsMonitor = @isMonitor WHERE IDFAMILIA = @id AND IsMonitor = 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nome", familia.NOME);
                        cmd.Parameters.AddWithValue("@isMonitor", familia.IsMonitor);
                        cmd.Parameters.AddWithValue("@id", familia.IDFAMILIA);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar família.", ex);
            }
        }

        public async Task DeleteFamily(int idFamilia)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM TABFAMILIAS WHERE IDFAMILIA = @id AND IsMonitor = 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", idFamilia);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir família.", ex);
            }
        }

        #endregion

        #region ARTIGO

        public async Task CreatArtigo(ArtigoDTO artigo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        INSERT INTO TABARTIGOS
                        (CODIGO, NOME, IDFAMILIA, IDUNIDADE, PRCVENDA, IDTAXAIPC, STOCK, FOTO, IDUSR, IdTaxaImposto, ValorTaxa, IdArmazem, IsDepositoSaldo, IsMulta)
                        VALUES (@codigo, @nome, @idFamilia, @idUnidade, @prcVenda, @idTaxaIpc, @stock, @foto, @idUsr, @idTaxaImposto, @valorTaxa, @idArmazem, @isDepositoSaldo, @isMulta)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {


                        cmd.Parameters.AddWithValue("@codigo", artigo.CODIGO);
                        cmd.Parameters.AddWithValue("@nome", artigo.NOME);
                        cmd.Parameters.AddWithValue("@idFamilia", artigo.IDFAMILIA);
                        cmd.Parameters.AddWithValue("@idUnidade", artigo.IDUNIDADE);
                        cmd.Parameters.AddWithValue("@prcVenda", artigo.PRCVENDA);
                        cmd.Parameters.AddWithValue("@idTaxaIpc", artigo.IDTAXAIPC);
                        cmd.Parameters.AddWithValue("@stock", artigo.STOCK);
                        byte[] fotoBytes = null;

                        if (!string.IsNullOrEmpty(artigo.FOTO))
                        {
                            // Remove prefixo se existir
                            string base64 = artigo.FOTO.Contains(",")
                                ? artigo.FOTO.Split(',')[1]
                                : artigo.FOTO;

                            fotoBytes = Convert.FromBase64String(base64);
                        }

                        cmd.Parameters.Add("@foto", SqlDbType.Image).Value = (object)fotoBytes ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@idUsr", artigo.IDUSR);
                        cmd.Parameters.AddWithValue("@idTaxaImposto", artigo.IdTaxaImposto);
                        cmd.Parameters.AddWithValue("@valorTaxa", artigo.ValorTaxa);
                        cmd.Parameters.AddWithValue("@idArmazem", artigo.IdArmazem);
                        cmd.Parameters.AddWithValue("@isDepositoSaldo", artigo.IsDepositoSaldo);
                        cmd.Parameters.AddWithValue("@isMulta", artigo.IsMulta);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar artigo.", ex);
            }
        }

        public async Task<IEnumerable<Artigo>> ListArtigo()
        {
            var artigos = new List<Artigo>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                            SELECT 
                                a.IDARTIGO, 
                                a.CODIGO, 
                                a.NOME, 
                                a.IDFAMILIA, 
                                a.IDUNIDADE, 
                                a.PRCVENDA, 
                                a.IDTAXAIPC, 
                                a.STOCK, 
                                a.FOTO, 
                                a.IDUSR, 
                                a.IdTaxaImposto, 
                                a.ValorTaxa, 
                                a.IdArmazem, 
                                a.IsDepositoSaldo, 
                                a.IsMulta
                            FROM TABARTIGOS a
                            INNER JOIN TABFAMILIAS f ON a.IDFAMILIA = f.IDFAMILIA
                            WHERE f.IsMonitor = 1";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                            artigos.Add(new Artigo
                            {
                                IDARTIGO = reader.GetInt32(0),
                                CODIGO = reader.GetInt32(1),
                                NOME = reader.GetString(2),
                                IDFAMILIA = reader.GetInt32(3),
                                IDUNIDADE = reader.GetInt32(4),
                                PRCVENDA = reader.GetDecimal(5),
                                IDTAXAIPC = reader.GetInt32(6),
                                STOCK = reader.GetInt32(7),
                                FOTO = reader.IsDBNull(8) ? null : (byte[])reader[8],
                                IDUSR = reader.GetInt32(9),
                                IdTaxaImposto = reader.GetInt32(10),
                                ValorTaxa = reader.GetDecimal(11),
                                IdArmazem = reader.GetInt32(12),
                                IsDepositoSaldo = reader.GetBoolean(13),
                                IsMulta = reader.GetBoolean(14)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar artigos.", ex);
            }

            return artigos;
        }

        public async Task<IEnumerable<Artigo>> FindByIdArtigo(int idArtigo)
        {
            var artigos = new List<Artigo>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                        SELECT a.IDARTIGO, a.CODIGO, a.NOME, a.IDFAMILIA, a.IDUNIDADE, a.PRCVENDA, 
                               a.IDTAXAIPC, a.STOCK, a.FOTO, a.IDUSR, a.IdTaxaImposto, a.ValorTaxa, 
                               a.IdArmazem, a.IsDepositoSaldo, a.IsMulta
                        FROM TABARTIGOS a
                        INNER JOIN TABFAMILIAS f ON a.IDFAMILIA = f.IDFAMILIA
                        WHERE a.IDARTIGO = @id
                          AND f.IsMonitor = 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", idArtigo);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                artigos.Add(new Artigo
                                {
                                    IDARTIGO = reader.GetInt32(0),
                                    CODIGO = reader.GetInt32(1),
                                    NOME = reader.GetString(2),
                                    IDFAMILIA = reader.GetInt32(3),
                                    IDUNIDADE = reader.GetInt32(4),
                                    PRCVENDA = reader.GetDecimal(5),
                                    IDTAXAIPC = reader.GetInt32(6),
                                    STOCK = reader.GetInt32(7),
                                    FOTO = reader.IsDBNull(8) ? null : (byte[])reader[8],
                                    IDUSR = reader.GetInt32(9),
                                    IdTaxaImposto = reader.GetInt32(10),
                                    ValorTaxa = reader.GetDecimal(11),
                                    IdArmazem = reader.GetInt32(12),
                                    IsDepositoSaldo = reader.GetBoolean(13),
                                    IsMulta = reader.GetBoolean(14)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao buscar artigo por ID.", ex);
            }

            return artigos;
        }

        public async Task UpdateArtigo(Artigo artigo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                        UPDATE TABARTIGOS
                        SET CODIGO = @codigo,
                            NOME = @nome,
                            IDFAMILIA = @idFamilia,
                            IDUNIDADE = @idUnidade,
                            PRCVENDA = @prcVenda,
                            IDTAXAIPC = @idTaxaIpc,
                            STOCK = @stock,
                            FOTO = @foto,
                            IDUSR = @idUsr,
                            IdTaxaImposto = @idTaxaImposto,
                            ValorTaxa = @valorTaxa,
                            IdArmazem = @idArmazem,
                            IsDepositoSaldo = @isDepositoSaldo,
                            IsMulta = @isMulta
                        WHERE IDARTIGO = @idArtigo";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@codigo", artigo.CODIGO);
                        cmd.Parameters.AddWithValue("@nome", artigo.NOME);
                        cmd.Parameters.AddWithValue("@idFamilia", artigo.IDFAMILIA);
                        cmd.Parameters.AddWithValue("@idUnidade", artigo.IDUNIDADE);
                        cmd.Parameters.AddWithValue("@prcVenda", artigo.PRCVENDA);
                        cmd.Parameters.AddWithValue("@idTaxaIpc", artigo.IDTAXAIPC);
                        cmd.Parameters.AddWithValue("@stock", artigo.STOCK);
                        cmd.Parameters.AddWithValue("@idUsr", artigo.IDUSR);
                        // Foto - definir explicitamente como tipo image
                        if (artigo.FOTO != null && artigo.FOTO.Length > 0)
                            cmd.Parameters.Add("@foto", SqlDbType.Image).Value = artigo.FOTO;
                        else
                            cmd.Parameters.Add("@foto", SqlDbType.Image).Value = DBNull.Value;
                        cmd.Parameters.AddWithValue("@idTaxaImposto", artigo.IdTaxaImposto);
                        cmd.Parameters.AddWithValue("@valorTaxa", artigo.ValorTaxa);
                        cmd.Parameters.AddWithValue("@idArmazem", artigo.IdArmazem);
                        cmd.Parameters.AddWithValue("@isDepositoSaldo", artigo.IsDepositoSaldo);
                        cmd.Parameters.AddWithValue("@isMulta", artigo.IsMulta);
                        cmd.Parameters.AddWithValue("@idArtigo", artigo.IDARTIGO);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar artigo.", ex);
            }
        }

        public async Task DeleteArtigo(int idArtigo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM TABARTIGOS WHERE IDARTIGO = @id";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", idArtigo);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir artigo.", ex);
            }
        }

        #endregion
    }
}
