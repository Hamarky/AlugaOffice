using AlugaOffice.Database;
using AlugaOffice.Models;
using AlugaOffice.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Repositories
{
    public class ImagemRepository : IImagemRepository
    {
        private AlugaOfficeContext _banco;

        public ImagemRepository(AlugaOfficeContext banco)
        {
            _banco = banco;
        }
        public void CadastrarImagens(List<Imagem> ListaImagens, int ProdutoId)
        {
            if (ListaImagens != null && ListaImagens.Count > 0)
            {
                foreach (var Imagem in ListaImagens)
                {
                    Cadastrar(Imagem);
                }
            }
        }
        public void Cadastrar(Imagem imagem)
        {
            _banco.Add(imagem);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Imagem imagem = _banco.Imagens.Find(Id);
            _banco.Remove(imagem);
            _banco.SaveChanges();
        }

        public void ExcluirImagensDoProduto(int ProdutoId)
        {
            List<Imagem> imagens = _banco.Imagens.Where(a => a.ProdutoId == ProdutoId).ToList();

            foreach (Imagem imagem in imagens)
            {
                _banco.Remove(imagem);
            }
            _banco.SaveChanges();
        }
    }
}
