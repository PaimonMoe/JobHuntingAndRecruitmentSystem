using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csproj
{
    public partial class resume : Form
    {
        int id;
        bool editable,exists;
        public resume()
        {
            InitializeComponent();
        }
        public resume(int idx,bool editablex)
        {
            InitializeComponent();
            id = idx;
            editable = editablex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (exists)//存在
            {
                String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";

                MySqlConnection conn2 = new MySqlConnection(connetStr);
                conn2.Open();
                string sql = "UPDATE `resume` SET `content` = '"+ Convert.ToBase64String(Encoding.UTF8.GetBytes(textBox1.Text))+"' WHERE `userid` = " + id ;
                MySqlCommand comm = new MySqlCommand(sql, conn2);
                comm.ExecuteNonQuery();
                conn2.Close();
                MessageBox.Show("已保存！");
            }
            else
            {
                String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";

                MySqlConnection conn2 = new MySqlConnection(connetStr);
                conn2.Open();
                string sql = "INSERT INTO `resume` VALUES("+id+",'" + Convert.ToBase64String(Encoding.UTF8.GetBytes(textBox1.Text)) + "')";
                MySqlCommand comm = new MySqlCommand(sql, conn2);
                comm.ExecuteNonQuery();
                conn2.Close();
                MessageBox.Show("已保存！");
            }
        }

        private void resume_Load(object sender, EventArgs e)
        {
            String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();

            string sql = "SELECT * from resume where userid="+id;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                textBox1.Text = Encoding.Default.GetString(Convert.FromBase64String(reader.GetString("content")));
                exists = true;
            }
            else
            {
                textBox1.PlaceholderText = "暂无简历数据";
                exists = false;
            }
            conn.Close();
            if (!editable)
            {
                textBox1.Enabled=false;
                button1.Visible=false;
            }
        }
    }
}
