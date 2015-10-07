using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Model
{
    public class UsuarioSistema
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Senha { get; set; }
        public DateTime? UltimoLogin { get; set; }
    }
}
