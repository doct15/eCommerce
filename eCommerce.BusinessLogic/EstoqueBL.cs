using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Model;
using eCommerce.DataAccess;
using eCommerce.BusinessLogic.Custom;

namespace eCommerce.BusinessLogic
{
    class EstoqueBL : BaseBL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public EstoqueBL(string _conStr)
        {
            conStr = _conStr;
        }

        public List<ItemEstoque> ListarItemEstoque()
        {
            EstoqueDAL DAL = new EstoqueDAL(conStr);

            return DAL.dbListarItemsEstoque();
        }
    }
}
