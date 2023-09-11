using LucasVaz.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LucasVaz.Data
{
    public class PrivacidadeDal
    {
        private readonly DataConnection _dataConnection;

        public PrivacidadeDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public List<Privacidade> GetPrivacidades()
        {
            return GetPrivacidadesByCriteria("SELECT * FROM VWPRIVACIDADE ORDER BY NRORDEMLISTAGEM", null);
        }

        public Privacidade GetPrivacidadeById(int idPrivacidade)
        {
            var privacidades = GetPrivacidadesByCriteria("SELECT * FROM VWPRIVACIDADE WHERE IDPRIVACIDADE = @IdPrivacidade", new[] { new SqlParameter("@IdPrivacidade", idPrivacidade) });
            return privacidades.FirstOrDefault();
        }

        private List<Privacidade> GetPrivacidadesByCriteria(string query, SqlParameter[] parameters)
        {
            var privacidades = new List<Privacidade>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var privacidade = ParsePrivacidadeFromReader(reader);
                            privacidades.Add(privacidade);
                        }
                    }
                }
            }

            return privacidades;
        }

        private Privacidade ParsePrivacidadeFromReader(SqlDataReader reader)
        {
            return new Privacidade
            {
                IdPrivacidade = reader.GetInt32(reader.GetOrdinal("IDPRIVACIDADE")),
                DtPrivacidade = reader.GetDateTime(reader.GetOrdinal("DTPRIVACIDADE")),
                DsTitulo = reader.GetString(reader.GetOrdinal("DSTITULO")),
                DsTexto = reader.GetString(reader.GetOrdinal("DSTEXTO")),
                NrOrdemListagem = reader.GetInt32(reader.GetOrdinal("NRORDEMLISTAGEM"))
            };
        }


        public bool UpsertPrivacidade(Privacidade privacidade, string operacao)
        {
            try
            {
                using (var connection = _dataConnection.CreateConnection())
                {
                    connection.Open();

                    using (var command = new SqlCommand("sp_UpsertPrivacidade", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                       
                        command.Parameters.AddWithValue("@IDPRIVACIDADE", privacidade.IdPrivacidade == 0 ? (object)DBNull.Value : privacidade.IdPrivacidade);
                        command.Parameters.AddWithValue("@DTPRIVACIDADE", DateTime.Now);
                        command.Parameters.AddWithValue("@DSTITULO", privacidade.DsTitulo);
                        command.Parameters.AddWithValue("@DSTEXTO", privacidade.DsTexto);
                        command.Parameters.AddWithValue("@NRORDEMLISTAGEM", privacidade.NrOrdemListagem);
                        command.Parameters.AddWithValue("@Operacao", operacao);  // 'I' para Insert, 'U' para Update

                        int result = command.ExecuteNonQuery();

                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }



    }
}
