﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AlugaOffice.Libraries.CarrinhoCompra;
using AlugaOffice.Libraries.Gerenciador.Frete;
using AlugaOffice.Models.TodosProdutos;
using AlugaOffice.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using AlugaOffice.Libraries.Login;
using AlugaOffice.Libraries.Seguranca;
using Newtonsoft.Json;

namespace AlugaOffice.Controllers.Base
{
    public class BaseController : Controller
    {
        protected CookieCarrinhoCompra _cookieCarrinhoCompra;
        protected IProdutoRepository _produtoRepository;
        protected IMapper _mapper;
        protected WSCorreiosCalcularFrete _wscorreios;
        protected CalcularPacote _calcularPacote;
        protected CookieFrete _cookieFrete;
        public BaseController(CookieCarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, 
            WSCorreiosCalcularFrete wscorreios, CalcularPacote calcularPacote, CookieFrete cookieFrete)
        {
            _cookieCarrinhoCompra = carrinhoCompra;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _wscorreios = wscorreios;
            _calcularPacote = calcularPacote;
            _cookieFrete = cookieFrete;
        }
        protected List<ProdutoItem> CarregarProdutoDB()
        {
            List<ProdutoItem> produtoItemNoCarrinho = _cookieCarrinhoCompra.Consultar();

            List<ProdutoItem> produtoItemCompleto = new List<ProdutoItem>();

            foreach (var item in produtoItemNoCarrinho)
            {
                Produto produto = _produtoRepository.ObterProduto(item.Id);

                ProdutoItem produtoItem = _mapper.Map<ProdutoItem>(produto);
                produtoItem.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;

                produtoItemCompleto.Add(produtoItem);
            }

            return produtoItemCompleto;
        }

        protected string GerarHash(object obj)
        {
            return StringMD5.MD5Hash(JsonConvert.SerializeObject(obj));
        }
    }
}