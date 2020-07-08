using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Models
{
    public class CartaoCredito
    {
        public string NumeroCartao { get; set; }
        public string NomeNoCartao { get; set; }
        public string Vecimento { get; set; }
        public string CodigoSeguranca { get; set; }
    }
}
