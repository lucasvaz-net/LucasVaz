using LucasVaz.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LucasVaz.Data
{
    public class ContatoDal
    {
        private readonly DataConnection _dataConnection;

        public ContatoDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public List<Contato> GetContatos()
        {
            return GetContatosByCriteria("SELECT * FROM VWCONTATOS", null);
        }

        public Contato GetContatoById(int idContato)
        {
            var contatos = GetContatosByCriteria("SELECT * FROM VWCONTATOS WHERE IDCONTATO = @IdContato", new[] { new SqlParameter("@IdContato", idContato) });
            return contatos.FirstOrDefault();
        }

        private List<Contato> GetContatosByCriteria(string query, SqlParameter[] parameters)
        {
            var contatos = new List<Contato>();

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
                            var contato = ParseContatoFromReader(reader);
                            contatos.Add(contato);
                        }
                    }
                }
            }

            return contatos;
        }

        private Contato ParseContatoFromReader(SqlDataReader reader)
        {
            return new Contato
            {
                IdContato = reader.GetInt32(reader.GetOrdinal("IDCONTATO")),
                DsContato = reader.GetString(reader.GetOrdinal("DSCONTATO")),
                TipoContato = new TipoContato
                {
                    IdTipoContato = reader.GetInt32(reader.GetOrdinal("IDTIPOCONTATO")),
                    DsTipoContato = reader.GetString(reader.GetOrdinal("DSTIPOCONTATO"))
                },
                Pessoa = new Pessoa
                {
                    IdPessoa = reader.GetInt32(reader.GetOrdinal("IDPESSOA")),
                    DsPessoa = reader.GetString(reader.GetOrdinal("DSPESSOA")),
                    CdCpfCnpj = reader.GetString(reader.GetOrdinal("CDCPFCNPJ")),
                    DtNascimento = reader.GetDateTime(reader.GetOrdinal("DTNASCIMENTO")),
                }
            };
        }
    }
}
