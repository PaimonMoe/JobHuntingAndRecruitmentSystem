using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;


namespace csproj
{
    public partial class reg : Form
    {
        public static string GetMd5(string text)
        {
            StringBuilder sb = new StringBuilder();
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                int length = data.Length;
                for (int i = 0; i < length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                return (sb.ToString());
            }
        }
       
        public reg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var name = textBox1.Text.Trim();
            var pw = textBox2.Text.Trim();
            if (comboBox1.SelectedItem != null && name != null && pw != null)
            {

                var cg = comboBox1.SelectedItem.ToString();
                byte[] cgbuffer = Encoding.GetEncoding("utf-8").GetBytes(cg);//string 2 byte

                var pwmatch = Regex.IsMatch(pw, "^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{2,256}$");
                //Password Preg_Match ok.
                if (pwmatch && textBox2.Text == textBox3.Text)
                {
                    
                    //User Side check ok.
                    
                    pw = GetMd5("salt888" + pw + "salt666");
                    //pass word salty md5 ok.
                    String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";
                    name = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));
                    MySqlConnection conn = new MySqlConnection(connetStr);
                    try
                    {
                        conn.Open();//建立连接
                        string existcheck = "SELECT * FROM user where `name`='"+name+"'";
                        MySqlCommand existcheckcomm = new MySqlCommand(existcheck, conn);
                        if (Convert.ToInt32(existcheckcomm.ExecuteScalar()) == 0)
                        {

                            string insert = "INSERT INTO user(`name`,`pw`,`category`) VALUES('" + name + "', '" + pw + "', '" + Convert.ToBase64String(cgbuffer) + "')";
                            MySqlCommand comm = new MySqlCommand(insert, conn);
                            comm.ExecuteNonQuery();


                            string sql = "SELECT * from user ORDER BY id DESC limit 1";
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            MySqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                MessageBox.Show("注册成功!  " + Encoding.Default.GetString(Convert.FromBase64String(reader.GetString("name")))+ "，欢迎您。您的ID为：" + reader.GetString("id"));
                                
                                login form = new login(reader.GetString("name"));
                                Thread th = new Thread(delegate ()
                                {
                                 
                                    form.ShowDialog();
                                });
                                th.Start();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("用户名已被占用，请变更。");
                        }

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }




                    //Actions above is after userside check.
                }
                else if (textBox2.Text == textBox3.Text)
                {
                    MessageBox.Show("密码设置不符合安全性要求！");
                }
                else
                {
                    MessageBox.Show("两次输入的密码不一样");
                }

            }
            else
            {
                MessageBox.Show("请完善表单信息！用户类型只允许填写下拉列表项目。");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            login form = new login();
            Thread th = new Thread(delegate ()
            {

                form.ShowDialog();
            });
            th.Start();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
