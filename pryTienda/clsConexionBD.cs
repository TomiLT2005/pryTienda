using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryTienda
{
    internal class clsConexionBD
    {
        //cadena de conexion
        string cadena = "Server = localhost\\SQLEXPRESS;Database=Comercio;Trusted_Connection=True;";

        //"Server=localhost;Database=Comercio;Trusted_Connection=True;";//


        //conector
        SqlConnection conexion;

        //comando
        SqlCommand comando;

        public string nombreBaseDeDatos;


        public void ConectarBD()
        {
            try
            {
                conexion = new SqlConnection(cadena);

                nombreBaseDeDatos = conexion.Database;

                conexion.Open();
            }
            catch (Exception error)
            {
                MessageBox.Show("Ups... algo salió mal al intentar conectar con la base de datos. Por favor, revise su conexión e intente nuevamente.", "Conexión fallida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        public void ListarBD(DataGridView Grilla)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    string query = "SELECT * FROM Productos";

                    SqlCommand comando = new SqlCommand(query, conexion);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);

                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    Grilla.DataSource = tabla;
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudieron cargar los productos correctamente. Revise su conexión o intente más tarde.", "Error de carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void Cargarcategorias(ComboBox Combo)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    string query = "SELECT Id, Nombre FROM Categorias";

                    SqlCommand comando = new SqlCommand(query, conexion);
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);

                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);

                    Combo.DataSource = tabla;
                    Combo.DisplayMember = "Nombre";
                    Combo.ValueMember = "Id";
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error al cargar categorías: " + error.Message);
            }
        }

        public void Agregar(clsProducto producto)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    string query = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaId) VALUES (@nombre, @descripcion, @precio, @stock, @categoriaId)";

                    SqlCommand comando = new SqlCommand(query, conexion);

                    comando.Parameters.AddWithValue("@nombre", producto.Nombre);
                    comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    comando.Parameters.AddWithValue("@precio", producto.Precio);
                    comando.Parameters.AddWithValue("@stock", producto.Stock);
                    comando.Parameters.AddWithValue("@categoriaId", producto.CategoriaId);

                    comando.ExecuteNonQuery();

                    MessageBox.Show("Producto agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error al agregar producto: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void Modificar(clsProducto producto)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    string query = "UPDATE Productos SET Nombre = @nombre, Descripcion = @descripcion, Precio = @precio, Stock = @stock, CategoriaId = @categoriaId WHERE Codigo = @codigo";

                    SqlCommand comando = new SqlCommand(query, conexion);

                    comando.Parameters.AddWithValue("@nombre", producto.Nombre);
                    comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    comando.Parameters.AddWithValue("@precio", producto.Precio);
                    comando.Parameters.AddWithValue("@stock", producto.Stock);
                    comando.Parameters.AddWithValue("@categoriaId", producto.CategoriaId);
                    comando.Parameters.AddWithValue("@codigo", producto.Codigo);

                    comando.ExecuteNonQuery();

                    MessageBox.Show("Producto modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al modificar producto: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Eliminar(int codigo)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    string query = "DELETE FROM Productos WHERE Codigo = @codigo";

                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@codigo", codigo);

                    comando.ExecuteNonQuery();
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error al eliminar el producto: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarporNombre(DataGridView Grilla, string nombreProducto)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    string query = "SELECT * FROM Productos WHERE Nombre LIKE @nombre";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@nombre", "%" + nombreProducto + "%");


                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);

                    if (tabla.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontró el producto", "Resultado de búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Grilla.DataSource = tabla;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al buscar productos: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        public bool verificarUsuario(clsUsuario usuario)
        {
            bool loginExitoso = false;

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    string query = "SELECT COUNT(*) FROM Usuarios WHERE Nombre = @Nombre AND Contraseña = @Contraseña";

                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);

                    int match = (int)comando.ExecuteScalar();

                    if (match > 0)
                    {
                        loginExitoso = true;
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos. Intente nuevamente.", "Error de acceso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al verificar el Usuario: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return loginExitoso;
        }
    }
}
