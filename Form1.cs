using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace _00.__AMEO
{
    public partial class Form1 : Form
    {
        List<Alumnos> LA;
        public Form1()
        {
            InitializeComponent();
            LA = new List<Alumnos>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Mostrar();
            dataGridView1.MultiSelect = false;
        }

        private void Mostrar()
        {
            LA.Clear();
            if (File.Exists("Datos.txt"))
            {
                string s;
                using (StreamReader sr = File.OpenText("Datos.txt"))
                {
                    s = sr.ReadToEnd();
                }
                string[] registro = s.Split(new char[] { '@', '\r', '\n'},StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var x in registro)
                {
                    var datos = x.Split(',');
                    LA.Add(new Alumnos { legajo = datos[0], nombre = datos[1], apellido = datos[2], DNI = datos[3] });
                }
                dataGridView1.DataSource = null; dataGridView1.DataSource = LA;
            }
            else { dataGridView1.DataSource = null; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string l = Interaction.InputBox("Legajo: ");
                string n = Interaction.InputBox("Nombre: ");
                string a = Interaction.InputBox("Apellido: ");
                string d = Interaction.InputBox("DNI: ");

                using (StreamWriter sr = File.AppendText("Datos.txt"))
                {
                    sr.WriteLine($"{l},{n},{a},{d}@");
                }
                Mostrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("Datos.txt"))
                {
                    var eli = (dataGridView1.SelectedRows[0].DataBoundItem as Alumnos).legajo;
                    string s;
                    using (StreamReader sr = File.OpenText("Datos.txt"))
                    {
                        while ((s=sr.ReadLine())!= null && s != "")
                        {
                            string[] registro = s.Split(new char[] { '@', '\r', '\n' },StringSplitOptions.RemoveEmptyEntries);

                            foreach (var x in registro)
                            {
                                var datos = x.Split(',');
                                if (datos[0]!= eli)
                                {
                                    using (StreamWriter sw = File.AppendText("Aux.txt"))
                                    {
                                        sw.WriteLine($"{datos[0]},{datos[1]},{datos[2]},{datos[3]}@");
                                    }
                                }
                            }
                        }
                    }
                    File.Delete("Datos.txt");
                    if (File.Exists("Aux.txt"))
                    {
                        File.Move("Aux.txt", "Datos.txt");
                        File.Delete("Aux.txt");
                    }
                    Mostrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("Datos.txt"))
                {
                    var mod = (dataGridView1.SelectedRows[0].DataBoundItem as Alumnos).legajo;
                    string s;
                    using (StreamReader sr = File.OpenText("Datos.txt"))
                    {
                        while ((s=sr.ReadLine())!=null&&s!="")
                        {
                            string[] registro = s.Split(new char[] { '@', '\r', '\n' },StringSplitOptions.RemoveEmptyEntries);
                            foreach (var x in registro)
                            {
                                var datos = x.Split(',');
                                if (mod == datos[0])
                                {
                                    using (StreamWriter sw = File.AppendText("Aux.txt"))
                                    {
                                        string l = Interaction.InputBox("Nuevo legajo: ", "Modificando...", mod);
                                        string n = Interaction.InputBox("Nuevo nombre: ", "Modificando...", datos[1]);
                                        string a = Interaction.InputBox("Nuevo apellido: ", "Modificando...", datos[2]);
                                        string d = Interaction.InputBox("Nuevo DNI: ", "Modificando...", datos[3]);
                                        sw.WriteLine($"{l},{n},{a},{d}@");

                                    }
                                }
                                else
                                {
                                    using (StreamWriter sw = File.AppendText("Aux.txt"))
                                    {
                                        sw.WriteLine($"{datos[0]},{datos[1]},{datos[2]},{datos[3]}@");
                                    }
                                }
                            }
                        }
                    }
                    File.Delete("Datos.txt");
                    if (File.Exists("Aux.txt"))
                    {
                        File.Move("Aux.txt", "Datos.txt");
                        File.Delete("Aux.txt");
                    }
                    Mostrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("Datos.txt"))
                {
                    string s;
                    using (StreamReader sr = File.OpenText("Datos.txt"))
                    {
                        s = sr.ReadToEnd();
                    }
                    string[] registro = s.Split(new char[] { '@', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    int mayor;
                    for (int i = 0; i < registro.Length - 1; i++)
                    {
                        mayor = 0;
                        for (int j = 0; j <= registro.Length - 1 - i; j++)
                        {
                            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 3)
                            {
                                if (int.Parse(registro[radioButton1.Checked ? mayor : j].Split(',')[comboBox1.SelectedIndex]) <
                                    int.Parse(registro[!radioButton1.Checked ? mayor : j].Split(',')[comboBox1.SelectedIndex]))
                                {
                                    mayor = j;
                                }
                            }
                            else
                            {
                                if (radioButton1.Checked)
                                {
                                    if (string.Compare(registro[mayor].Split(',')[comboBox1.SelectedIndex], registro[j].Split(',')[comboBox1.SelectedIndex]) < 0)
                                    {
                                        mayor = j;
                                    }
                                }
                                else
                                {
                                    if (string.Compare(registro[mayor].Split(',')[comboBox1.SelectedIndex], registro[j].Split(',')[comboBox1.SelectedIndex]) > 0)
                                    {
                                        mayor = j;
                                    }
                                }
                            }
                        }
                        var aux = registro[registro.Length - 1 - i];
                        registro[registro.Length - 1 - i] = registro[mayor];
                        registro[mayor] = aux;

                        using (StreamWriter sw = File.CreateText("Datos.txt"))
                        {
                            foreach (var x in registro)
                            {
                                var a = x.Split(',');
                                sw.WriteLine($"{a[0]},{a[1]},{a[2]},{a[3]}@");
                            }
                        }
                        Mostrar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MostrarEnListboxREC(ListBox pListBox, int largo, string[] registro)
        {
            if (largo != 0)
            {
                pListBox.Items.Add(registro[largo-1]);
                MostrarEnListboxREC(pListBox, largo-1, registro);
            }
        }
    
        

        private void MostrarEnListbox(ListBox pListBox)
        {
            string s;
            using (StreamReader sr = File.OpenText("Datos.txt"))
            {
                s = sr.ReadToEnd();
            }
            string[] registro = s.Split('@');
            Array.Resize(ref registro, registro.Length - 1);
            int largo = registro.Length;
            MostrarEnListboxREC(pListBox, largo, registro);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            MostrarEnListbox(listBox1);
        }
    }

    public class Alumnos
    {
        public string legajo { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string DNI { get; set; }
    }
}
