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
    public partial class frmPurchasing : Form
    {
        public frmPurchasing()
        {
            InitializeComponent();
        }       

        frmLogin login;
        public frmPurchasing(frmLogin login)
        {
            InitializeComponent();
            this.login = login;
        }
        SqlConnection con;
        string constr;
        SqlDataAdapter da;
        SqlCommand cmd;
        string querry;
        DataSet ds;
        DataRow dr;
        DataColumn[] dc1 = new DataColumn[1];
        DataColumn[] dc2 = new DataColumn[2];
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
        private void frmPurchasing_Load(object sender, EventArgs e)
        {
            Koneksi();
            lblUser.Text = login.txtUserId.Text;
            lblLoginTime.Text = DateTime.Now.ToString("hh:mm:ss");
            
        }

        private void btnPurchaseID_Click(object sender, EventArgs e)
        {
            frmbrwPurchasing brwpurchasing = new frmbrwPurchasing(this);
            brwpurchasing.Show();
        }

        private void btnProductID_Click(object sender, EventArgs e)
        {
            frmbrwProduct brwProduct = new frmbrwProduct(this);
            brwProduct.Show();
        }

        private void LoadDataPurchase()
        {
            ds = new DataSet();
            querry = "Select* from Purchasing";
            cmd = new SqlCommand(querry, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Purchasing");
            dc2[0] = ds.Tables["Purchasing"].Columns[0];
            dc2[1] = ds.Tables["Purchasing"].Columns[2];
            ds.Tables["Purchasing"].PrimaryKey = dc2;
        }
        private void LoadDataProduct()
        {
            ds = new DataSet();
            querry = "select*from Product";
            cmd = new SqlCommand(querry, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Product");
            dc1[0] = ds.Tables["Product"].Columns[0];
            ds.Tables["Product"].PrimaryKey = dc1;
        }

        private void UpdateDataPurchasing()
        {
            cb = new SqlCommandBuilder(da);
            da = cb.DataAdapter;
            da.Update(ds.Tables["Purchasing"]);
        }
        private void UpdateDataProduct()
        {
            cb = new SqlCommandBuilder(da);
            da = cb.DataAdapter;
            da.Update(ds.Tables["Product"]);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string[] cari = { txtPurchaseID.Text, txtProductID.Text };
            LoadDataPurchase();
            dr = ds.Tables["Purchasing"].Rows.Find(cari);
            if (dr == null)
            {
                dr = ds.Tables["Purchasing"].NewRow();
                dr[0] = txtPurchaseID.Text;
                dr[1] = dtpPurchaseDate.Value;
                dr[2] = txtProductID.Text;
                dr[3] = nudPurchaseUnit.Value;
                ds.Tables["Purchasing"].Rows.Add(dr);
                UpdateDataPurchasing();

                LoadDataProduct();
                dr = ds.Tables["Product"].Rows.Find(txtProductID.Text);
                if (dr !=null)
                {
                    dr[2] = decimal.Parse(dr[2].ToString()) + nudPurchaseUnit.Value;
                    UpdateDataProduct();
                }
                MessageBox.Show($"Data dengan Purchase ID {txtPurchaseID.Text} dan Product ID {txtProductID.Text} berhasil disimpan !", "Tambah Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Kosong();
            }
            else
            {
                MessageBox.Show($"Data dengan Purchase ID {txtPurchaseID.Text} dan Product ID {txtProductID.Text} sudah ada di database !", "Tambah Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void Kosong()
        {
            txtProductID.Clear();
            txtPurchaseID.Clear();
            dtpPurchaseDate.Value = DateTime.Today;
            nudPurchaseUnit.Value = 0;
            txtPurchaseID.Focus();
            lblProdutID.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string[] cari = { txtPurchaseID.Text, txtProductID.Text };
            {
                LoadDataPurchase();
                dr = ds.Tables["Purchasing"].Rows.Find(cari);
                if (dr !=null)
                {
                    decimal beliLama = decimal.Parse(dr[3].ToString());

                    dr[1] = dtpPurchaseDate.Value;
                    dr[3] = nudPurchaseUnit.Value;
                    UpdateDataPurchasing();

                    LoadDataProduct();
                    dr = ds.Tables["Product"].Rows.Find(txtProductID.Text);
                    if (dr !=null)
                    {
                        dr[2] = decimal.Parse(dr[2].ToString()) - beliLama + nudPurchaseUnit.Value;
                        UpdateDataProduct();
                    }
                    MessageBox.Show($"Data dengan purchse ID  {txtPurchaseID.Text} dan Product ID {txtProductID.Text} berhasil disimpan di database !", "Update Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Kosong();
                }
                else
                {
                    MessageBox.Show($"Data dengan purchse ID  {txtPurchaseID.Text} dan Product ID {txtProductID.Text} tidak ada di database !", "Update Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string[] cari = { txtPurchaseID.Text, txtProductID.Text };
            LoadDataPurchase();
            dr = ds.Tables["Purchasing"].Rows.Find(cari);
            if (dr != null)
            {
                decimal beiLama = decimal.Parse(dr[3].ToString());
                dr.Delete();
                UpdateDataPurchasing();

                LoadDataProduct();
                dr = ds.Tables["Product"].Rows.Find(txtProductID.Text);
                if(dr !=null)
                {
                    dr[2] = decimal.Parse(dr[2].ToString())- beiLama;
                    UpdateDataProduct();
                   
                }
                MessageBox.Show($"Data dengan purchase ID  {txtPurchaseID.Text} dan Product ID {txtProductID.Text} Berhasil dihapus dari database !", "Delete Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Kosong();
            } 
            else
            {
                MessageBox.Show($"Data dengan purchse ID  {txtPurchaseID.Text} dan Product ID {txtProductID.Text} Tidak ada di database !", "Delete Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Kosong();
        }
    }
}
