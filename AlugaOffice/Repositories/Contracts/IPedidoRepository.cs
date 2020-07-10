using AlugaOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace AlugaOffice.Repositories.Contracts
{
    public interface IPedidoRepository
    {
        void Cadastrar(Pedido pedido);
        void Atualizar(Pedido pedido);
        Pedido ObterPedido(int Id);
        IPagedList<Pedido> ObterTodosPedidoCliente(int? pagina, int clienteId);
    }
}
