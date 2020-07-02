using AlugaOffice.Models.TodosProdutos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Libraries.AutoMapper
{
    public class MapeamentoProfile : Profile
    {
        public MapeamentoProfile()
        {
            CreateMap<Produto, ProdutoItem>();
        }


    }
}
