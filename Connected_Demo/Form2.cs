using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data;
using System.Drawing;


namespace Connected_Demo
{

    
    public partial class Form2 : Form
    { 
        SqlConnection con;
        SqlDataAdapter da;
        SqlCommandBuilder builder;
        DataSet ds;
    
        public Form2()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                //Write a query
                string qry = " select * from Department";

                //Assign query to Adapter --> will fire the query
                da = new SqlDataAdapter(qry, con);

                //create object of DataSet
                ds = new DataSet();

                //FIll() will fire the select query and load data in the ds
                //Dept is a name given to the table in DataSet
                da.Fill(ds, "Department");
                cmbDid.DataSource = ds.Tables["Department"];
                cmbDid.DisplayMember = "D_Name";
                cmbDid.ValueMember = "D_id";
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private DataSet GetEmployees()
        {
            string qry = "Select * from Employee";

            //Assign the query
            da= new SqlDataAdapter(qry, con);

            //When app load the in Dataset, we need to manage the PK also
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            //SCB will track the Dataset and updatequeries to the DataAdapter
            builder = new SqlCommandBuilder(da);
            ds=new DataSet();
            da.Fill(ds, "Employee");//this name givrn to the dataset table
            return ds;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();

                // create new row to add recrod
                DataRow row = ds.Tables["Employee"].NewRow();
                // assign value to the row
                row["E_Name"] = txtname.Text;
                row["Email"] = txtEmail.Text;
                row["Age"] = txtage.Text;
                row["Salary"] = txtSalary.Text;
                row["D_id"] = cmbDid.SelectedValue;
                // attach this row in DataSet table
                ds.Tables["Employee"].Rows.Add(row);
                // update the changes from DataSet to DB
                int result = da.Update(ds.Tables["Employee"]);
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                }
            }

            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
                // find the row
                DataRow row = ds.Tables["Employee"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    row["E_Name"] = txtname.Text;
                    row["Email"] = txtEmail.Text;
                    row["Age"] = txtage.Text;
                    row["Salary"] = txtSalary.Text;
                    row["D_id"] = cmbDid.SelectedValue;
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Employee"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record updated");
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
                // find the row
                DataRow row = ds.Tables["Employee"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    // delete the current row from DataSet table
                    row.Delete();
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Employee"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record deleted");
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnShowall_Click(object sender, EventArgs e)
        {
            try
            {
               ds = GetEmployees();
                dataGridView1.DataSource = ds.Tables["Employee"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select e.*,d.D_Name from Employee e inner join Department d on d.D_id=e.D_id";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "Employee");

                //find method can only search the data if pf is applied in the Dataset table
                DataRow row = ds.Tables["Employee"].Rows.Find(txtid.Text);
                if (row != null)
                {
                    txtname.Text = row["E_Name"].ToString();
                    txtEmail.Text = row["Email"].ToString();
                    txtage.Text = row["Age"].ToString();
                    txtSalary.Text = row["Salary"].ToString();
                    cmbDid.Text = row["D_Name"].ToString();
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtid.Text = dataGridView1.CurrentRow.Cells["E_Id"].Value.ToString();
            txtname.Text = dataGridView1.CurrentRow.Cells["E_Name"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["Email"].Value.ToString();
            txtage.Text = dataGridView1.CurrentRow.Cells["Age"].Value.ToString();
            txtSalary.Text = dataGridView1.CurrentRow.Cells["Salary"].Value.ToString();
            cmbDid.Text = dataGridView1.CurrentRow.Cells["D_id"].Value.ToString() ;

        }
    }
}
