using AlugaOffice.Models.TodosProdutos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace AlugaOffice.Models.ViewModels.Components
{
    public class ProdutoListagemViewModel
    {

        public IPagedList<Produto> lista { get; set; }
        public List<SelectListItem> ordenacao
        {
            get
            {
                return new List<SelectListItem>() {
                    new SelectListItem("Alfabética", "A"),
                    new SelectListItem("Maior preço", "MAP"),
                    new SelectListItem("Menor preço", "MEP")
                };
            }
            private set { }
        }
    }
}

