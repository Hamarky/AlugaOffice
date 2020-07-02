using AlugaOffice.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Libraries.Component
{
    public class MenuViewComponent : ViewComponent
    {
        private ICategoriaRepository _categoriaRepository;
        public MenuViewComponent(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var ListaCategoria = _categoriaRepository.ObterTodasCategorias().ToList();
            return View(ListaCategoria);
        }




    }
}
