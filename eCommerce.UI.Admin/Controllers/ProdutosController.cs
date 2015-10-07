using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.Model;
using eCommerce.UI.Admin.Controllers.Custom;

namespace eCommerce.UI.Admin.Controllers
{
    public class ProdutosController : BaseController
    {

        public ActionResult Index()
        {
            List<Produto> listaProdutos = ProdutoBL.ListarProdutos();

            return View(listaProdutos);
        }

        public ActionResult Editar(int idProduto)
        {
            //Lista as categorias para o combo box
            ViewBag.Categorias = CategoriaBL.ListarCategorias();

            //Obtem o produto
            Produto produto = ProdutoBL.ObterProduto(idProduto);

            return View(produto);
        }

        public ActionResult Novo()
        {
            ViewBag.Categorias = CategoriaBL.ListarCategorias();
            return View();
        }

        [HttpPost]
        public ActionResult RemoverProduto(int idProduto)       
        {
            ProdutoBL.RemoverProduto(idProduto);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AdicionarProduto(FormCollection form)
        {
            string nomeProduto = form["inpNome"].ToString();
            string descricao = form["inpDescricao"].ToString();
            string caminhoImagem = form["inpCaminhoImagem"].ToString();
            decimal preco = Convert.ToDecimal(form["inpPreco"]);
            int categoriaId = Convert.ToInt32(form["inpCategoria"]);

            ProdutoBL.InserirProduto(nomeProduto, descricao, caminhoImagem, preco, categoriaId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditarProduto(FormCollection form)
        {
            string nomeProduto = form["inpNome"].ToString();
            string descricao = form["inpDescricao"].ToString();
            string caminhoImagem = form["inpCaminhoImagem"].ToString();
            decimal preco = Convert.ToDecimal(form["inpPreco"]);
            int categoriaId = Convert.ToInt32(form["inpCategoria"]);
            int idProduto = Convert.ToInt32(form["idProduto"]);

            ProdutoBL.EditarProduto(idProduto, nomeProduto, descricao, caminhoImagem, preco, categoriaId);

            return RedirectToAction("Index");
        }
        

        
    }
}
