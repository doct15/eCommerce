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
    public class CarrinhoBL : BaseBL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public CarrinhoBL(string _conStr)
        {
            conStr = _conStr;
        }

        #region METODOS PUBLICOS

        /*INSERE UM CARRINHO NA BASE, E O COLOCA NA SESSSAO*/
        public void CriarCarrinho()
        {
            CarrinhoDAL DAL = new CarrinhoDAL(conStr);

            //Cria um carrinho no banco, retornando o seu ID e armazenando na variavel
            int idCarrinho = DAL.dbCriarCarrinho();

            //Busca do Banco todos os dados deste carrinho e armazena na sessao
            SessaoCarrinho = DAL.dbObterCarrinho(idCarrinho);

        }

        /*VERIFICA SE EXISTE UM CARRINHO ATIVO*/
        public bool ExisteCarrinho()
        {
            if (SessaoCarrinho == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /*ADICIONA UM PRODUTO NO CARRINHO*/
        public bool AdicionarProduto(int stockItemId)
        {
            try
            {
                CarrinhoDAL carrinhoDAL = new CarrinhoDAL(conStr);
                EstoqueDAL estoqueDAL = new EstoqueDAL(conStr);

                //Se o item estiver disponivel, insere o produto no carrinho
                if (estoqueDAL.dbItemDisponivel(stockItemId))
                {
                    //Insere na base
                    carrinhoDAL.dbInserirProduto(ObterCarrinhoAtivo().ID, stockItemId);

                    //Altera o status do produto no estoque para 'IN CART', ou seja, dentro do carrinho
                    //estoqueDAL.dbAlterarStatusProduto(stockItemId, "IN CART");

                    //Atualiza a sessão
                    AtualizaCarrinho();

                    return true;

                }

                else
                {
                    AtualizaCarrinho();

                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /*REMOVE UM PRODUTO DO CARRINHO*/
        public void RemoverProduto(int cartItemId)
        {
            try
            {
                CarrinhoDAL carrinhoDAL = new CarrinhoDAL(conStr);
                EstoqueDAL estoqueDAL = new EstoqueDAL(conStr);

                //Remove o produto do carrinho
                carrinhoDAL.dbRemoverProduto(ObterCarrinhoAtivo().ID, cartItemId);

                //Altera o status do produto no estoque para 'AVAILABLE', tendo sido removido do carrinho
                estoqueDAL.dbAlterarStatusProduto(cartItemId, "AVAILABLE");

                //Atualiza a sessão
                AtualizaCarrinho();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public void CheckoutCarrinho()
        {
            CarrinhoDAL carrinhoDAL = new CarrinhoDAL(conStr);
            TransacaoDAL transacaoDAL = new TransacaoDAL(conStr);
            EstoqueDAL estoqueDAL = new EstoqueDAL(conStr);

            try
            {
                int idCarrinho = ObterCarrinhoAtivo().ID;
                int? idCupom = ObterCarrinhoAtivo().Cupom != null ? ObterCarrinhoAtivo().Cupom.ID : (int?)null;

                //Se todos os produtos dentro do carrinho ainda estão disponiveis para a venda
                if (RevisaoCarrinho())
                {

                    //Modifica o status do carrinho para convertido
                    carrinhoDAL.dbAlterarStatus(idCarrinho, "CONVERTED");

                    //Modifica o status dos items do estoque para vendido
                    foreach (ItemEstoque item in ObterCarrinhoAtivo().Produtos)
                    {
                        estoqueDAL.dbAlterarStatusProduto(item.ID, "SOLD");
                    }

                    //Insere a transação na base;
                    transacaoDAL.dbInserirTransacao(idCarrinho, ObterCarrinhoAtivo().ValorTotal(), idCupom);
                }
                else
                {
                    //TODO - FUNCAO DE RECUPERACAO DE INTEGRIDADE;
                    //1 - BUSCA OS PRODUTOS QUE ESTÃO VENDIDOS NO CARRINHO;
                    //2 - REMOVE ELES DO CARRINHO
                    //3 - PEDE DESCULPA
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*DEPRECATED - ABANDONA O CARRINHO*/
        public void AbandonarCarrinho()
        {
            //ALTERA O STATUS DO CARRINHO PARA ABANDONADO

            //SELECIONA OS PRODUTOS DELE E MUDA STATUS PARA AVAILABLE
        }

        /*ATRELA O CARRINHO AO USUARIO*/
        public void AtrelarCarrinho(int userId, int cartId)
        {
            try
            {
                CarrinhoDAL carrinhoDAL = new CarrinhoDAL(conStr);

                carrinhoDAL.dbAtrelarUsuario(userId, cartId);

                //Atualiza a sessão
                AtualizaCarrinho();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /*RETORNA O CARRINHO ATIVO*/
        public Carrinho ObterCarrinhoAtivo()
        {
            CarrinhoDAL DAL = new CarrinhoDAL(conStr);

            if (ExisteCarrinho())
            {
                return SessaoCarrinho;

            }

            else
            {
                throw new Exception("ERRO: Não existe carrinho ativo!");
            }
        }

        #endregion

        #region METODOS PRIVADOS

        /*ATUALIZA A SESSAO DO CARRINHO COM O QUE ESTÁ NA BASE. DEVE SER CHAMADO SEMPRE QUE EXISTIR UMA ALTERAÇÃO NO CARRINHO*/
        private void AtualizaCarrinho()
        {
            CarrinhoDAL DAL = new CarrinhoDAL(conStr);
            int idObterCarrinhoAtivo = ObterCarrinhoAtivo().ID;

            SessaoCarrinho = DAL.dbObterCarrinho(idObterCarrinhoAtivo);
        }

        /*REVISA, ANTES DE EFETIVAR A TRANSAÇÃO, SE OS PRODUTOS CONTINUAM DISPONIVEIS*/
        private bool RevisaoCarrinho()
        {
            EstoqueDAL estoqueDAL = new EstoqueDAL(conStr);

            foreach (ItemEstoque item in ObterCarrinhoAtivo().Produtos)
            {
                if (!estoqueDAL.dbItemDisponivel(item.ID))
                {
                    return false;
                }

            }

            return true;
        }

        /*DEPRECATED - ABANDONA O CARRINHO*/
        public void AbandonarCarrinho(int cartId)
        {
            CarrinhoDAL carrinhoDAL = new CarrinhoDAL(conStr);
            EstoqueDAL estoqueDAL = new EstoqueDAL(conStr);

            try
            {
                //Obtem o carrinho
                Carrinho carrinho = carrinhoDAL.dbObterCarrinho(cartId);

                //Status do carrinho fica como abandonado
                carrinhoDAL.dbAbandonarCarrinho(carrinho.ID);

                //Muda o status dos produtos do carrinho para available novamente
                foreach (ItemEstoque item in carrinho.Produtos)
                {
                    estoqueDAL.dbAlterarStatusProduto(item.ID, "AVAILABLE");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion
    }
}
