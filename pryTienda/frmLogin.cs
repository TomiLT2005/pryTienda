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
    public partial class frmLogin: MaterialForm
    {

        clsConexionBD conexion = new clsConexionBD();


        //Variable para guardar el número de intentos
        int intentos = 3;


        //Inicializa el Formulario con tema oscuro y colores personalizados
        public frmLogin()
        {
            InitializeComponent();

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
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


        //Evento de carga del Formulario
        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtContraseña.PasswordChar = '*';
            btnOcultar.Image = Properties.Resources.imgOjoCerrado;
        }



        //Evento Controles Principales (Ingresar y Cancelar)
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
            }
            else
            {
                clsUsuario usuario = new clsUsuario(txtUsuario.Text, txtContraseña.Text);

                bool resultado = conexion.verificarUsuario(usuario);

                if (resultado)
                {
                    frmInicio ventana = new frmInicio();
                    this.Hide();
                    ventana.ShowDialog();
                }
                else
                {
                    intentos = intentos - 1;
                    MessageBox.Show("Datos incorrectos. Intentos restantes: " + intentos);

                    if (intentos == 0)
                    {
                        MessageBox.Show("Has alcanzado el límite de intentos. Contacta con el administrador.", "Error de acceso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnIngresar.Enabled = false;
                    }
                }
            }
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        //Evento Ocultar y Mostrar Contraseña
        private void btnOcultar_Click(object sender, EventArgs e)
        {
            if (txtContraseña.PasswordChar == '*')
            {
                txtContraseña.PasswordChar = '\0';
                btnOcultar.Image = Properties.Resources.imgOjoAbierto;
            }
            else
            {
                txtContraseña.PasswordChar = '*';
                btnOcultar.Image = Properties.Resources.imgOjoCerrado;
            }
        }

        
    }
}
