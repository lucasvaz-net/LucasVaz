using LucasVaz.Models;
using System.Data.SqlClient;

namespace LucasVaz.Data
{
    public class HabilidadeDal
    {
        private readonly DataConnection _dataConnection;

        public HabilidadeDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public List<Tecnologia> GetHabilidades()
        {
            var habilidades = new List<Tecnologia>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM VWHABILIDADE", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var habilidade = new Tecnologia
                            {
                                IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                                DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA")),
                                QtHabilidade = reader.GetInt32(reader.GetOrdinal("QTHABILIDADE")),
                                TipoTecnologia = new TipoTecnologia
                                {
                                    IdTipoTecnologia = reader.GetInt32(reader.GetOrdinal("IDTIPOTECNOLOGIA")),
                                    DsTipoTecnologia = reader.GetString(reader.GetOrdinal("DSTIPOTECNOLOGIA"))
                                }
                            };

                            habilidades.Add(habilidade);
                        }
                    }
                }
            }

            return habilidades;
        }
    }
}
