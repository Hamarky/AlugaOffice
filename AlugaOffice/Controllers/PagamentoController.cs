﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Controllers.Base;
using AlugaOffice.Libraries.CarrinhoCompra;
using AlugaOffice.Libraries.Cookie;
using AlugaOffice.Libraries.Filtro;
using AlugaOffice.Libraries.Gerenciador.Frete;
using AlugaOffice.Libraries.Lang;
using AlugaOffice.Models.TodosProdutos;
using AlugaOffice.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlugaOffice.Controllers
{
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        public PagamentoController(Cookie cookie, CookieCarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wscorreios, CalcularPacote calcularPacote, CookieFrete cookieValorPrazoFrete) : base(carrinhoCompra, produtoRepository, mapper, wscorreios, calcularPacote, cookieValorPrazoFrete)
        {
            _cookie = cookie;
        }

        [ClienteAutorizacao]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            if (tipoFreteSelecionadoPeloUsuario != null)
            {/*
                var frete = _cookieFrete.Consultar().Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();

                if (frete != null)
                {
                    ViewBag.Frete = frete;
                    List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

                    return View(produtoItemCompleto);
                }
                */
            }

            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction("Index", "CarrinhoCompra");
        }
    }
}
