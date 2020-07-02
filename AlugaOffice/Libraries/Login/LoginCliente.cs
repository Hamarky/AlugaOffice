using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Libraries.Sessao;
using AlugaOffice.Models;
using Newtonsoft.Json;

namespace AlugaOffice.Libraries.Login
{
    public class LoginCliente
    {
        private string Key = "Login.Cliente";
        private Sessao.Sessao _sessao;
        public LoginCliente(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Cliente cliente)
        {
            string clienteJSONString = JsonConvert.SerializeObject(cliente);

            _sessao.Cadastrar(Key, clienteJSONString);
        }

        public Cliente GetCliente()
        {
            if (_sessao.Existe(Key))
            {
                string clienteJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Cliente>(clienteJSONString); ;
            }
            else
            {
                return null;
            }
        }

        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
