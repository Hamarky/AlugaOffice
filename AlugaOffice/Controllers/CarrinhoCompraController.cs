using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Controllers.Base;
using AlugaOffice.Libraries.CarrinhoCompra;
using AlugaOffice.Libraries.Filtro;
using AlugaOffice.Libraries.Gerenciador.Frete;
using AlugaOffice.Libraries.Lang;
using AlugaOffice.Libraries.Login;
using AlugaOffice.Libraries.Seguranca;
using AlugaOffice.Models;
using AlugaOffice.Models.Constants;
using AlugaOffice.Models.TodosProdutos;
using AlugaOffice.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AlugaOffice.Controllers
{
    public class CarrinhoCompraController : BaseController
    {

        private LoginCliente _loginCliente;
        private IEnderecoEntregaRepository _enderecoEntregaRepository;
        public CarrinhoCompraController(CookieCarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository,
            IMapper mapper, WSCorreiosCalcularFrete wscorreios, CalcularPacote calcularPacote,
            CookieFrete cookieValorPrazoFrete, LoginCliente loginCliente, IEnderecoEntregaRepository enderecoEntregaRepository)
            : base(carrinhoCompra, produtoRepository, mapper, wscorreios,
                calcularPacote, cookieValorPrazoFrete)
        {
            _loginCliente = loginCliente;
            _enderecoEntregaRepository = enderecoEntregaRepository;
        }

        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

            return View(produtoItemCompleto);
        }

        public IActionResult AdicionarItem(int id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);

            if (produto == null)
            {
                return View("NaoExisteItem");
            }
            else
            {
                var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = 1 };
                _cookieCarrinhoCompra.Cadastrar(item);

                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult AlterarQuantidade(int id, int quantidade)
        {
            Produto produto = _produtoRepository.ObterProduto(id);
            if (quantidade < 1)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E007 });
            }
            else if (quantidade > produto.Quantidade)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E008 });
            }
            else
            {
                var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = quantidade };
                _cookieCarrinhoCompra.Atualizar(item);
                return Ok(new { mensagem = Mensagem.MSG_S001 });
            }
        }
        public IActionResult RemoverItem(int id)
        {
            _cookieCarrinhoCompra.Remover(new ProdutoItem() { Id = id });
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> CalcularFrete(int cepDestino)
        {
            try
            {
                List<ProdutoItem> produtos = CarregarProdutoDB();

                List<Pacote> pacotes = _calcularPacote.CalcularPacotesDeProdutos(produtos);

                ValorPrazoFrete valorPAC = await _wscorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.PAC, pacotes);
                ValorPrazoFrete valorSEDEX = await _wscorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX, pacotes);

                List<ValorPrazoFrete> lista = new List<ValorPrazoFrete>();
                if (valorPAC != null) lista.Add(valorPAC);
                if (valorSEDEX != null) lista.Add(valorSEDEX);

                StringMD5.MD5Hash(JsonConvert.SerializeObject(_cookieCarrinhoCompra.Consultar()));

                var frete = new Frete()
                {
                    CEP = cepDestino,
                    CodCarrinho = GerarHash(_cookieCarrinhoCompra.Consultar()),
                    ListaValores = lista
                };

                _cookieFrete.Cadastrar(frete);

                return Ok(frete);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [ClienteAutorizacao]
        public IActionResult EnderecoEntrega()
        {

            Cliente cliente = _loginCliente.GetCliente();
            IList<EnderecoEntrega> enderecos = _enderecoEntregaRepository.ObterTodosEnderecoEntregaCliente(cliente.Id);
            ViewBag.Cliente = cliente;
            ViewBag.Enderecos = enderecos;
            return View();
        }


    }
}