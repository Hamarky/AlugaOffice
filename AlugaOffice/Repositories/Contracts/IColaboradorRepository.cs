using AlugaOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace AlugaOffice.Repositories.Contracts
{
    public interface IColaboradorRepository
    {
        Colaborador Login(string Email, string Senha);

        void Cadastrar(Colaborador colaborador);
        void Atualizar(Colaborador colaborador);
        void AtualizarSenha(Colaborador colaborador);
        void Excluir(int Id);

        Colaborador ObterColaborado(int Id);
        List<Colaborador> ObterColaboradoresPorEmail(string email);
        IPagedList<Colaborador> ObterTodosColaboradores(int? pagina);

    }
}
