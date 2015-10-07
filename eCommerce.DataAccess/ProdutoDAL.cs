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
    public class ProdutoDAL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public ProdutoDAL(string _conStr)
        {
            conStr = _conStr;
        }

        public List<Produto> dbListarProdutos()
        {
            List<Produto> listProdutos = new List<Produto>();

            string queryString = "SELECT * FROM PRODUCTS PR";
            queryString += " INNER JOIN CATEGORIES CA";
            queryString += " ON PR.PRODUCT_CATEGORY_ID = CA.CATEGORY_ID";

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Produto produto = new Produto();

                        produto.ID = Convert.ToInt32(reader["PRODUCT_ID"]);
                        produto.Nome = reader["PRODUCT_NAME"].ToString();
                        produto.Descricao = reader["PRODUCT_DESCRIPTION"].ToString();
                        produto.CaminhoImagem = reader["PRODUCT_IMAGEPATH"].ToString();
                        produto.Preco = Convert.ToDecimal(reader["PRODUCT_PRICE"]);

                        //Cria a categoria
                        produto.Categoria = new Categoria();

                        produto.Categoria.ID = Convert.ToInt32(reader["CATEGORY_ID"]);
                        produto.Categoria.Nome = reader["CATEGORY_NAME"].ToString();
                        produto.Categoria.Descricao = reader["CATEGORY_DESCRIPTION"].ToString();

                        listProdutos.Add(produto);
                    }

                    return listProdutos;
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void dbInserirProduto(string nome, string descricao, string caminhoImagem, decimal preco, int idCategoria)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_PRODUCT_ADD", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@PRODUCT_NAME", nome));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_DESCRIPTION", descricao));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_IMAGEPATH", caminhoImagem));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_PRICE", preco));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_CATEGORY_ID", idCategoria));
                    
                    command.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void dbEditarProduto(int idProduto, string nome, string descricao, string caminhoImagem, decimal preco, int idCategoria)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_PRODUCT_UPDATE", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@PRODUCT_ID", idProduto));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_NAME", nome));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_DESCRIPTION", descricao));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_IMAGEPATH", caminhoImagem));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_PRICE", preco));
                    command.Parameters.Add(new SqlParameter("@PRODUCT_CATEGORY_ID", idCategoria));

                    command.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void dbRemoverProduto(int idProduto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_PRODUCT_DELETE", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@PRODUCT_ID", idProduto));

                    command.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
