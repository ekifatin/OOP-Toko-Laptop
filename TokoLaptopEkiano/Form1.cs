using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TokoLaptopEkiano
{
    public partial class Form1 : Form
    {
        private MySqlCommand cmd;
        private MySqlDataReader rd;

        Koneksi Konn = new Koneksi();
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = Konn.GetConn();
            conn.Open();
            cmd = new MySqlCommand("Select * from login where username='" + textBox1.Text + "' and password='" + textBox2.Text + "'", conn);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                Dashboard menu_utama = new Dashboard();
                menu_utama.Show();
            }
            else
            {
                MessageBox.Show("Maaf Username atau Password anda salah");
            }
        }
    }
}
