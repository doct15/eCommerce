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
    public class UsuarioSistemaDAL
    {

        //Método construtor que recebe a string de conexão
        string conStr;
        public UsuarioSistemaDAL(string _conStr)
        {
            conStr = _conStr;
        }

        /*VALIDA O USUARIO, RETORNANDO O ID CASO POSITIVO, OU -1 CASO NAO SEJA AUTENTICADO*/
        public int dbValidarUsuarioSistema(string nome, string senha)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_SYS_USER_VALIDATE", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@USRNAME", nome));
                    command.Parameters.Add(new SqlParameter("@PASSWORD", senha));

                    return Convert.ToInt32(command.ExecuteScalar());

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*RETORNA UM OBJETO USUARIO A PARTIR DO ID*/
        public UsuarioSistema dbObterUsuarioSistema(int id)
        {
            UsuarioSistema usuarioSistema = new UsuarioSistema();

            try
            {
                string queryString = "SELECT * FROM USERS_SYS WHERE USERSYS_ID = @ID";

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@ID", id));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        usuarioSistema.ID = Convert.ToInt32(reader["USERSYS_ID"]);
                        usuarioSistema.Username = reader["USERSYS_NAME"].ToString();
                        usuarioSistema.Senha = reader["USERSYS_PASSWORD"].ToString();
                        //TODO: ADICIONAR O RESTO DAS PROPRIEDADES
                    }

                    return usuarioSistema;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("UsuarioDAL - ObterUsuario: " + ex.Message);
            }
        }

        /*LISTA TODOS OS USUARIOS*/
        public List<UsuarioSistema> dbListarUsuariosSistema()
        {
            List<UsuarioSistema> listUsuarios = new List<UsuarioSistema>();

            string queryString = "SELECT * FROM USERS_SYS";

            try
            {

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UsuarioSistema usuario = new UsuarioSistema();

                        usuario.ID = Convert.ToInt32(reader["USERSYS_ID"]);
                        usuario.Username = reader["USERSYS_NAME"].ToString();
                        usuario.Senha = reader["USERSYS_PASSWORD"].ToString();
                        usuario.UltimoLogin = reader["USERSYS_LAST_LOGIN"] != DBNull.Value ? Convert.ToDateTime(reader["USERSYS_LAST_LOGIN"]) : (DateTime?)null;


                        listUsuarios.Add(usuario);
                    }

                    return listUsuarios;
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /*INSERE USUARIO NA BASE*/
        public void dbInserirUsuarioSistema(string username, string senha)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_SYS_USER_INSERT", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@USERNAME", username));
                    command.Parameters.Add(new SqlParameter("@PASSWORD", senha));

                    command.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /*REMOVE USUARIO DA BASE*/
        public void dbRemoverUsuarioSistema(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_SYS_USER_DELETE", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@USRID", userId));

                    command.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*VERIFICA SE O E-MAIL ENTRADO NÃO EXISTE NA TABELA DE USUARIOS*/
        public bool dbUsuarioDisponivel(string username)
        {
            try
            {
                string queryString = "SELECT COUNT(*) NUM FROM USERS_SYS WHERE USERSYS_NAME = @NOME";
                int retornoQuery = -1;

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@NOME", username));

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
                throw new Exception(ex.Message);
            }
        }
    }
}
