using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Libraries.CarrinhoCompra;
using AlugaOffice.Models;
using AlugaOffice.Models.TodosProdutos;
using AlugaOffice.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlugaOffice.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private CarrinhoCompra _carrinhoCompra;
        private IProdutoRepository _produtoRepository;
        private IMapper _mapper;

        public CarrinhoCompraController(CarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper)
        {
            _carrinhoCompra = carrinhoCompra;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemNoCarrinho = _carrinhoCompra.Consultar();

            List<ProdutoItem> produtoItemCompleto = new List<ProdutoItem>();

            foreach (var item in produtoItemNoCarrinho)
            {
                Produto produto = _produtoRepository.ObterProduto(item.Id);

                ProdutoItem produtoItem = _mapper.Map<ProdutoItem>(produto);
                produtoItem.QuantidadeProduto = item.QuantidadeProduto;

                produtoItemCompleto.Add(produtoItem);
            }

            return View(produtoItemCompleto);
        }

        //Item ID = ID Produto
        public IActionResult AdicionarItem(int id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);

            if (produto == null)
            {
                return View("NaoExisteItem");
            }
            else
            {
                var item = new ProdutoItem() { Id = id, QuantidadeProduto = 1 };
                _carrinhoCompra.Cadastrar(item);

                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult AlterarQuantidade(int id, int quantidade)
        {
            //TODO - Validar se existe essa quantidade no estoque.
            var item = new ProdutoItem() { Id = id, QuantidadeProduto = quantidade };
            _carrinhoCompra.Atualizar(item);
            return Ok();
        }
        public IActionResult RemoverItem(int id)
        {
            _carrinhoCompra.Remover(new ProdutoItem() { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
