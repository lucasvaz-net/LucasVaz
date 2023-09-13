using LucasVaz.Models;
using System.Data.SqlClient;
using X.PagedList;

namespace LucasVaz.Data
{
    public class ExperienciaDal
    {
        private readonly DataConnection _dataConnection;

        public ExperienciaDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public IPagedList<Experiencia> GetExperiencias(int pageNumber, int pageSize)
        {
            return GetExperienciasByCriteria($"SELECT * FROM VWEXPERIENCIAS ORDER BY idtipoexperiencia", null, pageNumber, pageSize);
        }

        public Experiencia GetExperienciaById(int idExperiencia)
        {
            var experiencias = GetExperienciasByCriteria($"SELECT * FROM VWEXPERIENCIAS WHERE IDEXPERIENCIA = @IdExperiencia", new[] { new SqlParameter("@IdExperiencia", idExperiencia) }, 1, 1);
            return experiencias.FirstOrDefault();
        }

        private IPagedList<Experiencia> GetExperienciasByCriteria(string query, SqlParameter[] parameters, int pageNumber, int pageSize)
        {
            var experiencias = new Dictionary<int, Experiencia>();

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
                            var experiencia = ParseExperienciaFromReader(reader);
                            if (!experiencias.ContainsKey(experiencia.IdExperiencia))
                            {
                                experiencias[experiencia.IdExperiencia] = experiencia;
                            }

                            var tecnologiaExperiencia = ParseTecnologiaExperienciaFromReader(reader, experiencia);
                            experiencias[experiencia.IdExperiencia].ExperienciasTecnologias.Add(tecnologiaExperiencia);
                        }
                    }
                }
            }
            return experiencias.Values.ToList().ToPagedList(pageNumber, pageSize);
        }

        private Experiencia ParseExperienciaFromReader(SqlDataReader reader)
        {
            var experiencia = new Experiencia
            {
                IdExperiencia = reader.GetInt32(reader.GetOrdinal("IDEXPERIENCIA")),
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

            return experiencia;
        }

        private ExperienciaTecnologia ParseTecnologiaExperienciaFromReader(SqlDataReader reader, Experiencia experiencia)
        {
            var tecnologia = new Tecnologia
            {
                IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA")),
                QtHabilidade = reader.GetInt32(reader.GetOrdinal("QTHABILIDADE"))
            };

            return new ExperienciaTecnologia
            {
                IdExperienciaTecnologia = reader.GetInt32(reader.GetOrdinal("IDEXPERIENCIATECNOLOGIA")),
                Experiencia = experiencia,
                Tecnologia = tecnologia
            };
        }

        public void UpsertExperiencia(Experiencia experiencia, string operacao)
        {
            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("sp_UpsertExperiencia", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IDEXPERIENCIA", experiencia.IdExperiencia);
                    command.Parameters.AddWithValue("@DSEXPERIENCIA", experiencia.DsExperiencia);
                    command.Parameters.AddWithValue("@DSFUNCAO", experiencia.DsFuncao);
                    command.Parameters.AddWithValue("@DSLOCAL", experiencia.DsLocal);
                    command.Parameters.AddWithValue("@DTINI", experiencia.DtIni);
                    command.Parameters.AddWithValue("@DTFIM", experiencia.DtFim);
                    command.Parameters.AddWithValue("@IDTIPOEXPERIENCIA", experiencia.TipoExperiencia.IdTipoExperiencia);
                    command.Parameters.AddWithValue("@OPERACAO", operacao);
                    command.Parameters.AddWithValue("@log_ORIGEM", "NomeDaOrigem");  // Substitua pelo nome correto da origem

                    command.ExecuteNonQuery();
                }
            }
        }



    }
}
