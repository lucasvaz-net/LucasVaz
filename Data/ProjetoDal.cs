using LucasVaz.Models;
using System.Net.NetworkInformation;
using X.PagedList;
using X.PagedList.Mvc.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

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
            return GetProjetosByCriteria($"SELECT * FROM VWPROJETOS order by idtecnologia", null, pageNumber, pageSize);
        }

        public IPagedList<Projeto> GetProjetosPorTecnologia(List<int> idsTecnologia, int pageNumber, int pageSize)
        {
            var query = $"SELECT * FROM VWPROJETOS WHERE IDTECNOLOGIA IN (@ids)";
            var parameters = new SqlParameter("@ids", string.Join(',', idsTecnologia));

            return GetProjetosByCriteria(query, new[] { parameters }, pageNumber, pageSize);
        }

        public Projeto GetProjetoById(int idProjeto)
        {
            var projetos = GetProjetosByCriteria($"SELECT * FROM VWPROJETOS WHERE IDPROJETO = @IdProjeto", new[] { new SqlParameter("@IdProjeto", idProjeto) }, 1, 1);

            return projetos.FirstOrDefault();
        }

        private IPagedList<Projeto> GetProjetosByCriteria(string query, SqlParameter[] parameters, int pageNumber, int pageSize)
        {
            var projetos = new Dictionary<int, Projeto>();

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
                            var projeto = ParseProjetoFromReader(reader);
                            if (!projetos.ContainsKey(projeto.IdProjeto))
                            {
                                projetos[projeto.IdProjeto] = projeto;
                            }

                            var tecnologiaProjeto = ParseTecnologiaProjetoFromReader(reader, projeto);
                            projetos[projeto.IdProjeto].TecnologiasProjetos.Add(tecnologiaProjeto);
                        }
                    }
                }
            }

            return projetos.Values.ToList().ToPagedList(pageNumber, pageSize);
        }

        private Projeto ParseProjetoFromReader(SqlDataReader reader)
        {
            var projeto = new Projeto
            {
                IdProjeto = reader.GetInt32(reader.GetOrdinal("IDPROJETO")),
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

            return projeto;
        }

        private TecnologiaProjeto ParseTecnologiaProjetoFromReader(SqlDataReader reader, Projeto projeto)
        {
            return new TecnologiaProjeto
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

        public List<Tecnologia> ObterTecnologiasPorProjetoId(int idProjeto)
        {
            var tecnologias = new List<Tecnologia>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

               
                using (var command = new SqlCommand("SELECT DISTINCT IDTECNOLOGIA, DSTECNOLOGIA FROM VWPROJETOS WHERE IDPROJETO = @IdProjeto", connection))
                {
                    
                    command.Parameters.AddWithValue("@IdProjeto", idProjeto);

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


        public void InserirProjeto(Projeto projeto)
        {
            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_InsertProjeto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DsProjeto", projeto.DsProjeto);
                    command.Parameters.AddWithValue("@CmProjeto", projeto.CmProjeto);
                    command.Parameters.AddWithValue("@LkGithub", (object)projeto.LkGithub ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LkWeb", (object)projeto.LkWeb ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DsLoginTeste", (object)projeto.DsLoginTeste ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DsSenhaTeste", (object)projeto.DsSenhaTeste ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdPessoa", 1);
                    command.Parameters.AddWithValue("@IdTipoProjeto", projeto.TipoProjeto.IdTipoProjeto);
                    command.Parameters.AddWithValue("@log_Usuario", 1);
                    command.Parameters.AddWithValue("@log_Origem", "EditarProjeto");

                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditarProjeto(Projeto projeto)
        {
            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_UpdateProjeto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdProjeto", projeto.IdProjeto);
                    command.Parameters.AddWithValue("@DsProjeto", projeto.DsProjeto);
                    command.Parameters.AddWithValue("@CmProjeto", projeto.CmProjeto);
                    command.Parameters.AddWithValue("@LkGithub", (object)projeto.LkGithub ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LkWeb", (object)projeto.LkWeb ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DsLoginTeste", (object)projeto.DsLoginTeste ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DsSenhaTeste", (object)projeto.DsSenhaTeste ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdPessoa", projeto.Pessoa.IdPessoa);
                    command.Parameters.AddWithValue("@IdTipoProjeto", projeto.TipoProjeto.IdTipoProjeto);
                    command.Parameters.AddWithValue("@log_Usuario", 1);
                    command.Parameters.AddWithValue("@log_Origem", "EditarProjeto");

                    command.ExecuteNonQuery();
                }
            }
        }

       
        public void GerenciarTecnologiaProjeto(int idProjeto, int idTecnologia, char operacao)
        {
            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_TecnologiasProjetos_Operacao", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdProjeto", idProjeto);
                    command.Parameters.AddWithValue("@IdTecnologia", idTecnologia);
                    command.Parameters.AddWithValue("@Operacao", operacao);
                    command.Parameters.AddWithValue("@log_Usuario", 1);
                    command.Parameters.AddWithValue("@log_Origem", "EditarProjeto");

                    command.ExecuteNonQuery();
                }
            }
        }



    }
}
