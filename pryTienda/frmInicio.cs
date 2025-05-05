using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace pryTienda
{
    public partial class frmInicio: MaterialForm
    {

        clsConexionBD conexion = new clsConexionBD();


        //Variable para guardar el codigo seleccionado//
        public int codigoSeleccionado = 0;


        public frmInicio()
        {
            InitializeComponent();

            MaterialSkinManager materialSkinManager =  MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            materialSkinManager.ColorScheme = new ColorScheme(
            Primary.Red700,   // Rojo fuerte para los elementos principales
            Primary.Red900,   // Rojo más oscuro para los elementos secundarios
            Primary.Grey100,  // Gris claro para los contenedores
            Accent.Red700,    // Rojo oscuro para los acentos
            TextShade.WHITE   // Texto oscuro para buen contraste
            );
        }


        private void frmInicio_Load(object sender, EventArgs e)
        {
            conexion.ConectarBD();
            conexion.ListarBD(dgvDatos);
            conexion.Cargarcategorias(cmbCategoria);
            EstilizarDataGridView(dgvDatos);
        }


        private void tabCtrlMenu_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPageSalir)
            {
                DialogResult res = MessageBox.Show("¿Estás seguro de que deseas salir?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }


        //Eventos de Botones Primarios (Agregar, Modificar y Eliminar)//

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                string Nombre = txtNombre.Text;
                string Descripcion = txtDesc.Text;
                decimal Precio = Convert.ToDecimal(txtPrecio.Text);
                int Stock = Convert.ToInt32(txtStock.Text);
                int CategoriaId = Convert.ToInt32(cmbCategoria.SelectedValue);


                clsProducto nuevoproducto = new clsProducto(0, Nombre, Descripcion, Precio, Stock, CategoriaId);

                conexion.Agregar(nuevoproducto);
                conexion.ListarBD(dgvDatos);

                LimpiarCampos(txtNombre, txtDesc, txtPrecio, txtStock, cmbCategoria);
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                clsProducto modificado = new clsProducto(
                codigoSeleccionado,
                txtNombre.Text,
                txtDesc.Text,
                Convert.ToDecimal(txtPrecio.Text),
                Convert.ToInt32(txtStock.Text),
                Convert.ToInt32(cmbCategoria.SelectedValue));

                conexion.Modificar(modificado);
                conexion.ListarBD(dgvDatos);

                LimpiarCampos(txtNombre, txtDesc, txtPrecio, txtStock, cmbCategoria);
                codigoSeleccionado = 0;
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                conexion.Eliminar(codigoSeleccionado);
                conexion.ListarBD(dgvDatos);

                LimpiarCampos(txtNombre, txtDesc, txtPrecio, txtStock, cmbCategoria);
                codigoSeleccionado = 0;
            }
        }



        //Eventos de Botones Secundarios (Buscar, Volver y Cancelar)//

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBuscar.Text;
            conexion.BuscarporNombre(dgvDatos, busqueda);

            txtBuscar.Clear();
        }



        private void btnVolver_Click(object sender, EventArgs e)
        {
            conexion.ListarBD(dgvDatos);
            LimpiarCampos(txtNombre, txtDesc, txtPrecio, txtStock, cmbCategoria);
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos(txtNombre, txtDesc, txtPrecio, txtStock, cmbCategoria);

            txtNombre.Focus();
            conexion.ListarBD(dgvDatos);
            btnAgregar.Enabled = true;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
        }
















        //Eventos - Control Ingreso de Datos//
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras");
            }
        }

        private void txtDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras");
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten números.");
            }
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten números.");
            }
        }



        //Metodo Limpiar Campos//   
        private void LimpiarCampos(MaterialTextBox2 txtNombre, MaterialTextBox2 txtDesc, MaterialTextBox2 txtPrecio, MaterialTextBox2 txtStock, MaterialComboBox cmbCategoria)
        {
            txtNombre.Text = "";
            txtDesc.Text = "";
            cmbCategoria.SelectedIndex = -1;
            txtPrecio.Text = "";
            txtStock.Text = "";
        }



        //Metodo Validar Campos//
        private bool validarCampos()
        {
            epValidacion.Clear();

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                epValidacion.SetError(txtNombre, "El Producto necesita un Nombre");
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDesc.Text))
            {
                epValidacion.SetError(txtDesc, "El Producto necesita una Descripción");
                txtDesc.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                epValidacion.SetError(txtPrecio, "El Producto necesita un Precio");
                txtPrecio.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStock.Text) || Convert.ToInt32(txtStock.Text) <= 0)
            {
                epValidacion.SetError(txtStock, "Debe haber al menos un Producto en stock");
                txtStock.Focus();
                return false;
            }

            if (cmbCategoria.SelectedIndex == -1)
            {
                epValidacion.SetError(cmbCategoria, "El Producto necesita una Categoría");
                cmbCategoria.Focus();
                return false;
            }

            return true; //esta todo correcto//
        }



        //Estilo al DataGridView///
        private void EstilizarDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(183, 28, 28); // Rojo oscuro
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(244, 67, 54); // Rojo más claro al seleccionar
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            dgv.RowHeadersVisible = false;
        }

      
    }
}
