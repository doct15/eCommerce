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
    public class CategoriaDAL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public CategoriaDAL(string _conStr)
        {
            conStr = _conStr;
        }

        public List<Categoria> dbListarCategorias()
        {
            List<Categoria> listCategorias = new List<Categoria>();

            string queryString = "SELECT * from CATEGORIES";

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Categoria categoria = new Categoria();

                        categoria.ID = Convert.ToInt32(reader["CATEGORY_ID"]);
                        categoria.Nome = reader["CATEGORY_NAME"].ToString();
                        categoria.Descricao = reader["CATEGORY_DESCRIPTION"].ToString();


                        listCategorias.Add(categoria);
                    }

                    return listCategorias;
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void dbInserirCategoria(string nome, string descricao)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_CATEGORY_ADD", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@NOME", nome));
                    command.Parameters.Add(new SqlParameter("@DESCRICAO", descricao));

                    command.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void dbRemoverCategoria(int idCategoria)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_CATEGORY_DELETE", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@IDCATEGORIA", idCategoria));

                    command.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void dbEditarCategoria(int idCategoria, string categoriaNome, string categoriaDescricao)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_CATEGORY_UPDATE", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@CATEGORY_ID", idCategoria));
                    command.Parameters.Add(new SqlParameter("@CATEGORY_NAME", categoriaNome));
                    command.Parameters.Add(new SqlParameter("@CATEGORY_DESCRIPTION", categoriaDescricao));

                    command.ExecuteNonQuery();

                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
