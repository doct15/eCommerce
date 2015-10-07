using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using eCommerce.DataAccess;
using eCommerce.Model;


namespace eCommerce.BusinessLogic.Custom
{
    public class BaseBL
    {
        /*SESSÃO DO CARRINHO DE COMPRAS*/
        public Carrinho SessaoCarrinho
        {
            get { return (Carrinho)HttpContext.Current.Session["ActiveCart"]; } //DA EXPRCTION  QDO N EXISTE A SESSAO
            set { HttpContext.Current.Session["ActiveCart"] = value; }
        }

        /*SESSÃO DOS USUARIOS*/
        public Usuario SessaoUsuario
        {
            get { return (Usuario)HttpContext.Current.Session["LoggedUser"]; }
            set { HttpContext.Current.Session["LoggedUser"] = value; }

        }

        /*SESSÃO DOS USUARIOS*/
        public UsuarioSistema SessaoUsuarioSistema
        {
            get { return (UsuarioSistema)HttpContext.Current.Session["LoggedSysUser"]; }
            set { HttpContext.Current.Session["LoggedSysUser"] = value; }

        }
    }
}
