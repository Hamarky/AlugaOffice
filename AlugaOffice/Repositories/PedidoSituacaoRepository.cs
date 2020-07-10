﻿using AlugaOffice.Database;
using AlugaOffice.Models;
using AlugaOffice.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Repositories
{
    public class PedidoSituacaoRepository : IPedidoSituacaoRepository
    {
        IConfiguration _conf;
        AlugaOfficeContext _banco;

        public PedidoSituacaoRepository(AlugaOfficeContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }

        public void Atualizar(PedidoSituacao pedidoSituacao)
        {
            _banco.Update(pedidoSituacao);
            _banco.SaveChanges();
        }

        public void Cadastrar(PedidoSituacao pedidoSituacao)
        {
            _banco.Add(pedidoSituacao);
            _banco.SaveChanges();
        }

        public void Excluir(int id)
        {
            PedidoSituacao pedidoSituacao = ObterPedidoSituacao(id);
            _banco.Remove(pedidoSituacao);
            _banco.SaveChanges();
        }

        public PedidoSituacao ObterPedidoSituacao(int id)
        {
            return _banco.PedidoSituacoes.Include(a => a.Pedido).Where(a => a.Id == id).FirstOrDefault();
        }
    }
}
