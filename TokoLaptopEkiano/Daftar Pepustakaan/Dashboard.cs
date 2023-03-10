using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace Daftar_Pepustakaan
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=;database=dft_perpustakaan");

        public void Tampil(string valueToSearch)
        {
            MySqlCommand command = new MySqlCommand("select * from data_buku where concat(id,nama, buku, jenis, pinjam, kembali, kelas) like '%" + valueToSearch + "%'", connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.RowTemplate.Height = 60;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = table;
            DataGridViewImageColumn imgcol = new DataGridViewImageColumn();
            imgcol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            imgcol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Nama Peminjam";
            dataGridView1.Columns[2].HeaderText = "Nama Buku";
            dataGridView1.Columns[3].HeaderText = "Jenis Buku";
            dataGridView1.Columns[4].HeaderText = "Waktu Peminjaman";
            dataGridView1.Columns[5].HeaderText = "Waktu Peminjaman";
            dataGridView1.Columns[6].HeaderText = "Kelas";
            dataGridView1.Columns[7].HeaderText = "Foto";
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public void ExecMyQuery(MySqlCommand mcomd, string myMsg)
        {
            connection.Open();
            if (mcomd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show(myMsg);
            }
            else
            {
                MessageBox.Show("Error");
            }
            connection.Close();
            Tampil("");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image(*.JPG;*.PNG;*.JPEG;*.GIF;)|*.jpg;*.png;*.jpeg;*.gif;";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(opf.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            byte[] img = ms.ToArray();
            MySqlCommand command = new MySqlCommand("INSERT INTO data_buku(nama, buku, jenis, pinjam, kembali, kelas, foto) VALUES (@nama,@buku,@jenis,@pinjam,@kembali,@kelas,@foto)", connection);
            command.Parameters.Add("@nama", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@buku", MySqlDbType.VarChar).Value = textBox2.Text;
            command.Parameters.Add("@jenis", MySqlDbType.VarChar).Value = textBox3.Text;
            command.Parameters.Add("@pinjam", MySqlDbType.VarChar).Value = textBox4.Text;
            command.Parameters.Add("@kembali", MySqlDbType.VarChar).Value = textBox5.Text;
            command.Parameters.Add("@kelas", MySqlDbType.VarChar).Value = textBox6.Text;
            command.Parameters.Add("@foto", MySqlDbType.Blob).Value = img;

            ExecMyQuery(command, "Data Berhasil Ditambahkan");
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            Tampil("");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            Byte[] img = (Byte[])dataGridView1.CurrentRow.Cells[7].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox1.Image = Image.FromStream(ms);

            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM data_buku WHERE id=@id", connection);
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox8.Text;

            ExecMyQuery(command, "Data Berhasil Dihapus");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            byte[] img = ms.ToArray();
            MySqlCommand command = new MySqlCommand("UPDATE data_buku SET nama=@nama,buku=@buku,jenis=@jenis,pinjam=@pinjam,kembali=@kembali,kelas=@kelas,foto=@foto WHERE id=@id", connection);
            command.Parameters.Add("@nama", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@buku", MySqlDbType.VarChar).Value = textBox2.Text;
            command.Parameters.Add("@jenis", MySqlDbType.VarChar).Value = textBox3.Text;
            command.Parameters.Add("@pinjam", MySqlDbType.VarChar).Value = textBox4.Text;
            command.Parameters.Add("@kembali", MySqlDbType.VarChar).Value = textBox5.Text;
            command.Parameters.Add("@kelas", MySqlDbType.VarChar).Value = textBox6.Text;
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox8.Text;
            command.Parameters.Add("@foto", MySqlDbType.Blob).Value = img;

            ExecMyQuery(command, "Data Berhasil Diubah");
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Tampil(textBox7.Text);
        }
    }
}
