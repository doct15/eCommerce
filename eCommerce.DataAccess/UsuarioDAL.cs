using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using eCommerce.Model;
using eCommerce.Model.Custom;

namespace eCommerce.DataAccess
{
    public class UsuarioDAL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public UsuarioDAL(string _conStr)
        {
            conStr = _conStr;
        }

        /*LISTA TODOS OS USUARIOS DA BASE*/
        public List<Usuario> dbListarUsuarios()
        {
            List<Usuario> listUsuarios = new List<Usuario>();

            string queryString = "SELECT * FROM USERS";

            try
            {

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario();

                        usuario.ID = Convert.ToInt32(reader["USER_ID"]);
                        usuario.Nome = reader["USER_NAME"].ToString();
                        usuario.Email = reader["USER_EMAIL"].ToString();
                        usuario.Senha = reader["USER_PASSWORD"].ToString();

                        //ENDERECO
                        usuario.Endereco = new Endereco();
                        usuario.Endereco.Rua = reader["USER_ADR_STREET"] != DBNull.Value ? reader["USER_ADR_STREET"].ToString() : "";
                        usuario.Endereco.Numero = reader["USER_ADR_NUMBER"] != DBNull.Value ? reader["USER_ADR_NUMBER"].ToString() : "";
                        usuario.Endereco.Complemento = reader["USER_ADR_COMPLEMENT"] != DBNull.Value ? reader["USER_ADR_COMPLEMENT"].ToString() : "";
                        usuario.Endereco.Bairro = reader["USER_ADR_DISTRICT"] != DBNull.Value ? reader["USER_ADR_DISTRICT"].ToString() : "";
                        usuario.Endereco.Cidade = reader["USER_ADR_CITY"] != DBNull.Value ? reader["USER_ADR_CITY"].ToString() : "";
                        usuario.Endereco.Estado = reader["USER_ADR_STATE"] != DBNull.Value ? reader["USER_ADR_STATE"].ToString() : "";
                        usuario.Endereco.Pais = reader["USER_ADR_COUNTRY"] != DBNull.Value ? reader["USER_ADR_COUNTRY"].ToString() : "";
                        usuario.Endereco.CEP = reader["USER_ADR_ZIPCODE"] != DBNull.Value ? reader["USER_ADR_ZIPCODE"].ToString() : "";

                        listUsuarios.Add(usuario);
                    }

                    return listUsuarios;
                }

            }

            catch (Exception ex)
            {
                throw new Exception("UsuarioDAL - dbListarUsuarios: " + ex.Message);
            }

        }

        /*INSERE USUARIO NA BASE*/
        public void dbInserirUsuario(string nome, string email, string senha)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_USER_INSERT", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@USER_NAME", nome));
                    command.Parameters.Add(new SqlParameter("@USER_EMAIL", email));
                    command.Parameters.Add(new SqlParameter("@USER_PASSWORD", senha));

                    command.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                throw new Exception("UsuarioDAL - dbInserirUsuario: " + ex.Message);
            }

        }

        /*VERIFICA SE O E-MAIL ENTRADO NÃO EXISTE NA TABELA DE USUARIOS*/
        public bool dbEmailDisponivel(string email)
        {
            try
            {
                string queryString = "SELECT COUNT(*) NUM FROM USERS WHERE USER_EMAIL = @EMAIL";
                int retornoQuery = -1;

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@EMAIL", email));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        retornoQuery = Convert.ToInt32(reader["NUM"]);
                    }

                }

                //Se tiver mais de um registro, retorna falso
                if (retornoQuery != 0)
                {
                    return false;
                }

                else
                {
                    return true;
                }

            }

            catch (Exception ex)
            {
                throw new Exception("UsuarioDAL - dbEmailJaExistente: " + ex.Message);
            }
        }

        /*VALIDA O USUARIO, RETORNANDO O ID CASO POSITIVO, OU -1 CASO NAO SEJA AUTENTICADO*/
        public int dbValidarUsuario(string email, string senha)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_USER_VALIDATE", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@EMAIL", email));
                    command.Parameters.Add(new SqlParameter("@PASSWORD", senha));

                    return Convert.ToInt32(command.ExecuteScalar());

                }
            }

            catch (Exception ex)
            {
                throw new Exception("UsuarioDAL - dbValidarUsuario: " + ex.Message);
            }
        }

        /*RETORNA UM OBJETO USUARIO A PARTIR DO ID*/
        public Usuario dbObterUsuario(int id)
        {
            Usuario usuario = new Usuario();

            try
            {
                string queryString = "SELECT * FROM USERS WHERE USER_ID = @ID";

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@ID", id));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        usuario.ID = Convert.ToInt32(reader["USER_ID"]);
                        usuario.Nome = reader["USER_NAME"].ToString();
                        usuario.Email = reader["USER_EMAIL"].ToString();
                        usuario.Senha = reader["USER_PASSWORD"].ToString();
                        //TODO: ADICIONAR O RESTO DAS PROPRIEDADES
                    }

                    return usuario;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("UsuarioDAL - ObterUsuario: " + ex.Message);
            }
        }
    }
}
