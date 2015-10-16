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
    public class CategoriaBL :BaseBL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public CategoriaBL(string _conStr)
        {
            conStr = _conStr;
        }

        public List<Categoria> ListarCategorias()
        {
            CategoriaDAL DAL = new CategoriaDAL(conStr);

            return DAL.dbListarCategorias();
        }

        public Categoria ObterCategoria(int idCategoria)
        {
            CategoriaDAL DAL = new CategoriaDAL(conStr);

            //REFACTOR: Está trazendo todos os registros do banco. >>Fazer um metodo de consulta do banco que traga apenas um registro
            //Ou adicionar filtros no metodo existente.
            return DAL.dbListarCategorias().Where(categoria => categoria.ID == idCategoria).First();
        }

        public void InserirCategoria(string nome, string descricao)
        {
            CategoriaDAL DAL = new CategoriaDAL(conStr);

            DAL.dbInserirCategoria(nome, descricao);
        }

        public void RemoverCategoria(int idCategoria)
        {
            CategoriaDAL DAL = new CategoriaDAL(conStr);

            DAL.dbRemoverCategoria(idCategoria);
        }

        public void EditarCategoria(int idCategoria, string categoriaNome, string categoriaDescricao)
        {
            CategoriaDAL DAL = new CategoriaDAL(conStr);

            DAL.dbEditarCategoria(idCategoria, categoriaNome, categoriaDescricao);
        }
    }
}
