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
    public class TransacaoDAL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public TransacaoDAL(string _conStr)
        {
            conStr = _conStr;
        }

        //Insere uma transação
        public void dbInserirTransacao(int cartId, decimal valorTotal, int? couponId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("ecSP_TRANSACTION_ADD", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CART_ID", cartId));
                    command.Parameters.Add(new SqlParameter("@VALUE", valorTotal));
                    command.Parameters.Add(new SqlParameter("@COUPON", couponId));
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
