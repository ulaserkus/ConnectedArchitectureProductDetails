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

namespace ConnectedArchitecture
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-L0NA0DU\SQLEXPRESS;Initial Catalog=NORTHWND;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }
         
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select * from Categories",sqlConnection);
            sqlConnection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            comboBox1.DisplayMember = "CategoryName";
            comboBox1.ValueMember = "CategoryID";
            List<Category> categories = new List<Category>();

            while (dataReader.Read())
            {
                Category category = new Category();
                category.CategoryID =(int)dataReader["CategoryID"];
                category.CategoryName = (string)dataReader["CategoryName"];

                categories.Add(category);
            }
            sqlConnection.Close();
            comboBox1.DataSource = categories;
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            var selectedID = comboBox1.SelectedValue;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand("select * from Products where CategoryID="+selectedID.ToString(),sqlConnection);
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Button button = new Button();
                button.Name = "b"+dataReader["ProductID"];
                button.Height = 50;
                button.AutoSize = true;
                button.Text = dataReader["ProductName"].ToString();
                flowLayoutPanel1.Controls.Add(button);
                button.Click += Button_Click;

            }

            sqlConnection.Close();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedBtn = (Button)sender;
            var ID = clickedBtn.Name.Remove(0, 1);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select * from Products where ProductID="+ID,sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            ProductDetails form = new ProductDetails();
            form.lbl_product.Text = sqlDataReader["ProductName"].ToString();
            form.lbl_qpu.Text = sqlDataReader["QuantityPerUnit"].ToString();
            form.Show();
            sqlConnection.Close();
           
        }
    }
}
