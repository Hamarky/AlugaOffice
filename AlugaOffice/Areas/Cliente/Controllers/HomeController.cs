using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Libraries.Filtro;
using AlugaOffice.Libraries.Login;
using AlugaOffice.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using AlugaOffice.Models;
using Microsoft.AspNetCore.WebSockets.Internal;

namespace AlugaOffice.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private IClienteRepository _repositoryCliente;
        private LoginCliente _loginCliente;
        private IEnderecoEntregaRepository _enderecoEntregaRepository;

        public HomeController(IClienteRepository repositoryCliente, LoginCliente loginCliente,
            IEnderecoEntregaRepository enderecoEntregaRepository)
        {
            _repositoryCliente = repositoryCliente;
            _loginCliente = loginCliente;
            _enderecoEntregaRepository = enderecoEntregaRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Models.Cliente cliente, string returnUrl = null)
        {
            Models.Cliente clienteDB = _repositoryCliente.Login(cliente.Email, cliente.Senha);

            if (clienteDB != null)
            {
                if (clienteDB.Situacao == Models.Constants.SituacaoConstant.Desativado)
                {
                    ViewData["MSG_E"] = "Seu Login foi desativado";
                    return View();
                }
                else
                {
                    _loginCliente.Login(clienteDB);

                    if (returnUrl == null)
                    {
                        return new RedirectResult("~/");
                    }
                    else
                    {
                        return LocalRedirectPermanent(returnUrl);
                    }
                }
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o e-mail e senha digitado!";
                return View();
            }
        }

        [HttpGet]
        [ClienteAutorizacao]
        public IActionResult Painel()
        {
            return new ContentResult() { Content = "Este é o Painel do Cliente!" };
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastroCliente([FromForm] Models.Cliente cliente, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                cliente.Situacao = Models.Constants.SituacaoConstant.Ativado;
                _repositoryCliente.Cadastrar(cliente);
                _loginCliente.Login(cliente);

                TempData["MSG_S"] = "Cadastro realizado com sucesso!";

                if (returnUrl == null)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult CadastroEnderecoEntrega()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroEnderecoEntrega([FromForm] EnderecoEntrega enderecoentrega, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                enderecoentrega.ClienteId = _loginCliente.GetCliente().Id;

                _enderecoEntregaRepository.Cadastrar(enderecoentrega);

                if (returnUrl == null)
                {

                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Sair()
        {
            _loginCliente.Logout();

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
