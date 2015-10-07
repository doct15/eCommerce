using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.Model;
using eCommerce.UI.Admin.Controllers.Custom;

namespace eCommerce.UI.Admin.Controllers
{
    public class CategoriasController : BaseController
    {

        public ActionResult Index()
        {
            List<Categoria> listaCategorias = new List<Categoria>();

            listaCategorias = CategoriaBL.ListarCategorias();

            return View(listaCategorias);
        }

        public ActionResult Nova()
        {
            return View();
        }

        public ActionResult Editar(int idCategoria)
        {
            Categoria categoria = CategoriaBL.ObterCategoria(idCategoria);


            return View(categoria);
        }

        public ActionResult AdicionarCategoria(FormCollection form)
        {
            string nomeCategoria = form["inpNome"].ToString();
            string descricao = form["inpDescricao"].ToString();

            CategoriaBL.InserirCategoria(nomeCategoria, descricao);

            return RedirectToAction("Index", "Categorias");
        }

        public ActionResult RemoverCategoria(FormCollection form)
        {
            int idCategoria = Convert.ToInt32(form["inpIdCategoria"]);

            CategoriaBL.RemoverCategoria(idCategoria);

            return RedirectToAction("Index", "Categorias");
        }

        public ActionResult EditarCategoria(FormCollection form)
        {
            int idCategoria = Convert.ToInt32(form["idCategoria"]);
            string nomeCategoria = form["inpNome"].ToString();
            string descricao = form["inpDescricao"].ToString();

            CategoriaBL.EditarCategoria(idCategoria, nomeCategoria, descricao);

            return RedirectToAction("Index", "Categorias");
        }

        
    }
}
