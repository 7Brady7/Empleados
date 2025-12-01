using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using MySql.Data.MySqlClient;

using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;




namespace Empleados
{
    public partial class Empleados : Form
    {
        string conexion = "server=localhost; database=empleadosdb; user=root; password=root;";

        public Empleados()
        {
            InitializeComponent();

            CargarEmpleados();
        }

        private void CargarEmpleados()
        {
            using (MySqlConnection cn = new MySqlConnection(conexion))
            {
                cn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM empleados", cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)  // AGREGAR
        {
            string nombre = textBox2.Text;

            if (!decimal.TryParse(textBox3.Text, out decimal sueldo))
            {
                MessageBox.Show("Debe ingresar un número válido en Sueldo.");
                return;
            }

            string departamento = textBox4.Text;

            decimal ars = sueldo * 0.0304m;
            decimal afp = sueldo * 0.0287m;
            decimal neto = sueldo - ars - afp;
            decimal total = sueldo + ars + afp;

            using (MySqlConnection cn = new MySqlConnection(conexion))
            {
                cn.Open();

                string sql = @"INSERT INTO empleados 
                       (nombre, sueldoBruto, departamento, ars, afp, sueldoNeto, totalPagado)
                       VALUES (@nombre, @sueldo, @departamento, @ars, @afp, @neto, @total)";

                MySqlCommand cmd = new MySqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@sueldo", sueldo);
                cmd.Parameters.AddWithValue("@departamento", departamento);
                cmd.Parameters.AddWithValue("@ars", ars);
                cmd.Parameters.AddWithValue("@afp", afp);
                cmd.Parameters.AddWithValue("@neto", neto);
                cmd.Parameters.AddWithValue("@total", total);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Empleado agregado!");
            CargarEmpleados();
        }


        private void button2_Click(object sender, EventArgs e)  // ELIMINAR
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una fila para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

            using (MySqlConnection cn = new MySqlConnection(conexion))
            {
                cn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM empleados WHERE ID=@id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Empleado eliminado");
            CargarEmpleados();
        }

        private void button4_Click(object sender, EventArgs e)  // EDITAR
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una fila para editar.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

            string nombre = textBox2.Text;
            if (!decimal.TryParse(textBox3.Text, out decimal sueldo))
            {
                MessageBox.Show("Debe ingresar un número válido en Sueldo.");
                return;
            }
            string departamento = textBox4.Text;

            decimal ars = sueldo * 0.0304m;
            decimal afp = sueldo * 0.0287m;
            decimal sueldoNeto = sueldo - ars - afp;
            decimal totalPagado = sueldo + ars + afp;

            using (MySqlConnection cn = new MySqlConnection(conexion))
            {
                cn.Open();
                string sql = @"UPDATE empleados SET 
                        Nombre=@nombre,
                        SueldoBruto=@sueldo,
                        Departamento=@departamento,
                        ARS=@ars,
                        AFP=@afp,
                        SueldoNeto=@neto,
                        TotalPagado=@total
                       WHERE ID=@id";

                MySqlCommand cmd = new MySqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@sueldo", sueldo);
                cmd.Parameters.AddWithValue("@departamento", departamento);
                cmd.Parameters.AddWithValue("@ars", ars);
                cmd.Parameters.AddWithValue("@afp", afp);
                cmd.Parameters.AddWithValue("@neto", sueldoNeto);
                cmd.Parameters.AddWithValue("@total", totalPagado);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Empleado actualizado");
            CargarEmpleados();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["SueldoBruto"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Departamento"].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e) // CARGAR DATOS
        {
            if (dataGridView1.CurrentRow != null)
            {
                ID.Text = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells["SueldoBruto"].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells["Departamento"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Seleccione una fila.");
            }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void button5_Click_1(object sender, EventArgs e)
        {
            ID.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            ID.Focus();
        }
    }
}

