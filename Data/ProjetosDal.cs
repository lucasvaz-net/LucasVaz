using LucasVaz.Models;
using System.Data.SqlClient;

namespace LucasVaz.Data
{
    public class ProjetoDal
    {
        private readonly DataConnection _dataConnection;

        public ProjetoDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public List<Projeto> GetProjetos()
        {
            var projetos = new Dictionary<int, Projeto>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM VWPROJETOS", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var idProjeto = reader.GetInt32(reader.GetOrdinal("IDPROJETO"));

                            if (!projetos.TryGetValue(idProjeto, out var projeto))
                            {
                                projeto = new Projeto
                                {
                                    IdProjeto = idProjeto,
                                    DsProjeto = reader.GetString(reader.GetOrdinal("DSPROJETO")),
                                    CmProjeto = reader.GetString(reader.GetOrdinal("CMPROJETO")),
                                    LkGithub = reader.GetString(reader.GetOrdinal("LKGITHUB")),
                                    LkWeb = reader.GetString(reader.GetOrdinal("LKWEB")),
                                    DsLoginTeste = reader.GetString(reader.GetOrdinal("DSLOGINTESTE")),
                                    DsSenhaTeste = reader.GetString(reader.GetOrdinal("DSSENHATESTE")),
                                    TipoProjeto = new TipoProjeto
                                    {
                                        IdTipoProjeto = reader.GetInt32(reader.GetOrdinal("IDTIPOPROJETO")),
                                        DsTipoProjeto = reader.GetString(reader.GetOrdinal("DSTIPOPROJETO"))
                                    },
                                    Pessoa = new Pessoa
                                    {
                                        IdPessoa = reader.GetInt32(reader.GetOrdinal("IDPESSOA")),
                                        DsPessoa = reader.GetString(reader.GetOrdinal("DSPESSOA")),
                                        CdCpfCnpj = reader.GetString(reader.GetOrdinal("CDCPFCNPJ")),
                                        DtNascimento = reader.GetDateTime(reader.GetOrdinal("DTNASCIMENTO")),
                                    },
                                    TecnologiasProjetos = new List<TecnologiaProjeto>()
                                };

                                projetos[idProjeto] = projeto;
                            }

                            var tecnologia = new Tecnologia
                            {
                                IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                                DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA")),
                                QtHabilidade = reader.GetInt32(reader.GetOrdinal("QTHABILIDADE"))
                            };

                            var TecnologiaProjeto = new TecnologiaProjeto
                            {
                                IdTecnologiasProjetos = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIASPROJETOS")),
                                Projeto = projeto,
                                Tecnologia = tecnologia
                            };

                            projeto.TecnologiasProjetos.Add(TecnologiaProjeto);
                        }
                    }
                }
            }

            return projetos.Values.ToList();
        }
    }

}
