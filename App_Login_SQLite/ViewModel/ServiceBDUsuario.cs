using System;
using System.Collections.Generic;
using System.Text;
using App_Login_SQLite;
using App_Login_SQLite.Model;
using SQLite;

namespace App_Login_SQLite.ViewModel
{
    public class ServiceBDUsuario
    {
        SQLiteConnection conn;
        public string StatusMessage { get; set; }
        public ServiceBDUsuario(string dbPaht)
        {
            if (dbPaht == "") dbPaht = App.DbPath;
            conn = new SQLiteConnection(dbPaht);
            conn.CreateTable<ModelUsuario>();
        }

        public void Inserir(ModelUsuario user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Nome))
                    throw new Exception("Usuario não informado");
                if (string.IsNullOrEmpty(user.Senha))
                    throw new Exception("Senha não informados");
                    int result = conn.Insert(user);
                if (result != 0)
                {
                    this.StatusMessage =
                        string.Format("{0} registro(s) adicionado(s):[Usuario:{1}",
                        result, user.Nome);
                }
                else
                {
                    string.Format("0 registro (s) adicionado(s)");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
  
  	//Método de validação e Login
	public ModelUsuario ValidarLogin(string usuario, string senha)
        {
            try
            {
                var p = conn.Table<ModelUsuario>();
                var result = p.Where(x => x.Nome == usuario && x.Senha == senha).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Método Alterar
        public void Alterar(ModelUsuario user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Nome))
                    throw new Exception("Usuário não informado");
                if (string.IsNullOrEmpty(user.Senha))
                    throw new Exception("Senha não informada");
                if (user.Id <= 0)
                    throw new Exception("Id da nota não informado");
                //atualiza todas as colunas da tabela do objeto que estou passando.
                int result = conn.Update(user);
                StatusMessage = string.Format("{0} Registros alterados.", result);
                //return p;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
        }

        //Método de excluir um Registro
        public void Excluir(int id)
        {
            try
            {
                //recebe o id do registro a ser deletado - se Id da table igual ao id, deleta
                int result = conn.Table<ModelUsuario>().Delete(r => r.Id == id);//
                StatusMessage = string.Format("{0} Registros deletados.", result);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
        }
    }
}
