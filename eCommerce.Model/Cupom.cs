using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model
{
    public class Cupom
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public float PercentualDisconto { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
    }
}
