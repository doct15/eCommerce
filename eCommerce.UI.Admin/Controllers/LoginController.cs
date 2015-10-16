using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.UI.Admin.Controllers.Custom;

namespace eCommerce.UI.Admin.Controllers
{
    public class LoginController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult EfetuarLogin(FormCollection form)
        {
   
            string usuario = form["inpUsername"].ToString();
            string senha = form["inpSenha"].ToString();

            //Realiza o Login, e caso os dados sejam invalidos, retorna false;
            if(UsuarioSistemaBL.LoginUsuarioSistema(usuario, senha))
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                TempData["UsuarioInvalido"] = true;
                return RedirectToAction("Index", "Login");
            }
                
        }

    }
}
