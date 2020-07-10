using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Libraries.Filtro;
using AlugaOffice.Libraries.Json.Resolver;
using AlugaOffice.Libraries.Login;
using AlugaOffice.Models;
using AlugaOffice.Models.TodosProdutos;
using AlugaOffice.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AlugaOffice.Controllers
{
    [ClienteAutorizacao]
    public class PedidoController : Controller
    {
        private IPedidoRepository _pedidoRepository;
        private LoginCliente _loginCliente;
        public PedidoController(IPedidoRepository pedidoRepository, LoginCliente loginCliente)
        {
            _pedidoRepository = pedidoRepository;
            _loginCliente = loginCliente;
        }
        public IActionResult Index(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (pedido.ClienteId != _loginCliente.GetCliente().Id)
            {
                return new ContentResult() { Content = "Acesso negado. Cliente não autorizada para este pedido." };
            }

            ViewBag.Produtos = JsonConvert.DeserializeObject<List<ProdutoItem>>(
                pedido.DadosProdutos,
                new JsonSerializerSettings() { ContractResolver = new ProdutoItemResolver<List<ProdutoItem>>() }
            );

            var transacao = JsonConvert.DeserializeObject<TransacaoPagarMe>(pedido.DadosTransaction);

            ViewBag.Transacao = transacao;

            return View(pedido);
        }
    }
}
