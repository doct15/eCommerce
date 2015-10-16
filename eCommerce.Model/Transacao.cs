using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model
{
    public class Transacao
    {
        public int ID { get; set; }
        public Carrinho Carrinho { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }
        public Cupom Cupom { get; set; }
    }
}
