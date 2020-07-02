using AlugaOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Repositories.Contracts
{
    public interface IImagemRepository
    {
        void CadastrarImagens(List<Imagem> ListaImagens, int ProdutoId);
        void Cadastrar(Imagem imagem);
        void Excluir(int Id);
        void ExcluirImagensDoProduto(int ProdutoId);
    }
}
