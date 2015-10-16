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
    public class EstoqueDAL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public EstoqueDAL(string _conStr)
        {
            conStr = _conStr;
        }

        public List<ItemEstoque> dbListarItemsEstoque()
        {
            List<ItemEstoque> estoque = new List<ItemEstoque>();

            string queryString = "SELECT * from STOCK ST";
            queryString += " INNER JOIN PRODUCTS PR";
            queryString += " ON ST.STOCK_ITEM_PRODUCT_ID = PR.PRODUCT_ID";
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
                        ItemEstoque item = new ItemEstoque();

                        item.ID = Convert.ToInt32(reader["STOCK_ITEM_ID"]);

                        //PRODUTO
                        item.Produto = new Produto();
                        item.Produto.ID = Convert.ToInt32(reader["STOCK_ITEM_PRODUCT_ID"]);
                        item.Produto.Nome = reader["PRODUCT_NAME"].ToString();
                        item.Produto.Descricao = reader["PRODUCT_DESCRIPTION"].ToString();
                        item.Produto.CaminhoImagem = reader["PRODUCT_IMAGEPATH"].ToString();
                        item.Produto.Preco = Convert.ToDecimal(reader["PRODUCT_PRICE"]);

                        //PRODUTO_CATEGORIA
                        item.Produto.Categoria = new Categoria();
                        item.Produto.Categoria.ID = Convert.ToInt32(reader["PRODUCT_CATEGORY_ID"]);
                        item.Produto.Categoria.Nome = reader["CATEGORY_NAME"].ToString();

                        item.Status = reader["STOCK_ITEM_STATUS"].ToString();

                        estoque.Add(item);
                    }

                    return estoque;
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }

        public void dbAlterarStatusProduto(int stockItemId, string status)
        {
            if (status != "AVAILABLE" && status != "IN CART" && status != "SOLD")
            {
                throw new Exception("Error: status value can only be set to 'AVAILABLE', 'IN CART' or 'SOLD'");
            }

            else
            {
                try
                {
                    string queryString = "UPDATE STOCK";
                    queryString += " SET STOCK_ITEM_STATUS = @STATUS";
                    queryString += " WHERE STOCK_ITEM_ID = @STOCK_ITEM_ID";

                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();

                        //ALTERA O STATUS DO PRODUTO NA TABELA DE ESTOQUE
                        SqlCommand updateStatusCommand = new SqlCommand(queryString, connection);
                        updateStatusCommand.Parameters.Add(new SqlParameter("@STOCK_ITEM_ID", stockItemId));
                        updateStatusCommand.Parameters.Add(new SqlParameter("@STATUS", status));
                        updateStatusCommand.ExecuteNonQuery();

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

        }

        public bool dbItemDisponivel(int itemId)
        {
            try
            {
                string queryString = "SELECT STOCK_ITEM_STATUS FROM STOCK";
                queryString += " WHERE STOCK_ITEM_ID = @ITEMID";

                string status = String.Empty;

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    //BUSCA O ITEM NA TABELA
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@ITEMID", itemId));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        status = reader["STOCK_ITEM_STATUS"].ToString();
                    }

                    if (status == "AVAILABLE")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
