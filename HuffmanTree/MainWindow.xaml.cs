using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;

namespace HuffmanTree
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private Dictionary<char, double> targetDic;
        private Dictionary<char, string> resultDic;
        private Huffman hufTree;
        private string hufCode;
        private string content;
        private TextRange textRange1;
        private TextRange textRange2;
        private TextRange textRange3;
        private TextRange textRange4;
        private TextRange textRange5;
        private TextRange textRange6;
        private TextRange textRange7;
        private TextRange textRange8;

        private DrawTree drawTree;

        private byte[] result;
        private IPAddress ip;
        private Socket clientSocket;


        public MainWindow()
        {

            InitializeComponent();
            comparePage.Visibility = Visibility.Hidden;
            treePage.Visibility = Visibility.Hidden;
            chatPage.Visibility = Visibility.Hidden;
            //uffWindow = new HuffmanWindow();

            targetDic = new Dictionary<char, double>();
            resultDic = new Dictionary<char, string>();

            hufTree = new Huffman();
            drawTree = new DrawTree(treeCanvas, labelDemo);
            get_Range();

        }

        private void richTextBoxMsg_KeyDown(object sender, KeyEventArgs e)
        {
            //control+enter 发送信息
            if ((Keyboard.Modifiers&ModifierKeys.Control)==ModifierKeys.Control&&e.Key==Key.Enter)
            {
                textRange8 = Get_TextRange(richTextBoxMsg);
                string str = textRange8.Text;
                try
                {
                    clientSocket.Send(Encoding.UTF8.GetBytes(str.Substring(0, str.Length - 2)));
                    textRange8.Text = "";
                    textRange7.Text += "me:\t\n" + str;

                    richTextBoxRecode.ScrollToEnd();
                }
                catch
                {
                    textRange7.Text += "发送失败" + '\n';
                    richTextBoxRecode.ScrollToEnd();
                }
                textRange8.Text = "";
            }
        }

        private void show_HuffmanTree(object sender, RoutedEventArgs e)
        {
            //huffWindow.Show();
            //huffWindow.creat_Tree(targetDic);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //huffWindow.Close();
        }

        private TextRange Get_TextRange(RichTextBox box)
        {
            TextRange textRange = new TextRange(box.Document.ContentStart, box.Document.ContentEnd);
            return textRange;
        }
        private void calculate_TargetDic()
        {
            try
            {
                foreach (char c in content)
                {
                    if (!targetDic.ContainsKey(c))
                    {
                        targetDic.Add(c, 1.0 / content.Length);
                    }
                    else
                    {
                        targetDic[c] += 1.0 / content.Length;
                    }
                }
                //按频率从小到大排序
                targetDic = targetDic.OrderBy(r => r.Value).ToDictionary(r => r.Key, r => r.Value);
            }
            catch
            {

            }

        }
        private void get_Range()
        {
            textRange1 = Get_TextRange(richTextBox1);
            textRange2 = Get_TextRange(richTextBox2);
            textRange3 = Get_TextRange(richTextBox3);
            textRange4 = Get_TextRange(richTextBox4);
            textRange5 = Get_TextRange(richTextBox5);
            textRange6 = Get_TextRange(richTextBox6);
            textRange7 = Get_TextRange(richTextBoxRecode);
            textRange8 = Get_TextRange(richTextBoxMsg);

            //textRange1.Text = "";
            textRange2.Text = "";
            textRange3.Text = "";
            textRange4.Text = "";
            textRange5.Text = "";
            textRange6.Text = "";
            textRange7.Text = "";
            textRange8.Text = "";
        }

        private void botton5_click(object sender, RoutedEventArgs e)
        {
            //清空数据
            this.hufTree.Clear();
            textRange1.Text = "";
            textRange2.Text = "";
            textRange3.Text = "";
            textRange4.Text = "";
        }
        private void Close_Icon_MouseEnter(object sender, MouseEventArgs e)
        {
            Close_Icon1.Visibility = Visibility.Visible;

        }

        private void Close_Icon1_MouseLeave(object sender, MouseEventArgs e)
        {
            Close_Icon1.Visibility = Visibility.Hidden;

        }

        private void Close_Icon1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //关闭程序
            Application.Current.Shutdown();
        }


        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            //窗口拖动
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void icoButton1_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButton1.Foreground = Brushes.White;
        }

        private void icoButton1_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButton1.Foreground = Brushes.Black;
        }

        private void icoButton2_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButton2.Foreground = Brushes.White;
        }

        private void icoButton2_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButton2.Foreground = Brushes.Black;
        }
        private void icoButton3_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButton3.Foreground = Brushes.White;
        }

        private void icoButton3_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButton3.Foreground = Brushes.Black;
        }

        private void icoButton1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //清空频率词典
            targetDic.Clear();
            get_Range();
            try
            {
                content = textRange1.Text.Substring(0, textRange1.Text.Length - 2);
            }
            catch
            {

            }
            //计算频率词典
            calculate_TargetDic();
            if (content != null)
            {
                //根据频率词典 获得编码词典
                resultDic = hufTree.get_HuffmanCode(targetDic);

                string str = "";
                foreach (char key in targetDic.Keys)
                {
                    if (key == '\n')
                    {
                        str += "\\n" + "    " + Convert.ToDouble(targetDic[key]).ToString("0.0000") + "\n";
                    }
                    else if (key == '\r')
                    {
                        str += "\\r" + "    " + Convert.ToDouble(targetDic[key]).ToString("0.0000") + "\n";
                    }
                    else if (key == ' ')
                    {
                        str += "' '" + "   " + Convert.ToDouble(targetDic[key]).ToString("0.0000") + "\n";
                    }
                    else
                        str += key + "     " + Convert.ToDouble(targetDic[key]).ToString("0.0000") + "\n";
                }
                textRange2.Text = str;
                string str2 = "";
                foreach (char key in resultDic.Keys)
                {
                    if (key == '\n')
                    {
                        str2 += "\\n" + "\t" + resultDic[key] + "\n";
                    }
                    else if (key == '\r')
                    {
                        str2 += "\\t" + "\t" + resultDic[key] + "\n";
                    }
                    else if (key == ' ')
                    {
                        str2 += "' '" + "\t" + resultDic[key] + "\n";
                    }
                    else
                    {
                        str2 += key + "\t" + resultDic[key] + "\n";
                    }

                }
                textRange3.Text = str2;

                hufCode = "";
                foreach (char i in content)
                {
                    try
                    {
                        hufCode += resultDic[i];
                    }
                    catch
                    {

                    }
                }

                textRange4.Text = hufCode;
                resultDic.Clear();
            }
            else
            {
                //MessageBox.Show("无字符");
            }
        }

        private void icoButtonTree_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButtonTree.Foreground = Brushes.GreenYellow;
        }

        private void icoButtonTree_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButtonTree.Foreground = Brushes.Black;
        }

        private void icoButton2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainPage.Visibility = Visibility.Hidden;
            comparePage.Visibility = Visibility.Visible;
            textRange5.Text = textRange1.Text;
            //遍历二叉树获取译文
            textRange6.Text = hufTree.translateCode(hufCode);
        }

        private void icoButtonBack_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButtonBack.Foreground = Brushes.White;
        }

        private void icoButtonBack_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButtonBack.Foreground = Brushes.Black;
        }

        private void icoButtonBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainPage.Visibility = Visibility.Visible;
            comparePage.Visibility = Visibility.Hidden;

        }

        private void icoButtonTree_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainPage.Visibility = Visibility.Hidden;
            comparePage.Visibility = Visibility.Hidden;
            treePage.Visibility = Visibility.Visible;

            //画树
            if (drawTree == null)
            {
                drawTree = new DrawTree(treeCanvas, labelDemo);
            }
            drawTree.creat_Tree(targetDic);
        }

        private void icoButtonBack2_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButtonBack2.Foreground = Brushes.White;
        }

        private void icoButtonBack2_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButtonBack2.Foreground = Brushes.Black;
        }

        private void icoButtonBack2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            treePage.Visibility = Visibility.Hidden;
            mainPage.Visibility = Visibility.Visible;
            //清空画树页面 
            treeCanvas.Children.Clear();
            drawTree.reset();
        }

        private void icoButtonChat_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButtonChat.Foreground = Brushes.White;
        }

        private void icoButtonChat_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButtonChat.Foreground = Brushes.Black;
        }

        private void icoButtonChat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainPage.Visibility = Visibility.Hidden;
            chatPage.Visibility = Visibility.Visible;

            //连接socket服务器
            result = new byte[1024];
            ip = IPAddress.Parse("120.27.117.34");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 9999));
                clientSocket.BeginReceive(result, 0, 1024, 0, new AsyncCallback(ReceiveCallBack), null);
            }
            catch
            {
                textRange7.Text += "连接失败" + "\n";
            }
        }

        private void icoButtonBack3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            chatPage.Visibility = Visibility.Hidden;
            mainPage.Visibility = Visibility.Visible;

            //返回按钮，关闭socket
            clientSocket.Close();
            textRange7.Text = "";
            textRange8.Text = "";
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            //获取服务器发来的信息 回调函数
            try
            {
                int rEnd = clientSocket.EndReceive(ar);

                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {

                    string str = Encoding.UTF8.GetString(result).Replace("\0", "");
                    if (!str.Contains("("))
                    {
                        textRange7.Text += str + "\n";
                    }
                    else
                    {
                        int left = str.IndexOf("(");
                        int right = str.IndexOf(")");
                        if (left >= 0 && right > 0)
                        {
                            textRange7.Text += (str.Substring(left + 1, right - 1)) + ":\n";
                            textRange7.Text += (str.Substring(right + 1));
                            this.textRange7.Text += "\n";
                        }
                    }
                    richTextBoxRecode.ScrollToEnd();
                    //this.textRange7.Text += "\n";
                    Console.WriteLine(Encoding.UTF8.GetString(result).Replace("\0", "").Length);
                    Array.Clear(result, 0, result.Length);
                }
                );
                clientSocket.BeginReceive(result, 0, 1024, 0, new AsyncCallback(ReceiveCallBack), null);
            }
            catch
            {

            }
            
        }

        private void icoButtonBack3_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButtonBack3.Foreground = Brushes.White;
        }

        private void icoButtonBack3_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButtonBack3.Foreground = Brushes.Black;
        }

        private void icoButtonPush_MouseEnter(object sender, MouseEventArgs e)
        {
            icoButtonPush.Foreground = Brushes.White;
        }

        private void icoButtonPush_MouseLeave(object sender, MouseEventArgs e)
        {
            icoButtonPush.Foreground = Brushes.Black;
        }

        private void icoButtonPush_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //用按钮发送信息
            textRange8 = Get_TextRange(richTextBoxMsg);
            string str = textRange8.Text;
            try
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(str.Substring(0, str.Length - 2)));
                textRange8.Text = "";
                textRange7.Text += "me:\t\n" + str;
                
                richTextBoxRecode.ScrollToEnd();
            }
            catch
            {
                textRange7.Text += "发送失败" + '\n';
                richTextBoxRecode.ScrollToEnd();
            }
            textRange8.Text = "";
        }

        private void richTextBoxMsg_KeyDown_1(object sender, KeyEventArgs e)
        {

        }
    }
}
