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
    public partial class eapplychk : Form
    {
        string id;
        int[,] indexarray = new int[1000,2];
        public eapplychk()
        {
            InitializeComponent();
        }
        public eapplychk(string idx)
        {
            InitializeComponent();
            id = idx;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void eapplychk_Load(object sender, EventArgs e)
        {
            String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();

            string sql = "SELECT * from request where byid=" + id;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();
            MySqlConnection conn11 = new MySqlConnection(connetStr);
            conn11.Open();
            MySqlConnection conn2 = new MySqlConnection(connetStr);
            conn2.Open();
            int i = 0;
            while (reader.Read())
            {
                sql = "SELECT * from apply where requestid=" + reader.GetString("id");
                MySqlCommand cmd1111 = new MySqlCommand(sql, conn11);

                MySqlDataReader reader1 = cmd1111.ExecuteReader();
                while (reader1.Read())
                {
                    string status;
                    if (Convert.ToInt32(reader1.GetString("isdeal")) == 0)
                    {
                        status = "[未处理]";
                    }
                    else if (Convert.ToInt32(reader1.GetString("isdeal")) == 1)
                    {
                        status = "[已通过]";
                    }
                    else
                    {
                        status = "[已拒绝]";
                    }
                    sql = "select * from `user` where id =" + reader1.GetString("userid");
                    MySqlCommand cmd2 = new MySqlCommand(sql, conn2);

                    MySqlDataReader reader2 = cmd2.ExecuteReader();
                    reader2.Read();
                    listBox1.Items.Add(status + "--" + Encoding.Default.GetString(Convert.FromBase64String(reader2.GetString("name")))+"--" + Encoding.Default.GetString(Convert.FromBase64String(reader.GetString("title"))));
                    indexarray[i,0] = Convert.ToInt32(reader1.GetString("userid"));
                    indexarray[i, 1] = Convert.ToInt32(reader1.GetString("requestid"));
                    
                    //0=userid  1=requestid
                    reader2.Close();
                    i++;
                }
                reader1.Close();
            }
            conn11.Close();
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择一份简历");
            }
            else
            {
                String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";

                MySqlConnection conn2 = new MySqlConnection(connetStr);
                conn2.Open();
                string sql = "UPDATE `apply` SET `isdeal` = 1 WHERE `userid` = " + indexarray[listBox1.SelectedIndex, 0] + " AND `requestid` = " + indexarray[listBox1.SelectedIndex, 1] ;
                MySqlCommand comm = new MySqlCommand(sql, conn2);
                comm.ExecuteNonQuery();
                conn2.Close();
                MessageBox.Show("已通过！");
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择一份简历");
            }
            else
            {
                String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";

                MySqlConnection conn2 = new MySqlConnection(connetStr);
                conn2.Open();
                string sql = "UPDATE `apply` SET `isdeal` = 2 WHERE `userid` = " + indexarray[listBox1.SelectedIndex, 0] + " AND `requestid` = " + indexarray[listBox1.SelectedIndex, 1];
                MySqlCommand comm = new MySqlCommand(sql, conn2);
                comm.ExecuteNonQuery();
                conn2.Close();
                MessageBox.Show("已拒绝！");

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resume resume1 = new resume(indexarray[listBox1.SelectedIndex,0],false);
            Thread th = new Thread(delegate ()
            {
                resume1.ShowDialog();
            });
            th.Start();
        }
    }
}
