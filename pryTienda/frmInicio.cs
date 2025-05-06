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


        //Variable para guardar el código seleccionado
        public int codigoSeleccionado = 0;



        //Inicializa el formulario con tema oscuro y colores personalizados
        public frmInicio(string nombreUsuario)
        {
            InitializeComponent();

            lblUsuario.Text = "Hola " + nombreUsuario + "!";

            MaterialSkinManager materialSkinManager =  MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

            //Esquema de colores personalizado para el formulario
            materialSkinManager.ColorScheme = new ColorScheme(
            Primary.Red700,   // Rojo fuerte para los elementos principales
            Primary.Red900,   // Rojo más oscuro para los elementos secundarios
            Primary.Grey100,  // Gris claro para los contenedores
            Accent.Red700,    // Rojo oscuro para los acentos
            TextShade.WHITE   // Texto oscuro para buen contraste
            );
        }



        //Evento de carga del formulario
        private void frmInicio_Load(object sender, EventArgs e)
        {
            conexion.ConectarBD();
            conexion.ListarBD(dgvDatos);
            conexion.Cargarcategorias(cmbCategoria);
            EstiloDataGridView(dgvDatos);
        }



        //Evento para salir de la Aplicación
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



        //Método para obtener el valor de la fila seleccionada
        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs f)
        {
            if (f.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvDatos.Rows[f.RowIndex];

                codigoSeleccionado = Convert.ToInt32(fila.Cells["Codigo"].Value);

                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtDesc.Text = fila.Cells["Descripcion"].Value.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
                txtStock.Text = fila.Cells["Stock"].Value.ToString();
                cmbCategoria.SelectedValue = fila.Cells["CategoriaId"].Value;

                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
            }
        }



        //Controles (Ingreso de Datos, Validación y Limpiar)
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

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
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


        private void LimpiarCampos(MaterialTextBox2 txtNombre, MaterialTextBox2 txtDesc, MaterialTextBox2 txtPrecio, MaterialTextBox2 txtStock, MaterialComboBox cmbCategoria)
        {
            txtNombre.Text = "";
            txtDesc.Text = "";
            cmbCategoria.SelectedIndex = -1;
            txtPrecio.Text = "";
            txtStock.Text = "";
        }



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



        //Estilo al DataGridView
        private void EstiloDataGridView(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false; // Evitar el estilo predeterminado de encabezados de columna

            //Fondo general
            dgv.BackgroundColor = Color.FromArgb(18, 18, 18); 
            dgv.BorderStyle = BorderStyle.None;

            //Estilo de cabecera
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 26, 26);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("YaHei", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //Bordes suaves en la cabecera
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Estilo de celdas
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(33, 33, 33); 
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("YaHei", 10);                      //Tipografía
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(66, 66, 66);   //Selección
            dgv.DefaultCellStyle.SelectionForeColor = Color.Salmon;                 //Texto para selección

            //Bordes
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(50, 50, 50); 

            //Personalizar filas
            dgv.RowsDefaultCellStyle.BackColor = Color.FromArgb(24, 24, 24);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(36, 36, 36); 

            //Eliminamos bordes en las filas y cabecera
            dgv.RowHeadersVisible = false;

            //Desactivar botones de agregados, eliminación de filas y solamente Leer
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;

            //Selección de filas
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 
            dgv.MultiSelect = false; 
        }
    }
}
