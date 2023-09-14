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

        public void EditarPessoa(Pessoa pessoa)
        {
            using (var connection = _dataConnection.CreateConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("sp_AtualizarPessoa", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IDPESSOA", 1);
                    command.Parameters.AddWithValue("@DSPESSOA", pessoa.DsPessoa);
                    command.Parameters.AddWithValue("@CDCPFCNPJ", pessoa.CdCpfCnpj ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DTNASCIMENTO", pessoa.DtNascimento);
                    command.Parameters.AddWithValue("@TPPESOA", pessoa.TpPessoa ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DSLOGIN", pessoa.DsLogin ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DSSENHA", pessoa.DsSenha ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DSSOBREMIM", pessoa.DsSobreMim ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DSINICIO", pessoa.DsInicio ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@log_USUARIO", 1);
                    command.Parameters.AddWithValue("@log_ORIGEM", "EditarPessoa");

                    command.ExecuteNonQuery();
                }
            }
        }

    }

}
