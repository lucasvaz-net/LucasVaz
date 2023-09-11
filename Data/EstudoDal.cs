using LucasVaz.Models;
using System.Data.SqlClient;
using X.PagedList;

namespace LucasVaz.Data
{
    public class EstudoDal
    {
        private readonly DataConnection _dataConnection;

        public EstudoDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public IPagedList<Estudo> GetEstudos(int pageNumber, int pageSize)
        {
            return GetEstudosByCriteria($"SELECT * FROM VW_ESTUDOS", null, pageNumber, pageSize);
        }

        private IPagedList<Estudo> GetEstudosByCriteria(string query, SqlParameter[] parameters, int pageNumber, int pageSize)
        {
            var estudos = new Dictionary<int, Estudo>();

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
                            var estudo = ParseEstudoFromReader(reader);
                            if (!estudos.ContainsKey(estudo.IdEstudo))
                            {
                                estudos[estudo.IdEstudo] = estudo;
                            }

                            var tecnologiaEstudo = ParseTecnologiaEstudoFromReader(reader, estudo);
                            estudos[estudo.IdEstudo].EstudosTecnologias.Add(tecnologiaEstudo);
                        }
                    }
                }
            }
            return estudos.Values.ToList().ToPagedList(pageNumber, pageSize);
        }

        private Estudo ParseEstudoFromReader(SqlDataReader reader)
        {
            var estudo = new Estudo
            {
                IdEstudo = reader.GetInt32(reader.GetOrdinal("IDESTUDO")),
                DsEstudo = reader.GetString(reader.GetOrdinal("DSESTUDO")),
                DsLocal = reader.GetString(reader.GetOrdinal("DSLOCAL")),
                DtEstudo = reader.GetDateTime(reader.GetOrdinal("DTESTUDO")).ToString(),
                Pessoa = new Pessoa
                {
                    IdPessoa = reader.GetInt32(reader.GetOrdinal("IDPESSOA")),
                    DsPessoa = reader.GetString(reader.GetOrdinal("DSPESSOA")),
                    CdCpfCnpj = reader.GetString(reader.GetOrdinal("CDCPFCNPJ")),
                    DtNascimento = reader.GetDateTime(reader.GetOrdinal("DTNASCIMENTO")),
                },
                TipoEstudo = new TipoEstudo
                {
                    IdTipoEstudo = reader.GetInt32(reader.GetOrdinal("IDTIPOESTUDO")),
                    DsTipoEstudo = reader.GetString(reader.GetOrdinal("DSTIPOESTUDO"))
                },
                EstudosTecnologias = new List<EstudoTecnologia>()
            };

            return estudo;
        }

        private EstudoTecnologia ParseTecnologiaEstudoFromReader(SqlDataReader reader, Estudo estudo)
        {
            var tecnologia = new Tecnologia
            {
                IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA")),
                QtHabilidade = reader.GetInt32(reader.GetOrdinal("QTHABILIDADE"))
            };

            return new EstudoTecnologia
            {
                IdEstudoTecnologia = reader.GetInt32(reader.GetOrdinal("IDESTUDOTECNOLOGIA")),
                Estudo = estudo,
                Tecnologia = tecnologia
            };
        }
    }
}
