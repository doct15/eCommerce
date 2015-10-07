using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.UI.Admin.Controllers.Custom;
using eCommerce.Model;

namespace eCommerce.UI.Admin.Controllers
{
    public class UsuariosController : BaseController
    {

        public ActionResult Index()
        {
            //Lista todos os usuarios do sistema
            List<UsuarioSistema> listaUsuarios = UsuarioSistemaBL.ListarUsuariosSistema();

            return View(listaUsuarios);
        }

        public ActionResult Novo()
        {
            return View();
        }

        public ActionResult AdicionarUsuario(FormCollection form)
        {
            string username = form["inpUsername"].ToString();
            string senha = form["inpSenha"].ToString();

            if(UsuarioSistemaBL.CadastrarUsuarioSistema(username, senha))
            {
                return RedirectToAction("Index", "Usuarios");
            }

            //Ja existe um usuario com esse username....
            else
            {
                //ViewBag usuario ja existe...
                return RedirectToAction("NovoUsuario", "Usuarios");
            }
        }

        public ActionResult RemoverUsuario(FormCollection form)
        {
            int idUsuario = Convert.ToInt32(form["inpIdUsuario"]);

            UsuarioSistemaBL.RemoverUsuarioSistema(idUsuario);

            return RedirectToAction("Index", "Usuarios");
        }

    }
}
