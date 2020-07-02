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
        private IColaboradorRepository _colaboradorRepository;
        private GerenciarEmail _gerenciarEmail;
        public ColaboradorController(IColaboradorRepository colaboradorRepository, GerenciarEmail gerenciarEmail)
        {
            _colaboradorRepository = colaboradorRepository;
            _gerenciarEmail = gerenciarEmail;
        }
        public IActionResult Index(int? pagina)
        {
            IPagedList<Models.Colaborador> colaboradores = _colaboradorRepository.ObterTodosColaboradores(pagina);
            return View(colaboradores);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm]Models.Colaborador colaborador)
        {
            ModelState.Remove("Senha");
            if (ModelState.IsValid)
            {
                colaborador.Tipo = ColaboradorTipoConstant.Comum;
                colaborador.Senha = KeyGenerator.GetUniqueKey(8);
                _colaboradorRepository.Cadastrar(colaborador);
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
            Models.Colaborador colaborador = _colaboradorRepository.ObterColaborado(id);
            colaborador.Senha = KeyGenerator.GetUniqueKey(8);
            _colaboradorRepository.AtualizarSenha(colaborador);
            _gerenciarEmail.EnviarSenhaParaColaboradorPorEmail(colaborador);

            TempData["MSG_S"] = Mensagem.MSG_S003;
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            Models.Colaborador colaborador = _colaboradorRepository.ObterColaborado(id);
            return View(colaborador);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.Colaborador colaborador, int id)
        {
            ModelState.Remove("Senha");
            if (ModelState.IsValid)
            {
                _colaboradorRepository.Atualizar(colaborador);
                TempData["MSG_S"] = Mensagem.MSG_S001;
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Excluir(int id)
        {
            _colaboradorRepository.Excluir(id);
            TempData["MSG_S"] = Mensagem.MSG_S002;
            return RedirectToAction(nameof(Index));
        }
    }
}
