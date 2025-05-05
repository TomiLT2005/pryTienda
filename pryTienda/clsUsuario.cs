using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pryTienda
{
    public class clsUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }

        public clsUsuario(string Nombre, string Contraseña)
        {
            this.Nombre = Nombre;
            this.Contraseña = Contraseña;
        }

    }
}
