using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eCommerce.BusinessLogic;
using eCommerce.Model;
using eCommerce.UI.Admin.Controllers.Custom;

namespace eCommerce.UI.Admin.Controllers.Custom
{
    public class BaseController : Controller
    {
        //String de conexão com o banco de dados
        public static string conStr = ConfigurationManager.ConnectionStrings["ConnectionStringSql"].ToString();

        //Acesso à business layer
        public UsuarioSistemaBL UsuarioSistemaBL = new UsuarioSistemaBL(conStr);
        public CategoriaBL CategoriaBL = new CategoriaBL(conStr);
        public ProdutoBL ProdutoBL = new ProdutoBL(conStr);


        //Antes de executar cada ACtion...
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //VERIFICA SE O USUARIO ESTÁ LOGADO
            if(!filterContext.Controller.ToString().Contains("Login"))
            {
                if (!UsuarioSistemaBL.UsuarioLogado())
                {
                    //RedirectToLogin();
                    filterContext.Result = RedirectToAction("Index", "Login");
                    return;
                }
            }

        }  

    }
}
