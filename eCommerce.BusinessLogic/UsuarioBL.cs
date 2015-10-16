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
    public class UsuarioBL : BaseBL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public UsuarioBL(string _conStr)
        {
            conStr = _conStr;
        }

        /*LISTA TODOS OS USUARIOS*/
        public List<Usuario> ListarUsuarios()
        {
            try
            {
                UsuarioDAL DAL = new UsuarioDAL(conStr);

                return DAL.dbListarUsuarios();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /*INSERE UM NOVO USUARIO*/
        public bool CadastrarUsuario(string nome, string email, string senha)
        {
            UsuarioDAL DAL = new UsuarioDAL(conStr);

            try
            {
                //Caso o e-mail esteja disponivel, insere o usuario na base
                if (DAL.dbEmailDisponivel(email))
                {
                    //string encyptedPass;
                    //DO ENCRYPTION SHIT

                    DAL.dbInserirUsuario(nome, email, senha); //TODO: PASSAR SENHA ENCRYPTADA

                    return true;
                }

                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /*REALIZA O LOGIN DO USUARIO*/
        public bool LoginUsuario(string email, string senha)
        {
            UsuarioDAL DAL = new UsuarioDAL(conStr);
            CarrinhoBL CarrinhoBL = new CarrinhoBL(conStr);

            try
            {
                int userId = DAL.dbValidarUsuario(email, senha);

                //Se for igual a -1 é porque a autenticação falhou
                if (userId.Equals(-1))
                {

                    return false;
                }

                //Autenticação bem sucedida, insere na sessão de login o usuario
                else
                {
                    //COLOCA O USER NA SESSAO
                    SessaoUsuario = DAL.dbObterUsuario(userId);

                    //ATRELA O CARRINHO ATIVO A ESTE USUARIO
                    CarrinhoBL.AtrelarCarrinho(ObterUsuarioLogado().ID, CarrinhoBL.ObterCarrinhoAtivo().ID);

                    return true;
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*LOG OFF DO USUARIO*/
        public void UsuarioLogOff()
        {
            SessaoUsuario = null;
        }

        /*VERIFICA SE O USUARIO ESTÁ LOGADO*/
        public bool UsuarioLogado()
        {
            if (SessaoUsuario == null)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        /*RETORNA O USUARIO QUE ESTÁ LOGADO*/
        public Usuario ObterUsuarioLogado()
        {
            UsuarioDAL DAL = new UsuarioDAL(conStr);
            try
            {
                if (UsuarioLogado())
                {
                    return SessaoUsuario;
                }

                else
                {
                    throw new Exception("ERRO: Não existe nenhum usuario logado no sistema!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
