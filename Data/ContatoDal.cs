using LucasVaz.Models;
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
            var contatos = new List<Contato>();

            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM VWCONTATOS", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contato = new Contato
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

                            contatos.Add(contato);
                        }
                    }
                }
            }

            return contatos;
        }
    }

}
