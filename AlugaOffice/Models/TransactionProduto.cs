using AlugaOffice.Models.TodosProdutos;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Models
{
    public class TransactionProduto
    {
        public TransacaoPagarMe Transaction { get; set; }
        public List<ProdutoItem> Produtos { get; set; }

    }
}
