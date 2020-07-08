using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Libraries.Filtro;
using AlugaOffice.Libraries.Lang;
using AlugaOffice.Models;
using AlugaOffice.Models.Constants;
using AlugaOffice.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace AlugaOffice.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public IActionResult Index(int? pagina, string pesquisa)
        {
            IPagedList<Models.Cliente> clientes = _clienteRepository.ObterTodosClientes(pagina, pesquisa);
            return View(clientes);
        }

        [ValidateHttpReferer]
        public IActionResult AtivarDesativar(int id)
        {
            Models.Cliente cliente = _clienteRepository.ObterCliente(id);
            cliente.Situacao = (cliente.Situacao == SituacaoConstant.Ativado) ? cliente.Situacao = SituacaoConstant.Desativado : cliente.Situacao = SituacaoConstant.Desativado;
            _clienteRepository.Atualizar(cliente);

            TempData["MSG_S"] = Mensagem.MSG_S001;

            return RedirectToAction(nameof(Index));
        }
    }
}
