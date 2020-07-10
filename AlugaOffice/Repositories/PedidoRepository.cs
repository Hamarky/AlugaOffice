using AlugaOffice.Database;
using AlugaOffice.Models;
using AlugaOffice.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace AlugaOffice.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        IConfiguration _conf;
        AlugaOfficeContext _banco;

        public PedidoRepository(AlugaOfficeContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }

        public void Atualizar(Pedido pedido)
        {
            _banco.Update(pedido);
            _banco.SaveChanges();
        }

        public void Cadastrar(Pedido pedido)
        {
            _banco.Add(pedido);
            _banco.SaveChanges();
        }

        public Pedido ObterPedido(int Id)
        {
            return _banco.Pedidos.Include(a => a.PedidoSituacoes).Where(a => a.Id == Id).FirstOrDefault();
        }

        public IPagedList<Pedido> ObterTodosPedidoCliente(int? pagina, int clienteId)
        {
            int RegistroPorPagina = _conf.GetValue<int>("RegistroPorPagina");

            int NumeroPagina = pagina ?? 1;

            return _banco.Pedidos.Include(a => a.PedidoSituacoes).ToPagedList<Pedido>(NumeroPagina, RegistroPorPagina);
        }
    }
}
