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
    public class ProdutoBL : BaseBL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public ProdutoBL(string _conStr)
        {
            conStr = _conStr;
        }

        public List<Produto> ListarProdutos()
        {
            ProdutoDAL DAL = new ProdutoDAL(conStr);

            return DAL.dbListarProdutos();
        }

        public void InserirProduto(string nome, string descricao, string caminhoImagem, decimal preco, int idCategoria)
        {
            ProdutoDAL DAL = new ProdutoDAL(conStr);

            DAL.dbInserirProduto(nome, descricao, caminhoImagem, preco, idCategoria);
        }

        public void EditarProduto(int idProduto, string nome, string descricao, string caminhoImagem, decimal preco, int idCategoria)
        {
            ProdutoDAL DAL = new ProdutoDAL(conStr);

            DAL.dbEditarProduto(idProduto, nome, descricao, caminhoImagem, preco, idCategoria);
        }

        public void RemoverProduto(int idProduto)
        {
            ProdutoDAL DAL = new ProdutoDAL(conStr);

            DAL.dbRemoverProduto(idProduto);

        }

        public Produto ObterProduto(int idProduto)
        {
            ProdutoDAL DAL = new ProdutoDAL(conStr);

            //REFACTOR: Está trazendo todos os registros do banco. >>Fazer um metodo de consulta do banco que traga apenas um registro
            //Ou adicionar filtros no metodo existente.
            return DAL.dbListarProdutos().Where(produto => produto.ID == idProduto).First();
        }
    }
}
