using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
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
using System.Windows.Shapes;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Windows.Navigation;
using System.Net.Http;
using System.Windows.Threading;
using System.IO;
using MySql.Data.MySqlClient;

namespace TCP
{
   
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        Socket socket;
        Thread threadReceive;
        private ReceiveMsgCallBack receiveCallBack;
        private delegate string ConnectSocketDelegate(IPEndPoint ipep, Socket sock);
        private delegate void ReceiveMsgCallBack(string strMsg);
        DispatcherTimer time = new DispatcherTimer();
        bianji Bianji = null;



        private byte[] data = new byte[1024];
        private int size = 1024;
        private Socket server;
        private NetworkStream networkStream;
        PacketRecordList pack = new PacketRecordList();
        
        public PacketRecordList  Pack
        {
            get
            {
                return pack;
            }
            set
            {
                pack = value;
            }
        }
        public bianji  BIANJI
        {
            get
            {
                return Bianji;

            }
            set
            {
                Bianji = value;
            }
        }



        //List<EndPoint> TCPServerEndPointList = new List<EndPoint>();
        
        public MainWindow()
        {
            InitializeComponent();
            bool paket = pack.LoadRecordsFromFile("F:\\testtxt.txt");
            ThreadInvoker.Instance.InitDispatcher();
            //btnstop.IsEnabled = false;
            combox.Items.Add("TCPClient");
            combox.Items.Add("TCPServer");
            ComRequest.Items.Add("POST");
            ComRequest.Items.Add("GET");
            ComEncoding.Items.Add("application/json; charset=UTF-8");


            if (paket)
            {
                foreach (var name in pack.Records )
                {
                    listBaoWen.Items.Add(name.Name);
                }
            }
        }

        TCPClientCenter tcpClient = null;
        private void Btnconnect_Click(object sender, RoutedEventArgs e)
        {
            string Type = combox.SelectedItem.ToString();
            if (Type == "TCPClient")
            {
                if (tcpClient == null)
                {
                    if (TextBox4.Text == "")
                        return;
                    int port = 0;
                    string ip = "";
                    ip = Convert.ToString(TextBox3.Text);
                    try
                    {
                        port = Convert.ToInt32(TextBox4.Text);
                    }
                    catch
                    {
                        return;
                    }

                    tcpClient = new TCPClientCenter();
                    tcpClient.TcpClientCreate(ip, port);
                    tcpClient.Main = this;
                    tcpClient.ClientStart();
                    //btnconnect.IsEnabled = false;

                }
            }
            else if (Type == "TCPServer")
            {
                if (tcpServer == null)
                {
                    if (TextBox4.Text == "")
                        return;
                    int port = 0;
                    try
                    {
                        port = Convert.ToInt32(TextBox4.Text);
                    }
                    catch
                    {
                        return;
                    }

                    tcpServer = new TCPServerCenter();
                    tcpServer.TcpServerCreate(port);
                    tcpServer.Main = this;
                    tcpServer.Start();
                   
                    btnconnect.IsEnabled = false;
                }
            }
        }
        public void AddList(string str)
        {
            if (!listbox.Items.Contains(str))
                listbox.Items.Add(str);
        }
        public void RemoveList(string str)
        {
            listbox.Items.Remove(str);
        }
       
        private Dictionary<string, Session> m_ipSessionMapping = new Dictionary<string, Session>();

        public Dictionary<string, Session> IpSessionMapping
        {
            get
            {
                return m_ipSessionMapping;
            }
            set
            {
                m_ipSessionMapping = value;
            }
        }
        
                                                                                                                                                                            


        private string ConnectSocket(IPEndPoint ipep, Socket sock)
        {
            string exmessage = "";
            try
            {
                sock.Connect(ipep);
            }
            catch (System.Exception ex)
            {
                exmessage = ex.Message;
            }
            finally
            {
            }

            return exmessage;
        }

        public delegate void Feedback(string str);
        public void callback(string str)
        {
            ThreadInvoker.Instance.RunByUiThread(() =>
            {              
                if(TextB1.Text == "")
                {
                    rere(str);
                }
                else 
                {
                   
                }


            });
        }

        /*public  void rec(string str)
        {
            str = str.Replace("\0","");
            TexBox6.Text += str + "\r\n";
        }*/
        public void rere(string str)
        {
          //  string result = string.Empty;

            string strs = str.Replace("\0", "");
            string Check = pack.RecordsCheckOf(strs).Name;

            str = "【" + DateTime.Now.ToString() + "】" + "  :  " + Check  + " : " + str.Replace("\0", "");
            //byte[] arrByte = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
            //for(int i = 0;i<arrByte .Length;i++)
            //{
            //    result += "&#x" + System.Convert.ToString(arrByte[i], 16) + ";";
            //}
            TextB1.Text += str  + "\r\n"; ;

        }

        private void Btnsend_Click(object sender, RoutedEventArgs e)
        {
            
            string Type2 = combox.SelectedItem.ToString();
            if (Type2 == "TCPClient")
            {
               // if (RadioBtn.IsChecked==true )
                //{
                    string strMsg = this.TextBox2.Text.Trim();
                    byte[] buffer = new byte[2048];
                    buffer = Encoding.Default.GetBytes(strMsg);
                    

                    //byte[] b = new byte[strMsg.Length];
                    //for (int i = 0; i < strMsg.Length; i++)
                    //{
                       // b[i] = Convert.ToByte(strMsg[i]);
                    //}
                    tcpClient.TcpipCenterSendData(tcpClient.Client.ClientSession, buffer, buffer.Length);
                //}
               
            }

            else if (Type2 == "TCPServer")
            {
                string strMsg = this.TextBox2.Text.Trim();
                byte[] buffer = new byte[2048];
                buffer = Encoding.Default.GetBytes(strMsg);
                byte[] b = new byte[strMsg.Length];
                for (int i = 0; i < strMsg.Length; i++)
                {
                    b[i] = Convert.ToByte(strMsg[i]);
                }
                var item = listbox.SelectedItem.ToString();
                tcpServer.TcpipCenterSendData(m_ipSessionMapping[listbox.SelectedItem.ToString()], buffer, buffer.Length);


            }
            //TextBox2.Text = null;
        }
        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[2048];
                    int r = socket.Receive(buffer);

                    if (r == 0)
                    {
                        break;
                    }
                    else
                    {
                        string str = Encoding.Default.GetString(buffer, 0, r);
                        this.TextB1.Dispatcher.Invoke(receiveCallBack, str);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private void SetValue(string strValue)
        {
            this.TextB1.AppendText(strValue);
            this.TextB1.ScrollToEnd();
        }


        private void Btnclear_Click(object sender, RoutedEventArgs e)
        {
            TextB1.Text = "";
            TextBox2.Text = "";
        }

        TCPServerCenter tcpServer = null;
        private void BtnLister_Click(object sender, RoutedEventArgs e)
        {
           
        }
       
    private void Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
            
           // combox.SelectionChanged += new SelectionChangedEventHandler(Combox_SelectionChanged);
           // ComboBoxItem item = combox.SelectedItem as ComboBoxItem;
           if(combox .SelectedIndex ==0)
            {
                btnconnect.Content = "开始连接";
                TextBox3.IsEnabled = true; 

            }
           else if(combox.SelectedIndex ==1)
            {
                TextBox3.IsEnabled = false;
                //btnsend.IsEnabled = false;
                btnconnect.Content = "开始监听";
            }



    }

        private void Combox_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //listbox.SelectedItem as string;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
           
        }
        public void LstTxtItem(string item)
        {
            listBaoWen.Items.Add(item );
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

            bianji BianJi = new bianji(this, pack);
            BianJi.Setlabel3("请输入你要增加的报文");
            BianJi.ShowDialog();
           
        }
        private void BtnXiuGai_Click(object sender, RoutedEventArgs e)
        {
            // bianji B = new bianji(this, pack);
            bianji BianJi = new bianji(this, pack);
            string TexName = listBaoWen.SelectedItem.ToString();
            BianJi.Setlabel3("请输入你要修改的报文");
                     
            var v = pack.RecordsIndexOf(TexName);
            BianJi.TextName.Text = v.Name;
            BianJi.TextEdit.Text = v.Packet;
            BianJi.ShowDialog();


        }       
        private void BtnShan_Click(object sender, RoutedEventArgs e)
        {
            //PacketRecordList pa = new PacketRecordList();
            string list = listBaoWen.SelectedItem.ToString();
            if (listBaoWen.SelectedItem == null)
            {
                MessageBox.Show("请先选中某一项", "ERROR");
            }
            else 
            {


                listBaoWen.Items.RemoveAt(listBaoWen.SelectedIndex);
                
                
                pack.RecordsRemoveByName(list);
                MessageBox.Show("删除成功", "提示");
            }
            //pa .RecordsRemove 

        }
        private void ListBaoWen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ListBaoWen_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string Type2 = combox.SelectedItem.ToString();

            string strMsg = listBaoWen.SelectedItem.ToString();
            var a = pack.RecordsIndexOf(strMsg);
            string Msg = a.Packet;
            byte[] buffer = new byte[2048];
            buffer = Encoding.Default.GetBytes(Msg);
            //var item = listBaoWen.SelectedItems[0].ToString();
            if (Type2 == "TCPClient")
            {
                tcpClient.TcpipCenterSendData(tcpClient.Client.ClientSession, buffer, buffer.Length);
            }
        
            else if(Type2=="TCPServer")
            {
               
                tcpServer.TcpipCenterSendData(m_ipSessionMapping[listbox.SelectedItem.ToString()], buffer, buffer.Length);
            }
        }

        private void btnrequest_Click(object sender, RoutedEventArgs e)
        {
            string SUrl = TextRequest.Text;
            string PostData = TextPostData.Text;
            string Contenttype = ComEncoding.Text;
            Post(SUrl, PostData, Contenttype);
        }
        public static string Post(string Url, string postDataStr, string cookies)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            if (cookies != null)
                request.Headers.Add("Cookie", cookies);
            request.ContentType = "application/json; charset=UTF-8";
            request.ContentLength = postDataStr.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(postDataStr);
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;
        }
    }
    }

