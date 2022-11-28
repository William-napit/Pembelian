using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Pertemuan_9
{
    public partial class frmbrwProduct : Form
    {
        public frmbrwProduct()
        {
            InitializeComponent();
        }
        frmPurchasing brwProduct;
        public frmbrwProduct(frmPurchasing brwProduct)
        {
            InitializeComponent();
            this.brwProduct = brwProduct;
        }

        SqlConnection con;
        string constr;
        SqlDataAdapter da;
        SqlCommand cmd;
        string querry;
        DataSet ds;
        DataRow dr;
        DataColumn[] dc = new DataColumn[1];

        private void Koneksi()
        {
            try
            {
                constr = "Data Source = LocalHost; Initial Catalog = Latihan2_3; Integrated Security = true";
                con = new SqlConnection(constr);
                con.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmbrwProduct_Load(object sender, EventArgs e)
        {
            Koneksi();
            LoadData();
            TampilData();
        }
        private void LoadData()
        {
            ds = new DataSet();
            querry = "Select * from Product";
            cmd = new SqlCommand(querry, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Product");
            dc[0] = ds.Tables["Product"].Columns[0];
            ds.Tables["Product"].PrimaryKey = dc;
        }
        private void TampilData()
        {
            dgvProduct.DataSource = ds.Tables["Product"];
            dgvProduct.Columns[0].HeaderText = "Product ID";
            dgvProduct.Columns[1].HeaderText = "Product Name";
            dgvProduct.Columns[2].HeaderText = "Stock";
            dgvProduct.AllowUserToAddRows = false;
            dgvProduct.ReadOnly = true;
            lblJumlah.Text = dgvProduct.RowCount.ToString();

        }

        private void lblJumlah_Click(object sender, EventArgs e)
        {

        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = dgvProduct.CurrentCell.RowIndex;
            brwProduct.txtProductID.Text = dgvProduct[0, baris].Value.ToString();
            brwProduct.lblProdutID.Text = dgvProduct[1, baris].Value.ToString();

            this.Close();
        }
    }
}
