using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using static TCP.PacketRecord;

namespace TCP
{
    /// <summary>
    /// bianji.xaml 的交互逻辑
    /// </summary>
    public partial class bianji : Window
    {
        MainWindow ma = null;
        PacketRecordList pack = null;

        public bianji(MainWindow _mw,PacketRecordList _pack)
        {
            ma = _mw;
            pack = _pack;
            InitializeComponent();
                                   
        }
      
        public string Name
        {
            get;set;
        }
        public PacketType Type
        {
            get
            {
                return Type ;
            }
            set
            {
               Type = value;
            }
        }
        public string Packet
        {
            get
            {
                return Packet;
            }
            set
            {
                Packet = value;
            }
        }
        public void Setlabel3(string str)
        {
            TextBox.Text  = str;
        }
        List<PacketRecord> packetrecord = new List<PacketRecord>();
                
        private void Btnqueren_Click(object sender, RoutedEventArgs e)
        {
            //PacketRecordList add = new PacketRecordList();       
            string type = TextBox.Text.ToString();
            if (type == "请输入你要增加的报文")
            {

                string Textname = new PacketRecord(TextName.Text, PacketRecord.PacketType.String, TextEdit.Text).Name;
                pack .RecordsAdd(new PacketRecord(TextName.Text, PacketRecord.PacketType.String, TextEdit.Text));
                pack.SaveRecordsToFile("F:\\testtxt.txt");
                
                ma.LstTxtItem(Textname);
                
                this.Close();
                MessageBox.Show("添加成功","提示");
                //PacketRecord R = new PacketRecord(R .Name, R .Packet, R .Type );
            }
            else if (type == "请输入你要修改的报文")
            {
                var va = pack.RecordsIndexOf(TextName.Text);
                va.Packet = TextEdit.Text;
                MessageBox.Show("修改成功", "提示");
                this.Close();

               // pack .RecordsIndexOf (new PacketRecord (TextName .Text ,PacketRecord .PacketType .String ,))
                //string rev = new PacketRecord(TextName.Text, PacketRecord.PacketType.String, TextEdit.Text).Name;
                                 
            }          
        }

        private void Btnquxiao_Click(object sender, RoutedEventArgs e)
        {                
            this.Hide();
        }
    }
}
