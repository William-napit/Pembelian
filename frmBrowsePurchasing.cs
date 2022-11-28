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
    public partial class frmbrwPurchasing : Form
    {
        public frmbrwPurchasing()
        {
            InitializeComponent();
        }

        frmPurchasing purchasing;
        public frmbrwPurchasing(frmPurchasing purchasing)
        {
            InitializeComponent();
            this.purchasing = purchasing;
        }

        SqlConnection con;
        string constr;
        SqlDataAdapter da;
        SqlCommand cmd;
        string querry;
        DataSet ds;
        DataRow dr;
        DataColumn[] dc = new DataColumn[2];
        SqlCommandBuilder cb;

        private void Koneksi()
        {
            try
            {
                constr = "Data Source = Localhost; Initial Catalog = Latihan2_3; Integrated Security = true";
                con = new SqlConnection(constr);
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadData()
        {
            ds = new DataSet();
            querry = "SELECT Pur.Purchase_ID, Pur.Purchase_Date, Pur.Product_ID, Pro.Product_Name, pur.Qty FROM Purchasing Pur INNER JOIN Product Pro ON Pur.Product_Id = Pro.Product_ID";
            cmd = new SqlCommand(querry, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "vwPurchasing");
            dc[0] = ds.Tables["vwPurchasing"].Columns[0];
            dc[1] = ds.Tables["vwPurchasing"].Columns[2];
            ds.Tables["vwPurchasing"].PrimaryKey = dc;
            
        }
        private void Tampil()
        {
            dgvPurchasing.DataSource = ds.Tables["vwPurchasing"];
            dgvPurchasing.Columns[0].HeaderText = "Purchase ID";
            dgvPurchasing.Columns[1].HeaderText = "Purchase Date";
            dgvPurchasing.Columns[2].HeaderText = "Product ID";
            dgvPurchasing.Columns[3].HeaderText = "Product Name";
            dgvPurchasing.Columns[4].HeaderText = "Purchase Unit";
            dgvPurchasing.AllowUserToAddRows = false;
            dgvPurchasing.ReadOnly = true;
            lblJumlah.Text = dgvPurchasing.RowCount.ToString();
            
        }
       
        

        private void frmbrwPurchasing_Load(object sender, EventArgs e)
        {
            Koneksi();
            LoadData();
            Tampil();
        }

        private void dgvPurchasing_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = dgvPurchasing.CurrentCell.RowIndex;
            purchasing.txtPurchaseID.Text = dgvPurchasing[0, baris].Value.ToString();
            purchasing.dtpPurchaseDate.Value =DateTime.Parse(dgvPurchasing[1, baris].Value.ToString());
            purchasing.txtProductID.Text = dgvPurchasing[2, baris].Value.ToString();
            purchasing.lblProdutID.Text = dgvPurchasing[3, baris].Value.ToString();
            purchasing.nudPurchaseUnit.Value =decimal.Parse(dgvPurchasing[4, baris].Value.ToString());
            

            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ds = new DataSet();
            querry = $"SELECT Pur.Purchase_ID, Pur.Purchase_Date, Pur.Product_ID, Pro.Product_Name, pur.Purchase_Unit FROM Purchasing Pur INNER JOIN Product Pro ON Pur.ProductId = Pro.Product_ID WHERE Pur.Purchase_ID LIKE '%{txtSearch.Text}%'";
            cmd = new SqlCommand(querry, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "vwPurchasing");
            dc[0] = ds.Tables["vwPurchasing"].Columns[0];
            dc[1] = ds.Tables["vwPurchasing"].Columns[2];
            ds.Tables["vwPurchasing"].PrimaryKey = dc;
        }

        private void dgvPurchasing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
