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

namespace csproj
{
    public partial class resquestpublish : Form
    {
        string byid;
        public resquestpublish()
        {
            InitializeComponent();
            
        }
        public resquestpublish(string byidx)
        {
            InitializeComponent();
            byid = byidx;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string title = textBox1.Text.Trim();
            string duty = textBox2.Text.Trim();
            string demand = textBox3.Text.Trim();
            string reqnum = textBox4.Text.Trim();
            title = Convert.ToBase64String(Encoding.UTF8.GetBytes(title));
            duty = Convert.ToBase64String(Encoding.UTF8.GetBytes(duty));
            demand = Convert.ToBase64String(Encoding.UTF8.GetBytes(demand));
            String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";

            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {

                conn.Open();
                string insert = "INSERT INTO request(`title`,`duty`,`demand`,`reqnum`,`byid`) VALUES('" + title + "', '" + duty + "', '" + demand + "',"+ reqnum +",'"+ byid +"')";
                MySqlCommand comm = new MySqlCommand(insert, conn);
                if (comm.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("发布成功");
                    this.Close();
                }


            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }


            //textBox2.Text = title;
            //title = Encoding.Default.GetString(Convert.FromBase64String(title));
            //textBox3.Text = title;
        }
    }
}
