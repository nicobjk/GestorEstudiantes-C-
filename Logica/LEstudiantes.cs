using Data;
using LinqToDB;
using Logica.Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logica
{
    public class LEstudiantes : Librarys
    {
        private List<TextBox> listTextBox;
        private List<Label> listLabel;
        private PictureBox image;
        private Bitmap _imagBitmap;
        private DataGridView _dataGridView;
        private NumericUpDown _numericUpDown;
        private Paginador<Estudiante> _paginador;
        private string _accion = "insert";
        //private Librarys librarys;
        public LEstudiantes(List<TextBox> listTextBox, List<Label> listLabel, object[] objectos)
        {
            this.listTextBox = listTextBox;
            this.listLabel = listLabel;
            //librarys = new Librarys();
            image = (PictureBox)objectos[0];
            _imagBitmap = (Bitmap)objectos[1];
            _dataGridView = (DataGridView)objectos[2];
            _numericUpDown = (NumericUpDown)objectos[3];
            Restablecer();
        }
        public void Registrar()
        {
            if (listTextBox[0].Text.Equals(""))
            {
                listLabel[0].Text = "El campo es requerido";
                listLabel[0].ForeColor = Color.Red;
                listTextBox[0].Focus();
            }
            else
            {
                if (listTextBox[1].Text.Equals(""))
                {
                    listLabel[1].Text = "El campo es requerido";
                    listLabel[1].ForeColor = Color.Red;
                    listTextBox[1].Focus();
                }
                else
                {
                    if (listTextBox[2].Text.Equals(""))
                    {
                        listLabel[2].Text = "El campo es requerido";
                        listLabel[2].ForeColor = Color.Red;
                        listTextBox[2].Focus();
                    }
                    else
                    {
                        if (listTextBox[3].Text.Equals(""))
                        {
                            listLabel[3].Text = "El campo es requerido";
                            listLabel[3].ForeColor = Color.Red;
                            listTextBox[3].Focus();
                        }
                        else
                        {
                            if (textBoxEvent.comprobarFormatoEmail(listTextBox[3].Text))
                            {
                                var user = _Estudiante.Where(u => u.email.Equals(listTextBox[3].Text)).ToList();
                                if(user.Count.Equals(0)) 
                                {
                                    Save();
                                }
                                else
                                {
                                    if (user[0].id.Equals(_idEstudiante))
                                    {
                                        Save();
                                    }
                                    else
                                    {
                                        listLabel[3].Text = "Email ya registrado";
                                        listLabel[3].ForeColor = Color.Red;
                                        listTextBox[3].Focus();
                                    }
                                }
                            }
                            else
                            {
                                listLabel[3].Text = "Email no valido";
                                listLabel[3].ForeColor = Color.Red;
                                listTextBox[3].Focus();
                            }
                        }
                    }
                }
            }
        }
        private void Save()
        {
            BeginTransactionAsync();
            try
            {
                var imageArray = uploadimage.ImageToByte(image.Image);
                //using (var db = new Conexion())
                //{
                //    db.Insert(new Estudiante()
                //    {
                //        nid = listTextBox[0].Text,
                //        nombre = listTextBox[1].Text,
                //        apellido = listTextBox[2].Text,
                //        email = listTextBox[3].Text,
                //    });
                //}
                switch (_accion)
                {
                    case "insert":
                     _Estudiante.Value(e => e.nid, listTextBox[0].Text)
                        .Value(e => e.nombre, listTextBox[1].Text)
                        .Value(e => e.apellido, listTextBox[2].Text)
                        .Value(e => e.email, listTextBox[3].Text)
                        .Value(e => e.image, imageArray)
                        .Insert();
                        break;
                    case "update":
                        _Estudiante.Where(u => u.id.Equals(_idEstudiante))
                            .Set(e => e.nid, listTextBox[0].Text)
                            .Set(e => e.nombre, listTextBox[1].Text)
                            .Set(e => e.apellido, listTextBox[2].Text)
                            .Set(e => e.email, listTextBox[3].Text)
                            .Set(e => e.image, imageArray)
                            .Update();
                        break;
                }
                
                CommitTransaction();
                Restablecer();
            }
            catch (Exception)
            {
                RollbackTransaction();
            }
        }
        private int _reg_por_pagina = 2, _num_pagina = 1;
        public void SearchEstudiante(string campo)
        {
            List<Estudiante> query = new List<Estudiante>();
            int inicio = (_num_pagina - 1) * _reg_por_pagina;  
            if (campo.Equals(""))
            {
                query = _Estudiante.ToList();
            }
            else
            {
                query = _Estudiante.Where(c => c.nid.StartsWith(campo) || c.nombre.StartsWith(campo)
                        || c.apellido.StartsWith(campo)).ToList();
            }
            if (query.Count > 0)
            {
                _dataGridView.DataSource = query.Select(c => new
                {
                    c.id,
                    c.nid,
                    c.nombre,
                    c.apellido,
                    c.email,
                    c.image
                }).Skip(inicio).Take(_reg_por_pagina).ToList();
                _dataGridView.Columns[0].Visible = false;
                _dataGridView.Columns[5].Visible = false;
                _dataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                _dataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
            else
            {
                _dataGridView.DataSource = query.Select(c => new
                {
                    c.nid,
                    c.nombre,
                    c.apellido,
                    c.email,
                }).ToList();
            }
        }
        private int _idEstudiante = 0;
        public void GetEstudiante()
        {
            _accion = "update";
            _idEstudiante = Convert.ToInt16(_dataGridView.CurrentRow.Cells[0].Value);
            listTextBox[0].Text = Convert.ToString(_dataGridView.CurrentRow.Cells[1].Value);
            listTextBox[1].Text = Convert.ToString(_dataGridView.CurrentRow.Cells[2].Value);
            listTextBox[2].Text = Convert.ToString(_dataGridView.CurrentRow.Cells[3].Value);
            listTextBox[3].Text = Convert.ToString(_dataGridView.CurrentRow.Cells[4].Value);
            try
            {
                byte[] arrayImage = (byte[])_dataGridView.CurrentRow.Cells[5].Value;
                image.Image = uploadimage.byteArrayToImage(arrayImage);
            }
            catch (Exception)
            {
                image.Image = _imagBitmap;
            }
        }

        private List<Estudiante> listEstudiante;
        public void Paginador(string metodo)
        {
            switch (metodo)
            {
                case "Primero":
                    _num_pagina = _paginador.primero();
                    break;
                case "Anterior":
                    _num_pagina = _paginador.anterior();
                    break;
                case "Siguiente":
                    _num_pagina = _paginador.siguiente();
                    break;
                case "Ultimo":
                    _num_pagina = _paginador.ultimo();
                    break;
            }
            SearchEstudiante("");
        }
        public void Registro_Paginas()
        {
            _num_pagina = 1;
            _reg_por_pagina = (int)_numericUpDown.Value;
            var list = _Estudiante.ToList();
            if (0 < list.Count)
            {
                _paginador = new Paginador<Estudiante>(listEstudiante, listLabel[4], _reg_por_pagina);
                SearchEstudiante("");
            }
        }
        public void Eliminar()
        {
            if (_idEstudiante.Equals(0))
            {
                MessageBox.Show("Seleccione un Estudiante");
            }
            else
            {
                if (MessageBox.Show("¿Estás seguro de eliminar al estudiante?", "Eliminar estudiante",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _Estudiante.Where(c => c.id.Equals(_idEstudiante)).Delete();
                    Restablecer();
                }
            }
        }
        public void Restablecer()
        {
            _accion = "insert";
            _num_pagina = 1;
            _idEstudiante = 0;
            image.Image = _imagBitmap;
            listLabel[0].Text = "Nid";
            listLabel[1].Text = "Nombre";
            listLabel[2].Text = "Apellido";
            listLabel[3].Text = "Email";
            listLabel[0].ForeColor = Color.LightSlateGray;
            listLabel[1].ForeColor = Color.LightSlateGray;
            listLabel[2].ForeColor = Color.LightSlateGray;
            listLabel[3].ForeColor = Color.LightSlateGray;
            listTextBox[0].Text = "";
            listTextBox[1].Text = "";
            listTextBox[2].Text = "";
            listTextBox[3].Text = "";
            listEstudiante = _Estudiante.ToList();
            if (0 < listEstudiante.Count)
            {
                _paginador = new Paginador<Estudiante>(listEstudiante, listLabel[4], _reg_por_pagina);
            }
            SearchEstudiante("");
        }
    }
}
