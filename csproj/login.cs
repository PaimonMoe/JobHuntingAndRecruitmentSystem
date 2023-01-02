using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace csproj
{
    
    public partial class login : Form
    {
        string vcode,vcode2;
        public login()
        {
            InitializeComponent();
        }
        public login(string str)
        {
            InitializeComponent();
            textBox1.Text = Encoding.Default.GetString(Convert.FromBase64String(str));
        }
        public string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 24.0), 35);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        private Image ByteToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            return Image.FromStream(ms, true);
        }
        public void ShowCode()
        {
            vcode = CreateValidateCode(2);//把生成的验证码赋值给变量
            byte[] buffer = CreateValidateGraphic(vcode);//把字符串转成字节流
            pictureBox1.Image = ByteToImage(buffer);//把字节流转成图片显示在窗体中的pictureBox1控件
        }
        public void ShowCode2()
        {
            vcode2 = CreateValidateCode(2);//把生成的验证码赋值给变量
            byte[] buffer = CreateValidateGraphic(vcode2);//把字符串转成字节流
            pictureBox2.Image = ByteToImage(buffer);//把字节流转成图片显示在窗体中的pictureBox1控件
        }

        private void login_Load(object sender, EventArgs e)
        {
            ShowCode();
            ShowCode2();
        }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textBox3.Text.Trim()) != (Convert.ToInt32(vcode) + Convert.ToInt32(vcode2)))
            {
                MessageBox.Show("验证码错误");
                textBox3.Focus();//输入错误，输入框获取焦点
            }
            else
            {
                //Code Check ok.
                String connetStr = "server=39.107.226.228;port=3306;user=root;password=4565; database=CsharpRequired;sslmode=none";

                MySqlConnection conn1 = new MySqlConnection(connetStr);
                try{
                    conn1.Open();
                    string sql = "SELECT * from user where `name`='"+ Convert.ToBase64String(Encoding.UTF8.GetBytes(textBox1.Text.Trim())) +"' and `pw`='"+ GetMd5("salt888" + textBox2.Text.Trim() + "salt666") + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, conn1);
                    
                    if(cmd.ExecuteScalar() != null)
                    {
                        MessageBox.Show("登录成功. 您的id为"+Convert.ToInt32(cmd.ExecuteScalar()));
                        MySqlCommand cmd1 = new MySqlCommand(sql,conn1);

                        MySqlDataReader reader = cmd1.ExecuteReader();
                        reader.Read();
                        //MessageBox.Show(reader.GetString("category"));
                        if (reader.GetString("category") == "5LyB5Lia")
                        {

                            main2e main = new main2e(reader.GetString("id"), reader.GetString("name"));
                            Thread th = new Thread(delegate ()
                            {
                                main.ShowDialog();
                            });
                            th.Start();
                            conn1.Close();
                            this.Close();
                        }
                        else
                        {
                            main2p main = new main2p(reader.GetString("id"), reader.GetString("name"));
                            Thread th = new Thread(delegate ()
                            {
                                main.ShowDialog();
                            });
                            th.Start();
                            conn1.Close();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误");

                    }
                }
                catch (MySqlException ex) {
                    MessageBox.Show(ex.ToString());
                }
                
                
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ShowCode2();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reg form = new reg();
            Thread th = new Thread(delegate ()
            {
                form.ShowDialog();
            });
            th.Start();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowCode();
        }
    }
}
