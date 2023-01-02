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
    public partial class requestview : Form
    {
        string id,uid;
        public requestview()
        {
            InitializeComponent();
            
        }
        public requestview(string idx,string uidx)
        {
            InitializeComponent();
            id = idx;
            uid = uidx;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Close();
            
        }

        private void requestview_Load(object sender, EventArgs e)
        {
            String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();

            string sql = "SELECT * from request where id =" + id;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            textBox1.Text = Encoding.Default.GetString(Convert.FromBase64String(reader.GetString("title")));
            textBox2.Text = Encoding.Default.GetString(Convert.FromBase64String(reader.GetString("duty")));
            textBox3.Text = Encoding.Default.GetString(Convert.FromBase64String(reader.GetString("demand")));
            textBox4.Text = reader.GetString("reqnum");
            reader.Close();
            string sql1 = "SELECT * from comment where requestid =" + id;
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            MySqlDataReader reader1 = cmd1.ExecuteReader();

            int commentindex = 0;
            MySqlConnection conn11 = new MySqlConnection(connetStr);
            conn11.Open();
            while (reader1.Read())
            {
                
                string sql11 = "SELECT * from user where id=" + reader1.GetString("userid");
                MySqlCommand cmd11 = new MySqlCommand(sql11, conn11);
                MySqlDataReader reader11 = cmd11.ExecuteReader();
                reader11.Read();

                treeView1.Nodes.Add(Encoding.Default.GetString(Convert.FromBase64String(reader11.GetString("name"))) + "于"+reader1.GetString("time")+"留言：");
                treeView1.Nodes[commentindex].Nodes.Add(Encoding.Default.GetString(Convert.FromBase64String(reader1.GetString("content"))));
                commentindex++;
                reader11.Close();
                
            }
            conn11.Close();

            treeView1.ExpandAll();
            conn.Close();
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string content = Convert.ToBase64String(Encoding.UTF8.GetBytes(textBox5.Text));
            string time = DateTime.Now.ToString();

            String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();
            
            string insert = "INSERT INTO comment(`requestid`,`content`,`userid`,`time`) VALUES("+ id +",'"+ content +"',"+ uid +",'"+ time + "')";
            
            MySqlCommand comm = new MySqlCommand(insert, conn);
            comm.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("留言成功!");
            requestview requestview = new requestview(id, uid);
            Thread th = new Thread(delegate ()
            {
                requestview.ShowDialog();
            });
            th.Start();
            this.Close();
        }
    }
}
