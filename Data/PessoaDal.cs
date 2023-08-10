using LucasVaz.Models;
using System.Data;
using System.Data.SqlClient;

namespace LucasVaz.Data
{
    public class PessoaDal
    {
        private readonly DataConnection _dataConnection;

        public PessoaDal(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public Pessoa GetPessoa(int id)
        {
            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM VWPESSOA WHERE IDPESSOA = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Pessoa
                            {
                                IdPessoa = reader.GetInt32(reader.GetOrdinal("IDPESSOA")),
                                DsPessoa = reader.GetString(reader.GetOrdinal("DSPESSOA")),
                                CdCpfCnpj = reader.GetString(reader.GetOrdinal("CDCPFCNPJ")),
                                DtNascimento = reader.GetDateTime(reader.GetOrdinal("DTNASCIMENTO")),
                                TpPessoa = reader.GetString(reader.GetOrdinal("TPPESOA")),
                                DsLogin = reader.GetString(reader.GetOrdinal("DSLOGIN")),
                                DsSenha = reader.GetString(reader.GetOrdinal("DSSENHA")),
                                DsSobreMim = reader.GetString(reader.GetOrdinal("DSSOBREMIM")),
                                DsInicio = reader.GetString(reader.GetOrdinal("DSINICIO"))
                            };
                        }
                        else
                        {
                            throw new Exception("Pessoa não encontrada");
                        }
                    }
                }
            }
        }

        public int? GetPessoaLogin(string email, string password)
        {
            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                string query = "SELECT IDPESSOA FROM VWPESSOA WHERE dslogin = @Email AND DsSenha = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }

                    return null;
                }
            }
        }


    }

}
