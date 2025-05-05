using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pryTienda
{
    public class clsProducto
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }

        public clsProducto(int Codigo, string Nombre, string Descripcion, decimal Precio, int Stock, int CategoriaId)
        {
            this.Codigo = Codigo;
            this.Nombre = Nombre;
            this.Descripcion = Descripcion;
            this.Precio = Precio;
            this.Stock = Stock;
            this.CategoriaId = CategoriaId;
        }
    }
}
