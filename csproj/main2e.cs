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
    public partial class main2e : Form
    {
        string id,name;
        public main2e()
        {
            InitializeComponent();
        }

        public main2e(string idx,string namex)
        {
            InitializeComponent();
            id = idx;
            name = Encoding.Default.GetString(Convert.FromBase64String(namex));
        }



        int[] indexarray = new int[1000];
        bool[] candelarray = new bool[1000];
        


        // set listbox item height
        void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 41;
        }


        void _listBox_DrawItem(object sender, DrawItemEventArgs e)//center align text
        {
            e.DrawBackground();
            e.DrawFocusRectangle();


            System.Drawing.StringFormat strFmt = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
            strFmt.Alignment = System.Drawing.StringAlignment.Center;


            RectangleF rf = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);



            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new System.Drawing.SolidBrush(e.ForeColor), rf, strFmt);
        }



        public void drawlb()
        {
            listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            listBox1.DrawItem += _listBox_DrawItem;
            listBox1.MeasureItem += listBox1_MeasureItem;

        }



        private void button2_Click(object sender, EventArgs e)
        {
            resquestpublish resquestpublish = new resquestpublish(id);
            Thread th = new Thread(delegate ()
            {
                resquestpublish.ShowDialog();
            });
            th.Start();
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择一个需要删除的条目");
            }
            else
            {
                
                int delindex = listBox1.SelectedIndex;
                if (!candelarray[delindex])
                {
                    MessageBox.Show("只能删除自己发布的");
                }
                else
                {


                    String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";

                    MySqlConnection conn = new MySqlConnection(connetStr);
                    try
                    {

                        conn.Open();
                        string del = "delete from request where id = " + indexarray[delindex];
                        MySqlCommand comm = new MySqlCommand(del, conn);
                        if (comm.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("删除成功");
                            conn.Close();
                            this.main2e_Load(sender, e);
                        }


                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择一个查看的条目");
            }
            else
            {
                int viewindex = listBox1.SelectedIndex;
                requestview requestview = new requestview(Convert.ToString(indexarray[viewindex]),id);
                Thread th = new Thread(delegate ()
                {
                    requestview.ShowDialog();
                });
                th.Start();

                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            eapplychk eapplychk1 = new eapplychk(id);
            Thread th = new Thread(delegate ()
            {
                eapplychk1.ShowDialog();
            });
            th.Start();
            
        }

        private void main2e_Load(object sender, EventArgs e)
        {

            label2.Text = "尊敬的企业用户：" + name + "    欢迎您！";

            String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();

            string sql = "SELECT * from request";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();
            
            int i=0;
            MySqlConnection conn11 = new MySqlConnection(connetStr);
            conn11.Open();
            while (reader.Read())
            {
                indexarray[i] = Convert.ToInt32(reader.GetString("id"));
                candelarray[i] = false;
                if (reader.GetString("byid") == id)
                {
                    candelarray[i] = true;
                }
                
                string sql11 = "SELECT * from user where id="+ reader.GetString("byid");
                MySqlCommand cmd11 = new MySqlCommand(sql11, conn11);
                MySqlDataReader reader11 = cmd11.ExecuteReader();
                reader11.Read();
                string pubuser = Encoding.Default.GetString(Convert.FromBase64String(reader11.GetString("name")));
                listBox1.Items.Add("      ✿"+reader.GetString("id")+"--单位:" + pubuser + "----"+Encoding.Default.GetString(Convert.FromBase64String(reader.GetString("title")))+ "---["+reader.GetString("reqnum")+"]人");
                i++;
                reader11.Close();
            }
            conn11.Close();
            conn.Close();

            //drawlb();
        }
    }
}
