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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
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
                constr = "Data Source = localhost; Initial Catalog = Latihan2_3; Integrated Security = true";
                con = new SqlConnection(constr);
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            Koneksi();
            txtPassword.PasswordChar = '*';
        }
        private void LoadData()
        {
            ds = new DataSet();
            querry = "Select* From frmLogin";
            cmd = new SqlCommand(querry, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Login");
            dc[0] = ds.Tables["Login"].Columns[1];
            ds.Tables["Login"].PrimaryKey = dc;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoadData();
            dr = ds.Tables["Login"].Rows.Find(txtPassword.Text);
            if (dr!=null)
            {
                if(txtPassword.Text == dr[1].ToString())
                {
                    frmPurchasing purchasing = new frmPurchasing (this);
                    purchasing.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Password yang diinput salah !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            else
            {
                MessageBox.Show("User ID yang diinput salah !", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShow.Checked == true)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }

        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
