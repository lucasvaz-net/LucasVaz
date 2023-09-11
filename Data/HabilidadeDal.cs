using LucasVaz.Models;
using System.Collections.Generic;
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
            return GetHabilidadesByCriteria("SELECT * FROM VWHABILIDADE", null);
        }

        public Tecnologia GetHabilidadeById(int idTecnologia)
        {
            var habilidades = GetHabilidadesByCriteria("SELECT * FROM VWHABILIDADE WHERE IDTECNOLOGIA = @IdTecnologia", new[] { new SqlParameter("@IdTecnologia", idTecnologia) });
            return habilidades.FirstOrDefault();
        }

        private List<Tecnologia> GetHabilidadesByCriteria(string query, SqlParameter[] parameters)
        {
            var habilidades = new List<Tecnologia>();

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
                            var habilidade = ParseHabilidadeFromReader(reader);
                            habilidades.Add(habilidade);
                        }
                    }
                }
            }

            return habilidades;
        }

        private Tecnologia ParseHabilidadeFromReader(SqlDataReader reader)
        {
            return new Tecnologia
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
        }
    }
}
