using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;

namespace Portfolio
{
    public partial class Form1 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-74J1BFM\MSSQLSERVER01;initial Catalog=PersonDB;Integrated Security=
        True;");
        int personID = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtMobile_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvPerson_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PersonID", personID);
                param.Add("@Name", txtName.Text.Trim());
                param.Add("@Mobile", txtMobile.Text.Trim());
                param.Add("@Technologies", txtTechnologies.Text.Trim());

                sqlCon.Execute("PersonAddOrEdit", param, commandType: CommandType.StoredProcedure);
                if (personID == 0)
                    MessageBox.Show(" Save Successfully");
                else
                    MessageBox.Show(" Updated Successfully");
                FillDataGridView();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { 
                sqlCon.Close();
            }
        }

        void FillDataGridView()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SearchText", txtSearch.Text.Trim());

            List<Person> list = sqlCon.Query<Person>("PersonViewAllOrSearch", param, commandType: CommandType.StoredProcedure)
                .ToList<Person>();
            dgvPerson.DataSource = list;
            dgvPerson.Columns[0].Visible = false;
        }
        class Person
        {
            public int PersonID { get; set; }
            public string Name { get; set; }
            public string Mobile { get; set; }
            public string Technologies { get; set; }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
                Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bnt_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        void Clear()
        {
            txtName.Text = txtMobile.Text = txtTechnologies.Text = " ";
            personID = 0;
            btnSave.Text = " Save";
            btnDelete.Enabled = false;


        }

        private void dgvPerson_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if(dgvPerson.CurrentRow.Index !=1)
                {
                    personID =Convert.ToInt32 (dgvPerson.CurrentRow.Cells[0].Value.ToString());
                    txtName.Text = dgvPerson.CurrentRow.Cells[1].Value.ToString();
                    txtMobile.Text = dgvPerson.CurrentRow.Cells[2].Value.ToString();
                    txtTechnologies.Text = dgvPerson.CurrentRow.Cells[3].Value.ToString();
                    btnDelete.Enabled = true;
                    btnSave.Text = "Edit";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@PersonID",personID);
                sqlCon.Execute("PersonDeleteByID", param, commandType: CommandType.StoredProcedure);
                Clear();
                FillDataGridView();
                MessageBox.Show("Deleted Successfully");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
