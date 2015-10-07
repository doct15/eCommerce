using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model
{
    public class Produto
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string CaminhoImagem { get; set; }
        public decimal Preco { get; set; }
        public Categoria Categoria { get; set; }
    }
}
