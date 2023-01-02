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
    public partial class papplychk : Form
    {
        string id;
        public papplychk()
        {
            InitializeComponent();
        }
        public papplychk(string idx)
        {
            InitializeComponent();
            id = idx;
        }

        private void papplychk_Load(object sender, EventArgs e)
        {

            String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();

            string sql = "SELECT * from apply where userid="+id;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();
            MySqlConnection conn11 = new MySqlConnection(connetStr);
            conn11.Open();
            
            while (reader.Read())
            {
                sql = "SELECT * from request where id=" + reader.GetString("requestid");
                MySqlCommand cmd1111 = new MySqlCommand(sql, conn11);

                MySqlDataReader reader1 = cmd1111.ExecuteReader();
                reader1.Read();
                string status;
                if (Convert.ToInt32(reader.GetString("isdeal")) == 0)
                {
                    status = "[未处理]";
                }
                else if(Convert.ToInt32(reader.GetString("isdeal")) == 1)
                {
                    status = "[已通过]";
                }
                else
                {
                    status = "[已拒绝]";
                }
                listBox1.Items.Add(status+"-"+reader1.GetString("id")+"--"+ Encoding.Default.GetString(Convert.FromBase64String(reader1.GetString("title"))));
                reader1.Close();
            }
            conn11.Close();
            conn.Close();
        }
    }
}
