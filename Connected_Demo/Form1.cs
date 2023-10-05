using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data;

namespace Connected_Demo
{
    public partial class Form1 : Form
    {

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        private object DataTable;

        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Department> list = new List<Department>();
                string qry = "select * from Department";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        Department Dept = new Department();
                        Dept.D_id = Convert.ToInt32(reader["D_id"]);
                        Dept.D_Name = reader["D_Name"].ToString();
                        list.Add(Dept);
                    }
                }

                //Display Dname and on selection of dname we need did
                cmbDid.DataSource= list;
                cmbDid.DisplayMember = "D_Name";
                cmbDid.ValueMember = "D_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            { 
                con.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = " insert into Employee values(@E_Name,@Email,@Age,@Salary,@D_id)";
                cmd = new SqlCommand(qry, con);

                //Assign values to each parameter
                cmd.Parameters.AddWithValue("@E_name", txtname.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Age", Convert.ToInt32(txtage.Text));
                cmd.Parameters.AddWithValue("@Salary", Convert.ToInt32(txtSalary.Text));
                cmd.Parameters.AddWithValue("@D_id", Convert.ToInt32(cmbDid.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                    ClearFeilds();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry1 = "select e.*,d.D_Name from Employee e inner join Department d on d.D_id=e.D_id where e.E_Id = @E_Id";
                cmd = new SqlCommand(qry1, con);
                cmd.Parameters.AddWithValue("@E_Id",Convert.ToInt32(txtid.Text));
                con.Open();
                reader=cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    if(reader.Read())
                    {
                        txtname.Text = reader["E_Name"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtage.Text = reader["Age"].ToString();
                        txtSalary.Text = reader["Salary"].ToString();
                        cmbDid.Text = reader["D_Name"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                    ClearFeilds();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { 
                con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "update Employee set E_Name=@E_Name,Email=@Email,Age=@Age,Salary=@Salary,D_id=@D_id where E_Id=@E_Id";
                cmd = new SqlCommand(qry, con);

                //Assign values to each parameter
                cmd.Parameters.AddWithValue("@E_name", txtname.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Age", Convert.ToInt32(txtage.Text));
                cmd.Parameters.AddWithValue("@Salary", Convert.ToInt32(txtSalary.Text));
                cmd.Parameters.AddWithValue("@D_id", Convert.ToInt32(cmbDid.SelectedValue));
                cmd.Parameters.AddWithValue("@E_Id", Convert.ToInt32(txtid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record updated");
                    ClearFeilds();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                GetAllEmps();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "Delete from Employee where E_Id=@E_Id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@E_Id", Convert.ToInt32(txtid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record deleted");
                    ClearFeilds();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                GetAllEmps();
            }
        }


        private void GetAllEmps()
        {
            string qry = "Select e.*, d.D_Name from Employee e inner join Department d on e.D_id = d.D_id";
            cmd = new SqlCommand(qry, con);
            con.Open();
            reader=cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource= table;
            con.Close();
        }

        private void btnShowall_Click(object sender, EventArgs e)
        {
            GetAllEmps();
        }

        private void ClearFeilds()
        {
            txtid.Clear();
            txtname.Clear();
            txtEmail.Clear();
            txtage.Clear();
            txtSalary.Clear();
            cmbDid.Refresh();
        }
    }
}
