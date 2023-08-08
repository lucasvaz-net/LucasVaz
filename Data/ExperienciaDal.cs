using LucasVaz.Models;
using System.Data.SqlClient;

namespace LucasVaz.Data
{
    public class ExperienciaDal
    {
        private readonly DataConnection _dataConnection;

        public ExperienciaDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public List<Experiencia> GetExperiencias()
        {
            var experiencias = new Dictionary<int, Experiencia>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM VWEXPERIENCIAS", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var idExperiencia = reader.GetInt32(reader.GetOrdinal("IDEXPERIENCIA"));

                            if (!experiencias.TryGetValue(idExperiencia, out var experiencia))
                            {
                                experiencia = new Experiencia
                                {
                                    IdExperiencia = idExperiencia,
                                    DsExperiencia = reader.GetString(reader.GetOrdinal("DSEXPERIENCIA")),
                                    DsFuncao = reader.GetString(reader.GetOrdinal("DSFUNCAO")),
                                    DsLocal = reader.GetString(reader.GetOrdinal("DSLOCAL")),
                                    DtIni = reader.GetDateTime(reader.GetOrdinal("DTINI")),
                                    Pessoa = new Pessoa
                                    {
                                        IdPessoa = reader.GetInt32(reader.GetOrdinal("IDPESSOA")),
                                        DsPessoa = reader.GetString(reader.GetOrdinal("DSPESSOA")),
                                        CdCpfCnpj = reader.GetString(reader.GetOrdinal("CDCPFCNPJ")),
                                        DtNascimento = reader.GetDateTime(reader.GetOrdinal("DTNASCIMENTO")),
                                    },
                                    TipoExperiencia = new TipoExperiencia
                                    {
                                        IdTipoExperiencia = reader.GetInt32(reader.GetOrdinal("IDTIPOEXPERIENCIA")),
                                        DsTipoExperiencia = reader.GetString(reader.GetOrdinal("DSTIPOEXPERIENCIA"))
                                    },
                                    ExperienciasTecnologias = new List<ExperienciaTecnologia>()
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("DTFIM")))
                                {
                                    experiencia.DtFim = reader.GetDateTime(reader.GetOrdinal("DTFIM"));
                                }

                                experiencias[idExperiencia] = experiencia;
                            }

                            var tecnologia = new Tecnologia
                            {
                                IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                                DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA")),
                                QtHabilidade = reader.GetInt32(reader.GetOrdinal("QTHABILIDADE"))
                            };

                            var experienciaTecnologia = new ExperienciaTecnologia
                            {
                                IdExperienciaTecnologia = reader.GetInt32(reader.GetOrdinal("IDEXPERIENCIATECNOLOGIA")),
                                Experiencia = experiencia,
                                Tecnologia = tecnologia
                            };

                            experiencia.ExperienciasTecnologias.Add(experienciaTecnologia);
                        }
                    }
                }
            }

            return experiencias.Values.ToList();
        }


    }
}
