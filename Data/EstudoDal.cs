using LucasVaz.Models;
using System.Data.SqlClient;

namespace LucasVaz.Data
{
    public class EstudoDal
    {
        private readonly DataConnection _dataConnection;

        public EstudoDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public List<Estudo> GetEstudos()
        {
            var estudos = new Dictionary<int, Estudo>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM VW_ESTUDOS", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var idEstudo = reader.GetInt32(reader.GetOrdinal("IDESTUDO"));

                            if (!estudos.TryGetValue(idEstudo, out var estudo))
                            {
                                estudo = new Estudo
                                {
                                    IdEstudo = idEstudo,
                                    DsEstudo = reader.GetString(reader.GetOrdinal("DSESTUDO")),
                                    DsLocal = reader.GetString(reader.GetOrdinal("DSLOCAL")),
                                    DtEstudo = reader.GetDateTime(reader.GetOrdinal("DTESTUDO")).ToString(),
                                    TipoEstudo = new TipoEstudo
                                    {
                                        IdTipoEstudo = reader.GetInt32(reader.GetOrdinal("IDTIPOESTUDO")),
                                        DsTipoEstudo = reader.GetString(reader.GetOrdinal("DSTIPOESTUDO"))
                                    },
                                    Pessoa = new Pessoa
                                    {
                                        IdPessoa = reader.GetInt32(reader.GetOrdinal("IDPESSOA")),
                                        DsPessoa = reader.GetString(reader.GetOrdinal("DSPESSOA")),
                                        CdCpfCnpj = reader.GetString(reader.GetOrdinal("CDCPFCNPJ")),
                                        DtNascimento = reader.GetDateTime(reader.GetOrdinal("DTNASCIMENTO")),
                                    },
                                    EstudosTecnologias = new List<EstudoTecnologia>()
                                };

                                estudos[idEstudo] = estudo;
                            }

                            var tecnologia = new Tecnologia
                            {
                                IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                                DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA")),
                                QtHabilidade = reader.GetInt32(reader.GetOrdinal("QTHABILIDADE"))
                            };

                            var estudoTecnologia = new EstudoTecnologia
                            {
                                IdEstudoTecnologia = reader.GetInt32(reader.GetOrdinal("IDESTUDOTECNOLOGIA")),
                                Estudo = estudo,
                                Tecnologia = tecnologia
                            };

                            estudo.EstudosTecnologias.Add(estudoTecnologia);
                        }
                    }
                }
            }

            return estudos.Values.ToList();
        }
    }

}
