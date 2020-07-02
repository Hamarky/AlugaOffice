using AlugaOffice.Database;
using AlugaOffice.Models;
using AlugaOffice.Models.TodosProdutos;
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
    public class ProdutoRepository : IProdutoRepository
    {
        IConfiguration _conf;
        AlugaOfficeContext _banco;

        public ProdutoRepository(AlugaOfficeContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }

        public void Atualizar(Produto produto)
        {
            _banco.Update(produto);
            _banco.SaveChanges();
        }

        public void Cadastrar(Produto produto)
        {
            _banco.Add(produto);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Produto produto = ObterProduto(Id);
            _banco.Remove(produto);
            _banco.SaveChanges();
        }

        public Produto ObterProduto(int Id)
        {
            return _banco.Produtos.Include(a => a.Imagens).OrderBy(a=> a.Nome).Where(a => a.Id == Id).FirstOrDefault();
        }

        public IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa)
        {
            return ObterTodosProdutos(pagina, pesquisa, "A", null);
        }

        public IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa, string ordenacao, IEnumerable<Categoria> categorias)
        {
            int RegistroPorPagina = _conf.GetValue<int>("RegistroPorPagina");

            int NumeroPagina = pagina ?? 1;

            var bancoProduto = _banco.Produtos.AsQueryable();
            if (!string.IsNullOrEmpty(pesquisa))
            {
                bancoProduto = bancoProduto.Where(a => a.Nome.Contains(pesquisa.Trim()));
            }
            if (ordenacao == "A")
            {
                bancoProduto = bancoProduto.OrderBy(a => a.Nome);
            }
            if (ordenacao == "MAP")
            {
                bancoProduto = bancoProduto.OrderByDescending(a => a.Valor);
            }
            if (ordenacao == "MEP")
            {
                bancoProduto = bancoProduto.OrderBy(a => a.Valor);
            }
            if (categorias != null && categorias.Count() > 0)
            {
                bancoProduto = bancoProduto.Where(a => categorias.Select(b => b.Id).Contains(a.CategoriaId));
            }

            return bancoProduto.Include(a => a.Imagens).ToPagedList<Produto>(NumeroPagina, RegistroPorPagina);
        }
    }
}
