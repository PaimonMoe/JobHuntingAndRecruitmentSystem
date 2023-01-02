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
    public partial class main2p : Form
    {
        string id, name;
        public main2p()
        {
            InitializeComponent();
        }
        public main2p(string idx, string namex)
        {
            InitializeComponent();
            id = idx;
            name = Encoding.Default.GetString(Convert.FromBase64String(namex));
        }
        int[] indexarray = new int[1000];

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择一个查看的条目");
            }
            else
            {
                int viewindex = listBox1.SelectedIndex;
                requestview requestview = new requestview(Convert.ToString(indexarray[viewindex]), id);
                Thread th = new Thread(delegate ()
                {
                    requestview.ShowDialog();
                });
                th.Start();

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            login login = new login();
            Thread th = new Thread(delegate ()
            {
                login.ShowDialog();
            });
            th.Start();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择一个投递简历的条目");
            }
            else
            {
                String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
                name = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));
                MySqlConnection conn0 = new MySqlConnection(connetStr);
                conn0.Open();//建立连接
                string existcheck = "SELECT * FROM apply where `requestid`='" + indexarray[listBox1.SelectedIndex] + "' and `userid`="+id;
                MySqlCommand existcheckcomm = new MySqlCommand(existcheck, conn0);
                if (Convert.ToInt32(existcheckcomm.ExecuteScalar()) == 0)
                {
                    connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
                    MySqlConnection conn = new MySqlConnection(connetStr);
                    conn.Open();

                    string insert = "INSERT INTO apply VALUES(" + id + "," + indexarray[listBox1.SelectedIndex] + ",0)";

                    MySqlCommand comm = new MySqlCommand(insert, conn);
                    comm.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("投递成功!");
                }
                else
                {
                    MessageBox.Show("您已经投递过此项目了");
                }
                conn0.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            papplychk papplychk1 = new papplychk(id);
            Thread th = new Thread(delegate ()
            {
                papplychk1.ShowDialog();
            });
            th.Start();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resume resume1 = new resume(Convert.ToInt32(id),true);
            Thread th = new Thread(delegate ()
            {
                resume1.ShowDialog();
            });
            th.Start();
        }

        private void main2p_Load(object sender, EventArgs e)
        {
            label1.Text = "尊敬的求职者：" + name + "    您好！";

            String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();

            string sql = "SELECT * from request";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();

            int i = 0;
            MySqlConnection conn11 = new MySqlConnection(connetStr);
            conn11.Open();
            while (reader.Read())
            {
                indexarray[i] = Convert.ToInt32(reader.GetString("id"));
                
                string sql11 = "SELECT * from user where id=" + reader.GetString("byid");
                MySqlCommand cmd11 = new MySqlCommand(sql11, conn11);
                MySqlDataReader reader11 = cmd11.ExecuteReader();
                reader11.Read();
                string pubuser = Encoding.Default.GetString(Convert.FromBase64String(reader11.GetString("name")));
                listBox1.Items.Add("      ✿" + reader.GetString("id") + "--单位:" + pubuser + "----" + Encoding.Default.GetString(Convert.FromBase64String(reader.GetString("title"))) + "---[" + reader.GetString("reqnum") + "]人");
                i++;
                reader11.Close();
            }
            conn11.Close();
            conn.Close();
        }
    }
}
