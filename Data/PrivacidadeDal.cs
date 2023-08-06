using LucasVaz.Models;
using System;
using System.Collections.Generic;
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
            List<Privacidade> privacidades = new List<Privacidade>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM VWPRIVACIDADE ORDER BY NRORDEMLISTAGEM", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            privacidades.Add(new Privacidade
                            {
                                IdPrivacidade = reader.GetInt32(reader.GetOrdinal("IDPRIVACIDADE")),
                                DtPrivacidade = reader.GetDateTime(reader.GetOrdinal("DTPRIVACIDADE")),
                                DsTitulo = reader.GetString(reader.GetOrdinal("DSTITULO")),
                                DsTexto = reader.GetString(reader.GetOrdinal("DSTEXTO")),
                                NrOrdemListagem = reader.GetInt32(reader.GetOrdinal("NRORDEMLISTAGEM"))
                            });
                        }
                    }
                }
            }

            return privacidades;
        }
    }
}
