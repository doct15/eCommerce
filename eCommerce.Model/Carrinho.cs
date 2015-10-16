using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model
{
    public class Carrinho
    {
        public int ID { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Status { get; set; }
        public Cupom Cupom { get; set; }
        public List<ItemEstoque> Produtos { get; set; }


        public decimal ValorTotal()
        {
            decimal valorTotal = 0;

            foreach (ItemEstoque item in Produtos)
            {
                valorTotal += item.Produto.Preco;
            }

            return valorTotal;
        }
    }
}
