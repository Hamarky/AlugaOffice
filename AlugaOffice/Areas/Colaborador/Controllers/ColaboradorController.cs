﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlugaOffice.Libraries.Email;
using AlugaOffice.Libraries.Filtro;
using AlugaOffice.Libraries.Lang;
using AlugaOffice.Libraries.Texto;
using AlugaOffice.Models.Constants;
using AlugaOffice.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using X.PagedList;

namespace AlugaOffice.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao(ColaboradorTipoConstant.Gerente)]
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRespository;
        private GerenciarEmail _gerenciarEmail;

        public ColaboradorController(IColaboradorRepository colaboradorRespository, GerenciarEmail gerenciarEmail)
        {
            _colaboradorRespository = colaboradorRespository;
            _gerenciarEmail = gerenciarEmail;
        }

        public IActionResult Index(int? pagina, string pesquisa)
        {
            IPagedList<Models.Colaborador> colaboradores = _colaboradorRespository.ObterTodosColaboradores(pagina, pesquisa);

            return View(colaboradores);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Models.Colaborador colaborador)
        {
            ModelState.Remove("Senha");
            if (ModelState.IsValid)
            {
                colaborador.Tipo = ColaboradorTipoConstant.Comum;
                colaborador.Senha = KeyGenerator.GetUniqueKey(8);
                _colaboradorRespository.Cadastrar(colaborador);

                _gerenciarEmail.EnviarSenhaParaColaboradorPorEmail(colaborador);

                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult GerarSenha(int id)
        {
            Models.Colaborador colaborador = _colaboradorRespository.ObterColaborador(id);
            colaborador.Senha = KeyGenerator.GetUniqueKey(8);
            _colaboradorRespository.AtualizarSenha(colaborador);

            _gerenciarEmail.EnviarSenhaParaColaboradorPorEmail(colaborador);

            TempData["MSG_S"] = Mensagem.MSG_S003;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            Models.Colaborador colaborador = _colaboradorRespository.ObterColaborador(id);
            return View(colaborador);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.Colaborador colaborador, int id)
        {
            ModelState.Remove("Senha");
            if (ModelState.IsValid)
            {
                _colaboradorRespository.Atualizar(colaborador);

                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Excluir(int id)
        {
            _colaboradorRespository.Excluir(id);

            TempData["MSG_S"] = Mensagem.MSG_S002;

            return RedirectToAction(nameof(Index));
        }
    }
}
