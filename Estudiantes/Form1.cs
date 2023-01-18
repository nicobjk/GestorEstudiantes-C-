using Logica;
using Logica.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estudiantes
{
    public partial class Form1 : Form
    {
        private LEstudiantes estudiante ;
        //private Librarys librarys;
        public Form1()
        {
            InitializeComponent();
            //librarys = new Librarys();
            var listTextBox = new List<TextBox>();
            listTextBox.Add(textBoxNid);
            listTextBox.Add(textBoxNombre);
            listTextBox.Add(textBoxApellido);
            listTextBox.Add(textBoxEmail);
            var listLabel = new List<Label>();
            listLabel.Add(labelNid);
            listLabel.Add(labelNombre);
            listLabel.Add(labelApellido);
            listLabel.Add(labelEmail);
            listLabel.Add(labelPaginas);
            Object[] objectos = {
                pictureBoxImage,
                Properties.Resources.logo_google,
                dataGridView1,
                numericUpDown1
            };

            estudiante = new LEstudiantes(listTextBox, listLabel, objectos);
        }

        private void PictureBoxImage_Click(object sender, EventArgs e)
        {
            estudiante.uploadimage.CargarImagen(pictureBoxImage);
        }

        private void TextBoxNid_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNid.Text.Equals(""))
            {
                labelNid.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelNid.ForeColor = Color.Green;
                labelNid.Text = "Nid";
            }
        }

        private void TextBoxNid_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.numberKeyPress(e);
        }

        private void TextBoxNombre_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNombre.Text.Equals(""))
            {
                labelNombre.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelNombre.ForeColor = Color.Green;
                labelNombre.Text = "Nombre";
            }
        }

        private void TextBoxNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.textKeyPress(e);
        }
        private void TextBoxApellido_TextChanged(object sender, EventArgs e)
        {
            if (textBoxApellido.Text.Equals(""))
            {
                labelApellido.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelApellido.ForeColor = Color.Green;
                labelApellido.Text = "Apellido";
            }
        }

        private void TextBoxApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            estudiante.textBoxEvent.textKeyPress(e);
        }

        private void TextBoxEmail_TextChanged(object sender, EventArgs e)
        {
            if (textBoxEmail.Text.Equals(""))
            {
                labelEmail.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelEmail.ForeColor = Color.Green;
                labelEmail.Text = "Email";
            }
        }
        private void ButtonAgregar_Click(object sender, EventArgs e)
        {
            estudiante.Registrar();
        }

        private void textBoxBuscar_TextChanged(object sender, EventArgs e)
        {
            estudiante.SearchEstudiante(textBoxBuscar.Text);
        }

        private void buttonPrimero_Click(object sender, EventArgs e)
        {
            estudiante.Paginador("Primero");
        }

        private void buttonAnterior_Click(object sender, EventArgs e)
        {
            estudiante.Paginador("Anterior");
        }

        private void buttonSiguiente_Click(object sender, EventArgs e)
        {
            estudiante.Paginador("Siguiente");
        }

        private void buttonUltima_Click(object sender, EventArgs e)
        {
            estudiante.Paginador("Ultimo");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            estudiante.Registro_Paginas();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
                estudiante.GetEstudiante();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
                estudiante.GetEstudiante();
        }

        private void buttonCancelkar_Click(object sender, EventArgs e)
        {
            estudiante.Restablecer();
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            estudiante.Eliminar();
        }
    }
}
