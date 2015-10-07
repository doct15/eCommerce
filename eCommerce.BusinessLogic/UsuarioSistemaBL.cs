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
    public class UsuarioSistemaBL : BaseBL
    {
        //Método construtor que recebe a string de conexão
        string conStr;
        public UsuarioSistemaBL(string _conStr)
        {
            conStr = _conStr;
        }

        /*REALIZA O LOGIN DO USUARIO*/
        public bool LoginUsuarioSistema(string nome, string senha)
        {
            UsuarioSistemaDAL DAL = new UsuarioSistemaDAL(conStr);

            try
            {
                int userId = DAL.dbValidarUsuarioSistema(nome, senha);

                //Se for igual a -1 é porque a autenticação falhou
                if (userId.Equals(-1))
                {
                    return false;
                }

                //Autenticação bem sucedida, insere na sessão de login o usuario
                else
                {
                    //COLOCA O USER NA SESSAO
                    SessaoUsuarioSistema = DAL.dbObterUsuarioSistema(userId);

                    return true;
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*VERIFICA SE O USUARIO ESTÁ LOGADO*/
        public bool UsuarioLogado()
        {
            if (SessaoUsuarioSistema != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*LISTAR OS USUARIOS*/
        public List<UsuarioSistema> ListarUsuariosSistema()
        {
            try
            {
                UsuarioSistemaDAL DAL = new UsuarioSistemaDAL(conStr);

                return DAL.dbListarUsuariosSistema();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*INSERE UM NOVO USUARIO*/
        public bool CadastrarUsuarioSistema(string username, string senha)
        {
            UsuarioSistemaDAL DAL = new UsuarioSistemaDAL(conStr);

            try
            {
                //Caso o e-mail esteja disponivel, insere o usuario na base
                if (DAL.dbUsuarioDisponivel(username))
                {
                    //string encyptedPass;
                    //DO ENCRYPTION SHIT

                    DAL.dbInserirUsuarioSistema(username, senha); //TODO: PASSAR SENHA ENCRYPTADA

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

        /*REMOVE UM USUARIO*/
        public void RemoverUsuarioSistema(int userId)
        {
            UsuarioSistemaDAL DAL = new UsuarioSistemaDAL(conStr);

            try
            {
                DAL.dbRemoverUsuarioSistema(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
