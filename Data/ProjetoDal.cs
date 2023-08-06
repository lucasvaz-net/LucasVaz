using LucasVaz.Models;
using System.Net.NetworkInformation;
using X.PagedList;
using X.PagedList.Mvc.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;


namespace LucasVaz.Data
{
    public class ProjetoDal
    {
        private readonly DataConnection _dataConnection;

        public ProjetoDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }


        public IPagedList<Projeto> GetProjetos(int pageNumber, int pageSize)
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

                                if (!reader.IsDBNull(reader.GetOrdinal("LKWEB")))
                                {
                                    projeto.LkWeb = reader.GetString(reader.GetOrdinal("LKWEB"));
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("DSLOGINTESTE")))
                                {
                                    projeto.DsLoginTeste = reader.GetString(reader.GetOrdinal("DSLOGINTESTE"));
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("DSSENHATESTE")))
                                {
                                    projeto.DsSenhaTeste = reader.GetString(reader.GetOrdinal("DSSENHATESTE"));
                                }

                                projetos[idProjeto] = projeto;
                            }

                            var tecnologiaProjeto = new TecnologiaProjeto
                            {
                                IdTecnologiasProjetos = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIASPROJETOS")),
                                Tecnologia = new Tecnologia
                                {
                                    IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                                    DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA")),
                                    QtHabilidade = reader.GetInt32(reader.GetOrdinal("QTHABILIDADE"))
                                },
                                Projeto = projeto
                            };

                            projeto.TecnologiasProjetos.Add(tecnologiaProjeto);
                        }
                    }
                }
            }

            return projetos.Values.ToList().ToPagedList(pageNumber, pageSize);
        }


        public IPagedList<Projeto> GetProjetosPorTecnologia(List<int> idsTecnologia, int pageNumber, int pageSize)
        {
            var projetos = new Dictionary<int, Projeto>();

            var idsTecnologiaString = string.Join(',', idsTecnologia);

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand($"SELECT * FROM VWPROJETOS WHERE IDPROJETO IN (SELECT IDPROJETO FROM VWPROJETOS WHERE IDTECNOLOGIA IN ({idsTecnologiaString}))", connection))
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

                                if (!reader.IsDBNull(reader.GetOrdinal("LKWEB")))
                                {
                                    projeto.LkWeb = reader.GetString(reader.GetOrdinal("LKWEB"));
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("DSLOGINTESTE")))
                                {
                                    projeto.DsLoginTeste = reader.GetString(reader.GetOrdinal("DSLOGINTESTE"));
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("DSSENHATESTE")))
                                {
                                    projeto.DsSenhaTeste = reader.GetString(reader.GetOrdinal("DSSENHATESTE"));
                                }

                                projetos[idProjeto] = projeto;
                            }

                            var tecnologiaProjeto = new TecnologiaProjeto
                            {
                                IdTecnologiasProjetos = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIASPROJETOS")),
                                Tecnologia = new Tecnologia
                                {
                                    IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                                    DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA")),
                                    QtHabilidade = reader.GetInt32(reader.GetOrdinal("QTHABILIDADE"))
                                },
                                Projeto = projeto
                            };

                            projeto.TecnologiasProjetos.Add(tecnologiaProjeto);
                        }
                    }
                }
            }

            return projetos.Values.ToList().ToPagedList(pageNumber, pageSize);
        }

        public List<Tecnologia> ObterTodasAsTecnologias()
        {
            var tecnologias = new List<Tecnologia>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT DISTINCT IDTECNOLOGIA, DSTECNOLOGIA FROM VWPROJETOS", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tecnologia = new Tecnologia
                            {
                                IdTecnologia = reader.GetInt32(reader.GetOrdinal("IDTECNOLOGIA")),
                                DsTecnologia = reader.GetString(reader.GetOrdinal("DSTECNOLOGIA"))
                            };

                            tecnologias.Add(tecnologia);
                        }
                    }
                }
            }

            return tecnologias;
        }


    }
}
