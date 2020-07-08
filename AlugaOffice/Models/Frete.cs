using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Models
{
    public class Frete
    {
        public int CEP { get; set; }
        public string CodCarrinho { get; set; }
        public List<ValorPrazoFrete> ListaValores { get; set; }

    }
}
