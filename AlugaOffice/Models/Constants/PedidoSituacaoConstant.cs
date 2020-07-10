using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlugaOffice.Models.Constants
{
    public class PedidoSituacaoConstant
    {
        public const string AGUARDANDO_PAGAMENTO = "AGUARDANDO PAGAMENTO";
        public const string PAGAMENTO_APROVADO = "PAGAMENTO APROVADO";
        public const string PAGAMENTO_REJEITADO = "PAGAMENTO REJEITADO";
        public const string NF_EMITIDA = "NF EMITIDA";
        public const string EM_TRANSPORTE = "EM TRANSPORTE";
        public const string ENTREGUE = "ENTREGUE";
        public const string FINALIZADO = "FINALIZADO";
        public const string EM_CANCELAMENTO = "EM CANCELAMENTO";
        public const string EM_ANALISE = "EM ANALISE";
        public const string CANCELAMENTO_ACEITO = "CANCELAMENTO ACEITO";
        public const string CANCELAMENTO_REJEITADO = "CANCELAMENTO REJEITADO";
        public const string ESTORNO = "ESTORNO";

        public static string GetNames(string code)
        {
            foreach (var field in typeof(TipoFreteConstant).GetFields())
            {
                if ((string)field.GetValue(null) == code)
                    return field.Name.ToString();
            }
            return "";
        }
    }
}
