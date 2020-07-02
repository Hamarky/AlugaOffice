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
    public class CategoriaRepository : ICategoriaRepository
    {
        private IConfiguration _config;
        const int _registroPorPagina = 10;
        private AlugaOfficeContext _banco;
        public CategoriaRepository(AlugaOfficeContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _config = configuration;
        }

        public void Atualizar(Categoria categoria)
        {
            _banco.Update(categoria);
            _banco.SaveChanges();
        }

        public void Cadastrar(Categoria categoria)
        {
            _banco.Add(categoria);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Categoria categoria = ObterCategoria(Id);
            _banco.Remove(categoria);
            _banco.SaveChanges();
        }

        public Categoria ObterCategoria(int Id)
        {
            return _banco.Categorias.Find(Id);
        }
        public Categoria ObterCategoria(string Slug)
        {
            return _banco.Categorias.Where(a=>a.Slug == Slug).FirstOrDefault();
        }

        public IPagedList<Categoria> ObterTodasCategorias(int? pagina)
        {
            int RegistroPorPagina = _config.GetValue<int>("RegistroPorPagina");
            int NumeroPagina = pagina ?? 1;
            return _banco.Categorias.Include(a => a.CategoriaPai).ToPagedList<Categoria>(NumeroPagina, RegistroPorPagina);
        }

        public IEnumerable<Categoria> ObterTodasCategorias()
        {
            return _banco.Categorias;
        }
        private List<Categoria> Categorias;
        private List<Categoria> ListaCategoriaRecursiva = new List<Categoria>();
        public IEnumerable<Categoria> ObterCategoriasRecursivas(Categoria categoriaPai)
        {
            if (Categorias == null)
            {
                Categorias = ObterTodasCategorias().ToList();
            }
            if (!ListaCategoriaRecursiva.Exists(a => a.Id == categoriaPai.Id))
            {
                ListaCategoriaRecursiva.Add(categoriaPai);
            }

            var ListaCategoriaFilho = Categorias.Where(a => a.CategoriaPaiId == categoriaPai.Id);
            if (ListaCategoriaFilho.Count() > 0)
            {
                ListaCategoriaRecursiva.AddRange(ListaCategoriaFilho.ToList());
                foreach (var categoria in ListaCategoriaFilho)
                {
                    ObterCategoriasRecursivas(categoria);
                }
            }

            return ListaCategoriaRecursiva;
        }
    }
}
