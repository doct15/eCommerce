using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model
{
    public class ItemEstoque
    {
        public int ID { get; set; }
        public Produto Produto { get; set; }
        public string Status { get; set; }
    }
}
