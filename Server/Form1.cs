using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        static int M = 52;
        private TCPModel tcp;
        private SocketModel[] socketList;
        private SocketModel[] socketList1;
        private int numberOfPlayers = 4;
        private int currentClient;
        private object thislock;
        QuanBai[] labai = new QuanBai[52];
        double[] s = new double[M];
        List<double> deck = new List<double>();
        Room r=new Room();
        int dem = 0; int l = 0;
        readonly object balanceLock = new object();
        int tamthoi = 0;
        void XaoBai()
        {
            Random rnd = new Random();
            int k = 0;
            for (int i = 3; i <= 15; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    labai[k] = new QuanBai();
                    labai[k].ten = i;
                    labai[k].chat = j;
                    s[k] = labai[k].ten + 0.1 * labai[k].chat;
                    k++;
                }
            }
            int n = 52;
            double temp;
            int index;
            for (int i = 0; i < n; i++)
            {
                index = rnd.Next(n);
                temp = s[i];
                s[i] = s[index];
                s[index] = temp;
            }

            //     while(n>0)
            //     {
            //         index = rnd.Next(n);
            //         deck.Add(s[index]);
            //         for (int i = 0; i < n; i++)
            //         {
            //             foreach(var item in deck)
            //             if (s[i] == item)
            //             {  
            //                  for(int j= i;j<n-1;j++)
            //                  s[j] = s[j + 1];
            //             }
            //         }n--;
            //     }
            //foreach(var item in deck)
            //     {
            //         Console.WriteLine(item);
            //     }
        }
        public Form1()
        {
            InitializeComponent();
           
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            thislock = new object();
        }
        public void StartServer()
        {
            string ip = textBox1.Text;
            int port = int.Parse(textBox2.Text);
            tcp = new TCPModel(ip, port);
            tcp.Listen();
            button1.Enabled = false;
        }
        public void ServeClients()
        {
            socketList = new SocketModel[numberOfPlayers];
            socketList1 = new SocketModel[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                ServeAClient();
            }
        }
        public void Accept()
        {
            int status = -1;
            Socket s = tcp.SetUpANewConnection(ref status);
            socketList[currentClient] = new SocketModel(s);
            string str = socketList[currentClient].GetRemoteEndpoint();
            string str1 = "New connection from: " + str + "\n";
            textBox3.AppendText(str1);
        }
        public void Accept1()
        {
            int status = -1;
            Socket s = tcp.SetUpANewConnection(ref status);
            socketList1[currentClient] = new SocketModel(s);
        }

        public void ServeAClient()
        {
            int num = -1;
            lock (thislock)
            {
                Accept();
                Accept1();
                currentClient++;
                num = currentClient - 1;
            }
            
            Thread t = new Thread(Commmunication);
          t.Start(num);
        }
        public void Commmunication(object obj)
        {
            int pos = (Int32)obj;
            while(true)
            {
            string str = socketList[pos].ReceiveData();
                if (string.Equals(str, "play"))
                {
                        r.Add(socketList[pos]);
                }
            }
        }

        public void BroadcastResult(int pos, string result)
        {
            socketList[pos].SendData(result);
            if (pos == 0)
            {
                socketList1[1].SendData(result);
                socketList1[2].SendData(result);
                socketList1[3].SendData(result);
                return;
            }
            if(pos==1)
            {
                socketList1[0].SendData(result);
                socketList1[2].SendData(result);
                socketList1[3].SendData(result);
                return;
            }
            if(pos==2)
            {
                    socketList1[0].SendData(result);
                    socketList1[1].SendData(result);
                    socketList1[3].SendData(result);
                    return;
            }
            if(pos==3)
            {
                
                    socketList1[0].SendData(result);
                    socketList1[1].SendData(result);
                    socketList1[2].SendData(result);
                    return;
                
            }
        }

        void Button1Click(object sender, EventArgs e)
        {
            StartServer();
            Thread t = new Thread(ServeClients);
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // Shuffle();
            StartServer();
            Thread t = new Thread(ServeClients);
            t.Start();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            tcp.Shutdown();
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(Environment.ExitCode);
        }
    }
}

