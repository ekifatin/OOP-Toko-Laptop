using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace TokoLaptopEkiano
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        MySqlConnection connection = new MySqlConnection("datasource = localhost;port=3307;user=root;password='';database=dblaptop");

        public void resetIncrement()
        {
            MySqlScript script = new MySqlScript(connection, "SET @id := 0; UPDATE data_laptop SET id = @id := (@id+1); " +
                "ALTER TABLE data_laptop AUTO_INCREMENT = 1;");
            script.Execute();
        }
        public void resetData()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            tanggal.Value = DateTime.Now;
            textBox8.Text = "";
            pictureBox1.Image = null;
        }
        public void Tampil(string valueToSearch)
        {
            MySqlCommand command = new MySqlCommand("select * from data_laptop where concat(id,nama, brand, stock, tglmasuk, harga) like '%" + valueToSearch + "%'", connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.RowTemplate.Height = 120;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = table;
            DataGridViewImageColumn imgcol = new DataGridViewImageColumn();
            imgcol = (DataGridViewImageColumn)dataGridView1.Columns[6];
            imgcol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Nama Barang";
            dataGridView1.Columns[2].HeaderText = "Brand Barang";
            dataGridView1.Columns[3].HeaderText = "Stock Barang";
            dataGridView1.Columns[4].HeaderText = "Harga Barang";
            dataGridView1.Columns[5].HeaderText = "Tanggal Masuk";
            dataGridView1.Columns[6].HeaderText = "Foto";
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

            string date1 = tanggal.Value.ToString("yyyy-MM-dd");
            MySqlCommand command = new MySqlCommand("INSERT INTO data_laptop(nama, brand, stock, harga,tglmasuk, foto) VALUES (@nama,@brand,@stock,@harga,@tglmasuk,@foto)", connection);
            command.Parameters.Add("@nama", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@brand", MySqlDbType.VarChar).Value = textBox2.Text;
            command.Parameters.Add("@stock", MySqlDbType.VarChar).Value = textBox3.Text;
            command.Parameters.Add("@harga", MySqlDbType.VarChar).Value = textBox4.Text;
            command.Parameters.Add("@tglmasuk", MySqlDbType.Date).Value= date1;
            command.Parameters.Add("@foto", MySqlDbType.Blob).Value = img;

            ExecMyQuery(command, "Data Berhasil Ditambahkan");
            resetIncrement();
            resetData();
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            resetIncrement();
            Tampil("");
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            Byte[] img = (Byte[])dataGridView1.CurrentRow.Cells[6].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox1.Image = Image.FromStream(ms);

            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            string dateString = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            DateTime dateValue;
            if (DateTime.TryParse(dateString, out dateValue))
            {
                tanggal.Value = dateValue;
            }
            textBox8.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM data_laptop WHERE id=@id", connection);
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox8.Text;

            ExecMyQuery(command, "Data Berhasil Dihapus");
            resetIncrement();
            resetData();
            Tampil("");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            byte[] img = ms.ToArray();
            string date1 = tanggal.Value.ToString("yyyy-MM-dd");
            MySqlCommand command = new MySqlCommand("UPDATE data_laptop SET nama=@nama,brand=@brand,stock=@stock,harga=@harga,tglmasuk=@tglmasuk,foto=@foto WHERE id=@id", connection);
            command.Parameters.Add("@nama", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@brand", MySqlDbType.VarChar).Value = textBox2.Text;
            command.Parameters.Add("@stock", MySqlDbType.VarChar).Value = textBox3.Text;
            command.Parameters.Add("@harga", MySqlDbType.VarChar).Value = textBox4.Text;
            command.Parameters.Add("@tglmasuk", MySqlDbType.Date).Value = date1;
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox8.Text;
            command.Parameters.Add("@foto", MySqlDbType.Blob).Value = img;

            ExecMyQuery(command, "Data Berhasil Diubah");
            resetData();
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Tampil(textBox7.Text);
        }
    }
}