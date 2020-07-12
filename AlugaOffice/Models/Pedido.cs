using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int? ClienteId { get; set; }
        public string TransactionId { get; set; }

        
        public string FreteEmpresa { get; set; }
        public string FreteCodRastreamento { get; set; }

        public string FormaPagamento { get; set; }
        public decimal ValorTotal { get; set; }
        public string DadosTransaction { get; set; }
        public string DadosProdutos { get; set; }

        public DateTime DataRegistro { get; set; }
        public string Situacao { get; set; }

        public string NFE { get; set; }

        public virtual Cliente Cliente { get; set; }

        [ForeignKey("PedidoId")]
        public virtual ICollection<PedidoSituacao> PedidoSituacoes { get; set; }

    }
}
