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
    public class CarrinhoDAL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public CarrinhoDAL(string _conStr)
        {
            conStr = _conStr;
        }


        //Cria um carrinho para quem está navegando, é acionado quando alguem primeiro coloca um produto no carrinho
        public int dbCriarCarrinho()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_CART_CREATE", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    //Retorna o ID do carrinho recém criado
                    return Convert.ToInt32(command.ExecuteScalar());

                }
            }

            catch (Exception ex)
            {
                throw new Exception("CarrinhoDAL - dbCriarCarrinho: " + ex.Message);
            }
        }

        //Verifica se o usuario tem ou nao carrinho
        public bool dbUsuarioTemCarrinho(int userId)
        {
            try
            {
                string queryString = "SELECT COUNT(*) NUM FROM CARTS WHERE CART_ID = @USERID";
                int retornoQuery = -1;

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@USERID", userId));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        retornoQuery = Convert.ToInt32(reader["NUM"]);
                    }

                }

                //Se tiver mais de um registro, retorna falso
                if (retornoQuery != 0)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                throw new Exception("CarrinhoDAL - dbUsuarioTemCarrinho: " + ex.Message);
            }
        }

        //Retorna o objeto Carrinho com o ID passado
        public Carrinho dbObterCarrinho(int cartId)
        {
            Carrinho carrinho = new Carrinho();
            carrinho.Produtos = new List<ItemEstoque>();

            try
            {

                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_CART_INFO", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CARTID", cartId));

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //ID
                        carrinho.ID = Convert.ToInt32(reader["CART_ID"]);

                        //DATA
                        carrinho.DataCriacao = Convert.ToDateTime(reader["CART_CREATION_DATE"]);

                        //Status
                        carrinho.Status = reader["CART_STATUS"].ToString();

                        //Se o carrinho tem um usuario atrelado a ele
                        if (reader["USER_ID"] != DBNull.Value)
                        {
                            carrinho.Usuario = new Usuario();

                            carrinho.Usuario.ID = Convert.ToInt32(reader["USER_ID"]);
                            carrinho.Usuario.Nome = reader["USER_NAME"].ToString();
                            carrinho.Usuario.Email = reader["USER_EMAIL"].ToString();
                            //TODO: COLOCAR O RESTO DAS PROPRIEDADES
                        }

                        //Se o carrinho tem items...
                        if (reader["STOCK_ITEM_ID"] != DBNull.Value)
                        {
                            ItemEstoque item = new ItemEstoque();
                            item.ID = Convert.ToInt32(reader["STOCK_ITEM_ID"]);
                            item.Status = reader["STOCK_ITEM_STATUS"].ToString();

                            //Produto
                            item.Produto = new Produto();
                            item.Produto.ID = Convert.ToInt32(reader["PRODUCT_ID"]);
                            item.Produto.Nome = reader["PRODUCT_NAME"].ToString();
                            item.Produto.Descricao = reader["PRODUCT_DESCRIPTION"].ToString();
                            item.Produto.CaminhoImagem = reader["PRODUCT_IMAGEPATH"].ToString();
                            item.Produto.Preco = Convert.ToDecimal(reader["PRODUCT_PRICE"]);

                            //Categoria
                            item.Produto.Categoria = new Categoria();
                            item.Produto.Categoria.ID = Convert.ToInt32(reader["CATEGORY_ID"]);
                            item.Produto.Categoria.Nome = reader["CATEGORY_NAME"].ToString();
                            item.Produto.Categoria.Descricao = reader["CATEGORY_DESCRIPTION"].ToString();

                            //ADICIONA À LISTA DE PRODUTOS
                            carrinho.Produtos.Add(item);
                        }

                    }

                    return carrinho;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("UsuarioDAL - ObterUsuario: " + ex.Message);
            }

        }

        //Insere produto 
        public void dbInserirProduto(int cartId, int stockItemId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    //INSERE OS DEVIDOS VALORES NA TABELA CART_PRODUCTS
                    SqlCommand addCartCommand = new SqlCommand("ecSP_CART_ADD_PRODUCT", connection);
                    addCartCommand.CommandType = CommandType.StoredProcedure;
                    addCartCommand.Parameters.Add(new SqlParameter("@CART_ID", cartId));
                    addCartCommand.Parameters.Add(new SqlParameter("@STOCK_ITEM_ID", stockItemId));
                    addCartCommand.ExecuteNonQuery();


                }
            }

            catch (Exception ex)
            {
                throw new Exception("CarrinhoDAL - dbAdicionarProduto: " + ex.Message);
            }
        }

        //Remove produto
        public void dbRemoverProduto(int cartId, int cartItemId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();
                    SqlCommand addCartCommand = new SqlCommand("ecSP_CART_REMOVE_PRODUCT", connection);
                    addCartCommand.CommandType = CommandType.StoredProcedure;
                    addCartCommand.Parameters.Add(new SqlParameter("@CART_ID", cartId));
                    addCartCommand.Parameters.Add(new SqlParameter("@CART_ITEM_ID", cartItemId));
                    addCartCommand.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Atrela um usuario ao carrinho
        public void dbAtrelarUsuario(int userId, int cartId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    //ATUALIZA O CARRINHO, COLOCANDO NELE O USUARIO
                    SqlCommand addCartCommand = new SqlCommand("ecSP_CART_ADD_USER", connection);
                    addCartCommand.CommandType = CommandType.StoredProcedure;
                    addCartCommand.Parameters.Add(new SqlParameter("@USER_ID", userId));
                    addCartCommand.Parameters.Add(new SqlParameter("@CART_ID", cartId));
                    addCartCommand.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //Altera o status do carrinho
        public void dbAlterarStatus(int cartId, string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    string queryString = "UPDATE CARTS SET CART_STATUS = @STATUS WHERE CART_ID = @CART_ID";

                    //MUDA O STATUS DO CARRINHO PARA ABANDONADO
                    SqlCommand addCartCommand = new SqlCommand(queryString, connection);
                    addCartCommand.Parameters.Add(new SqlParameter("@CART_ID", cartId));
                    addCartCommand.Parameters.Add(new SqlParameter("@STATUS", status));
                    addCartCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //DEPRECATED - Abandona o carrinho
        public void dbAbandonarCarrinho(int cartId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    //MUDA O STATUS DO CARRINHO PARA ABANDONADO
                    SqlCommand addCartCommand = new SqlCommand("ecSP_CART_ABANDON", connection);
                    addCartCommand.CommandType = CommandType.StoredProcedure;
                    addCartCommand.Parameters.Add(new SqlParameter("@CART_ID", cartId));
                    addCartCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
