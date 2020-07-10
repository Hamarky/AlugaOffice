using AlugaOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Repositories.Contracts
{
    public interface IPedidoSituacaoRepository
    {
        void Cadastrar(PedidoSituacao pedidoSituacao);
        void Atualizar(PedidoSituacao pedidoSituacao);
        void Excluir(int id);

        PedidoSituacao ObterPedidoSituacao(int id);
    }
}
