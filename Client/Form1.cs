using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        PictureBox[] arrPB = new PictureBox[13];
        PictureBox[] arrPBhands = new PictureBox[13];
        PictureBox[] arrPBleft = new PictureBox[13];
        PictureBox[] arrPBmid = new PictureBox[13];
        PictureBox[] arrPBright = new PictureBox[13];
        int[] imgCurr = new int[13];
        QuanBai[] labai = new QuanBai[52];//cards
        private TCPModel tcpForPlayer;
        private TCPModel tcpForOpponent;
        string[] deck;
        string[] handDeck;
        bool Loaded = false;
        bool hadcheck = false;
        int runortrip = 0;//1 if run, 2 if trip
        bool New = false;
        bool done = false;
        int win_count = 0;int l = 0;int m = 0;int r = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Connect();
            button2.Enabled = false;
            button3.Enabled = false;
        }
        public void Connect()
        {
            string ip = "127.0.0.1";
            int port = 13000;
            tcpForPlayer = new TCPModel(ip, port);
            tcpForPlayer.ConnectToServer();
            this.Text = tcpForPlayer.UpdateInformation();
            tcpForOpponent = new TCPModel(ip, port);
            tcpForOpponent.ConnectToServer();
        }

        public void Sort(string[] a, int n)
        {
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (double.Parse(a[i]) > double.Parse(a[j]))
                    {
                        string temp = a[i];
                        a[i] = a[j];
                        a[j] = temp;
                    }
                }
            }
        }
        public void Receive()
        {
            while (true)
            {
                bool anycards = true;
                string s = tcpForPlayer.ReadData();
                if (string.Equals(s, "new")) { New = true; }//
                else New = false;
                string a = s;
                    if (string.Equals(s, "OK"))//check "ok" signal from room
                    {
                        button2.Enabled = true;
                        button3.Enabled = true;
                    button2.BackColor = Color.PeachPuff;
                    button3.BackColor = Color.PeachPuff;
                    MessageBox.Show("It's "+this.Text+" turn");
                    }
                if (!Loaded)
                {
                    arrPBleft[0] = pictureBox40;arrPBmid[0] = pictureBox27; arrPBright[0] = pictureBox53;
                    arrPBleft[1] = pictureBox41; arrPBmid[1] = pictureBox28; arrPBright[1] = pictureBox54;
                    arrPBleft[2] = pictureBox42; arrPBmid[2] = pictureBox29; arrPBright[2] = pictureBox55;
                    arrPBleft[3] = pictureBox43; arrPBmid[3] = pictureBox30; arrPBright[3] = pictureBox56;
                    arrPBleft[4] = pictureBox44; arrPBmid[4] = pictureBox31; arrPBright[4] = pictureBox57;
                    arrPBleft[5] = pictureBox45; arrPBmid[5] = pictureBox32; arrPBright[5] = pictureBox58;
                    arrPBleft[6] = pictureBox46; arrPBmid[6] = pictureBox33; arrPBright[6] = pictureBox59;
                    arrPBleft[7] = pictureBox47; arrPBmid[7] = pictureBox34; arrPBright[7] = pictureBox60;
                    arrPBleft[8] = pictureBox48; arrPBmid[8] = pictureBox35; arrPBright[8] = pictureBox61;
                    arrPBleft[9] = pictureBox49; arrPBmid[9] = pictureBox36; arrPBright[9] = pictureBox62;
                    arrPBleft[10] = pictureBox50; arrPBmid[10] = pictureBox37; arrPBright[10] = pictureBox63;
                    arrPBleft[11] = pictureBox51; arrPBmid[11] = pictureBox38; arrPBright[11] = pictureBox64;
                    arrPBleft[12] = pictureBox52; arrPBmid[12] = pictureBox39; arrPBright[12] = pictureBox65;
                    for (int i=0;i<13;i++)
                    {
                        string temp = @"PNG\" +"blue_back.png";//img link
                        arrPBleft[i].Image = Image.FromFile(temp);
                        arrPBleft[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        arrPBmid[i].Image = Image.FromFile(temp);
                        arrPBmid[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        arrPBright[i].Image = Image.FromFile(temp);
                        arrPBright[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    //load opp cards
                    deck = s.Split(';');//get cards from server and split
                    for (int i = 0; i < 13; i++)
                    {
                        Sort(deck, 13);
                        string temp = @"PNG\" + deck[i].Replace('.', '_') + ".png";//img link
                        arrPB[i].Image = Image.FromFile(temp);
                        arrPB[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    Loaded = true;
                }
              

                if (anycards)//opp cards
                {
                    try {
                        if (!string.Equals(a, "OK"))
                        {
                            if(a.Contains("l>"))
                            {
                                string[] tam = a.Split('>');
                                //MessageBox.Show(tam[1]);
                                string[] demso = tam[1].Split('/');
                                for (int i=0;i<demso.Count()-1;i++)
                                {
                                    arrPBleft[l].Visible = false;
                                    l++;
                                }
                                a = a.Remove(0,2);
                                //MessageBox.Show(a);
                            }
                            if (a.Contains("m>"))
                            {
                                string[] tam = a.Split('>');
                                //MessageBox.Show(tam[1]);
                                string[] demso = tam[1].Split('/');
                                for (int i = 0; i < demso.Count() - 1; i++)
                                {
                                    arrPBmid[m].Visible = false;
                                    m++;
                                }
                                a = a.Remove(0, 2);
                                //MessageBox.Show(a);
                            }
                            if (a.Contains("r>"))
                            {
                                string[] tam = a.Split('>');
                                //MessageBox.Show(tam[1]);
                                string[] demso = tam[1].Split('/');
                                for (int i = 0; i < demso.Count() - 1; i++)
                                {
                                    arrPBright[r].Visible = false;
                                    r++;
                                }
                                a = a.Remove(0, 2);
                                //MessageBox.Show(a);
                            }
                            
                            handDeck = a.Split('/');
                            bool havecard = false;

                            for (int i = 0; i < 13; i++)
                            {
                                if (arrPBhands[i].Image != null)
                                    havecard = true;
                            }
                            if (havecard && button2.Enabled == false)
                            {
                                for (int i = 0; i < 13; i++)
                                {
                                    arrPBhands[i].Image = null;
                                }
                            }
                            for (int i = 0; i < handDeck.Count() - 1; i++)
                            {
                                string temp = @"PNG\" + handDeck[i].Replace('.', '_') + ".png";
                                arrPBhands[i].Image = Image.FromFile(temp);
                                arrPBhands[i].SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                        }
                    }catch { anycards = false; }
                }
                if (Loaded)
                {
                    int cwin = 0,cwin1=0;
                    for(int i=0;i<13;i++)
                    {
                        if (string.Equals(deck[i], "3.1") || string.Equals(deck[i], "3.2") || string.Equals(deck[i], "3.3") || string.Equals(deck[i], "3.4"))
                            cwin++;
                        if (string.Equals(deck[i], "15.1") || string.Equals(deck[i], "15.2") || string.Equals(deck[i], "15.3") || string.Equals(deck[i], "15.4"))
                            cwin1++;
                    }
                    if (cwin == 4||cwin1==4) { MessageBox.Show(this.Text + " win!"); tcpForPlayer.SendData("~" + this.Text + " win!"); button1.Enabled = true; done = true; }
                    if (s.Contains('~'))
                    {
                        button1.Enabled = true;
                        button2.Enabled = button3.Enabled = false;
                        MessageBox.Show(s);
                        //string[] win = s.Split('~');
                        //string temp = "";
                        //for (int i = 0; i < (win.Count() - 1); i++)
                        //{
                        //    temp = temp + win[i];
                        //    button1.Enabled = true;
                        //}
                        //MessageBox.Show(temp);
                        done = true;
                        return;
                    }
                    // MessageBox.Show("win count: " + win_count.ToString());
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Connect();//
            arrPB[0] = pictureBox1; imgCurr[0] = arrPB[0].Top;
            arrPB[1] = pictureBox2; imgCurr[1] = arrPB[1].Top;
            arrPB[2] = pictureBox3; imgCurr[2] = arrPB[2].Top;
            arrPB[3] = pictureBox4; imgCurr[3] = arrPB[3].Top;
            arrPB[4] = pictureBox5; imgCurr[4] = arrPB[4].Top;
            arrPB[5] = pictureBox6; imgCurr[5] = arrPB[5].Top;
            arrPB[6] = pictureBox7; imgCurr[6] = arrPB[6].Top;
            arrPB[7] = pictureBox8; imgCurr[7] = arrPB[7].Top;
            arrPB[8] = pictureBox9; imgCurr[8] = arrPB[8].Top;
            arrPB[9] = pictureBox10; imgCurr[9] = arrPB[9].Top;
            arrPB[10] = pictureBox11; imgCurr[10] = arrPB[10].Top;
            arrPB[11] = pictureBox12; imgCurr[11] = arrPB[11].Top;
            arrPB[12] = pictureBox13; imgCurr[12] = arrPB[12].Top;
            arrPBhands[0] = pictureBox14;
            arrPBhands[1] = pictureBox15;
            arrPBhands[2] = pictureBox16;
            arrPBhands[3] = pictureBox17;
            arrPBhands[4] = pictureBox18;
            arrPBhands[5] = pictureBox19;
            arrPBhands[6] = pictureBox20;
            arrPBhands[7] = pictureBox21;
            arrPBhands[8] = pictureBox22;
            arrPBhands[9] = pictureBox23;
            arrPBhands[10] = pictureBox24;
            arrPBhands[11] = pictureBox25;
            arrPBhands[12] = pictureBox26;
            button1.Enabled = false;

            tcpForPlayer.SendData("play");
            Thread t = new Thread(Receive);
            t.Start();
            if (done) { t.Abort(); done = false; }
        }
        void Send()
        {
            tcpForPlayer.SendData("~" + this.Text + " win!");
            tcpForPlayer.SendData("~" + this.Text + " win!");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            arrPB[0] = pictureBox1;
            arrPB[1] = pictureBox2;
            arrPB[2] = pictureBox3;
            arrPB[3] = pictureBox4;
            arrPB[4] = pictureBox5;
            arrPB[5] = pictureBox6;
            arrPB[6] = pictureBox7;
            arrPB[7] = pictureBox8;
            arrPB[8] = pictureBox9;
            arrPB[9] = pictureBox10;
            arrPB[10] = pictureBox11;
            arrPB[11] = pictureBox12;
            arrPB[12] = pictureBox13;
            string str = "";
            string[] hand=new string[13]; int k = 0; int x = 0;//int runortrip = 0;
            double[] inHand = new double[13]; bool check31 = false; bool checkok = true;
            string[] table = new string[13];
            double[] inTable = new double[13];
            for (int i = 0; i < 13; i++) { if (string.Equals(deck[i], "3.1")) check31 = true; }//it has 3.1
            for (int i = 0; i < 13; i++)
            {
                if (arrPB[i].Top != imgCurr[i])
                {
                    //arrPB[i].Visible = false;
                    //will have that value
                    str = str + deck[i] + ";";// and put it in str 
                    hand[k] = deck[i];//take value from pop up card put in hand[]
                    inHand[k] = double.Parse(deck[i]); //convert to double and put in inHand[]
                    k++;
                } 
            }
           //string str6 = "";
            for (int i = 0; i < handDeck.Count()-1; i++)///////
            {
                if (this.arrPBhands[i].Image != null)
                {
                    //str6 = str6 + handDeck[i] + "\n";
                    table[x] = handDeck[i];
                    inTable[x] = double.Parse(handDeck[i]);
                    x++;
                }
            }
            //MessageBox.Show(str6);
            if (k != 0) {
                for (int i = 0; i < k; i++)
            { if (check31==true && inHand[i] != 3.1) { checkok = false; } }//it has 3.1 but didnt go
                for (int i = 0; i < k; i++)
                { if (inHand[i] == 3.1) checkok = true; }//yes it has 3.1
               // MessageBox.Show(check31.ToString()+checkok.ToString());
                if (!checkok&&!hadcheck)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (arrPB[i].Top != imgCurr[i])
                        {
                            arrPB[i].Top = imgCurr[i];
                        }
                    }
                    MessageBox.Show("Ban danh loi!");
                    return;
                }

                if (!Play_Check(inHand,k))
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (arrPB[i].Top != imgCurr[i])
                        {
                           
                            arrPB[i].Top = imgCurr[i];
                        }
                    } MessageBox.Show("Ban danh loi!");
                            return;
                }
                if (x != 0)//check on table
                {
                    if (!Check_Table(inHand, inTable, k, x))
                    {
                        for (int i = 0; i < 13; i++)
                        {
                            if (arrPB[i].Top != imgCurr[i])
                            {
                                arrPB[i].Top = imgCurr[i];
                                
                            }
                        }MessageBox.Show("Ban danh loi!");
                        return;
                    }
                }
                for (int i = 0; i < 13; i++)
                    {
                    if (arrPB[i].Top != imgCurr[i])
                    {
                        arrPB[i].Top = imgCurr[i];
                        arrPB[i].Visible = false;
                       // win_count++;
                       // MessageBox.Show(win_count + "");
                    }
                    }
                }
            win_count = 0;//
            for (int i = 0; i < 13; i++)
            {
                if (!arrPB[i].Visible) win_count++;
            }
            if (win_count == 13)
            {
                MessageBox.Show(this.Text + "win!");
                for (int i = 0; i < 13; i++)
                {
                    arrPBhands[i].Visible = false;
                }
                win_count = 0;
                Send();
                button1.Enabled = true;
                done = true;
            }//
            if (string.Equals(str, "")) { MessageBox.Show("Ban chua di!"); return; }
            for (int i = 0; i < 13; i++)
            {
                arrPBhands[i].Image= null;
            }
                for (int i = 0; i < k; i++)
                {
                    string temp = @"PNG\" + hand[i].Replace('.', '_') + ".png";
                    arrPBhands[i].Image = Image.FromFile(temp);
                    arrPBhands[i].SizeMode = PictureBoxSizeMode.StretchImage;
                }
            
            //MessageBox.Show(str);
           //neu nhan danh ma k co gi thi khong lam mat button
            tcpForPlayer.SendData(str);//send to server first cause room inside it
            tcpForPlayer.SendData(str);//now u have inside, send again :) 
            button2.Enabled = false;
            button3.Enabled = false;
            button2.BackColor = Color.AliceBlue;
            button3.BackColor = Color.AliceBlue;
            hadcheck = true;
        }
        public bool Check_Table(double[] arr,double[] arrTable, int k,int x)
        {//k = so phan tu cua arr(in hand) x=so phan tu arrtable(tren field)
            int counter = 0,count_run=0,count_tuquy=0,check=0;
            if (New)
            {
                New = false;
                return true;
            }
            for(int i=0;i< x;i++)//
            {
                if (Math.Floor(arrTable[i]) == 15) counter++;
            }
            //MessageBox.Show(counter + "");
            if(counter!=0)//
            {
               // MessageBox.Show(counter + "");
                switch(counter)
                {
                    case 1:
                        {
                            //if (k == 10)//if pair run, 5 doi thong 33 44 55 66 77 
                            //{           //{
                            //    for (int i = 0; i < k; i = i + 2)
                            //    {
                            //        if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) check++;//except 33 44 66 77
                            //    }
                            //    if (k == 10 && check == 5)//if pair run 33(1) 44(1) 66(1)
                            //    {
                            //        for (int i = 0; i < k; i = i + 2)//33 44 55 66
                            //        {
                            //            if (Math.Floor(arr[i]) + 1 == Math.Floor(arr[i + 2]) && Math.Floor(arr[i + 1]) != 15)
                            //            {
                            //                count_run++;
                            //                //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                            //            }
                            //        }
                            //    }
                            //    if (count_run == 4) return true;
                            //}
                            if (k == 8)//if pair run, 4 doi thong chat 1 heo
                            {        //{
                                for (int i = 0; i < k; i = i + 2)
                                {
                                    if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) check++;//except 33 44 66 77
                                }
                                if (k == 8 && check == 4)//if pair run 33(1) 44(1) 66(1)
                                {
                                    for (int i = 0; i < k; i = i + 2)//33 44 55 66
                                    {
                                        if (Math.Floor(arr[i]) + 1 == Math.Floor(arr[i + 2]) && Math.Floor(arr[i + 1]) != 15)
                                        {
                                            count_run++;
                                            //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                                        }
                                    }
                                }
                                if (count_run == 3) return true;
                            }

                            if (k == 6)//if pair run, 3 doi thong chat dc 1 heo + 3 thong nho hon
                                       //{
                                for (int i = 0; i < k; i = i + 2)
                                {
                                    if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) check++;//except 33 45 66
                                }
                            if (k == 6 && check == 3)//if pair run 33(1) 44(1) 66(1)
                            {
                                for (int i = 0; i < k; i = i + 2)//33 44 55
                                {
                                    if (Math.Floor(arr[i]) + 1 == Math.Floor(arr[i + 2]) && Math.Floor(arr[i + 1]) != 15)
                                    {
                                        count_run++;
                                        //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                                    }
                                }
                            }
                            if (count_run == 2) return true;
                            for (int i = 0; i < k-1; i++) //4444
                            {
                                if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1]) && Math.Floor(arr[i + 1]) != 15)
                                {
                                    count_tuquy++;
                                }
                            }
                            if (count_tuquy == 3) return true;

                            break;
                        }
                    case 2:
                        {
                            //if (k == 10)//if pair run, 5 doi thong 33 44 55 66 77 
                            //{           //{
                            //    for (int i = 0; i < k; i = i + 2)
                            //    {
                            //        if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) check++;//except 33 44 66 77
                            //    }
                            //    if (k == 10 && check == 5)//if pair run 33(1) 44(1) 66(1)
                            //    {
                            //        for (int i = 0; i < k; i = i + 2)//33 44 55 66
                            //        {
                            //            if (Math.Floor(arr[i]) + 1 == Math.Floor(arr[i + 2]) && Math.Floor(arr[i + 1]) != 15)
                            //            {
                            //                count_run++;
                            //                //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                            //            }
                            //        }
                            //    }
                            //    if (count_run == 4) return true;
                            //}
                            if (k == 8)//if pair run, 4 doi thong chat 2 heo
                                       //{
                                for (int i = 0; i < k; i = i + 2)
                                {
                                    if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) check++;//except 33 44 66 77
                                }
                            if (k == 8 && check == 4)//if pair run 33(1) 44(1) 66(1)
                            {
                                for (int i = 0; i < k; i = i + 2)//33 44 55 66
                                {
                                    if (Math.Floor(arr[i]) + 1 == Math.Floor(arr[i + 2]) && Math.Floor(arr[i + 1]) != 15)
                                    {
                                        count_run++;
                                        //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                                    }
                                }
                            }
                            if(count_run==3) return true;
                            for (int i = 0; i < k-1; i++) //4444
                            {
                                if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1]) && Math.Floor(arr[i + 1]) != 15)
                                {
                                    count_tuquy++;
                                }
                            }
                            if (count_tuquy == 3) return true;
                            break;
                        }//
                    case 3:
                        {
                            //if (k == 10)//if pair run, 5 doi thong 33 44 55 66 77 
                            //{           //{
                            //    for (int i = 0; i < k; i = i + 2)
                            //    {
                            //        if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) check++;//except 33 44 66 77
                            //    }
                            //    if (k == 10 && check == 5)//if pair run 33(1) 44(1) 66(1)
                            //    {
                            //        for (int i = 0; i < k; i = i + 2)//33 44 55 66
                            //        {
                            //            if (Math.Floor(arr[i]) + 1 == Math.Floor(arr[i + 2]) && Math.Floor(arr[i + 1]) != 15)
                            //            {
                            //                count_run++;
                            //                //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                            //            }
                            //        }
                            //    }
                            //    if (count_run == 4) return true;
                            //}
                            break;
                        }
                }
            }
            if (k==x)
            {              
                Boolean triple = true;
                Boolean run = true;
                Boolean thong = true;
                for (int i = 0; i < x - 1; i++)//ktra tu quy nho hon
                {
                    if (Math.Floor(arrTable[i]) != Math.Floor(arrTable[i + 1])) // != 3333
                    {
                        triple = false;
                    }
                }
                //check run
                for (int i = 0; i < x - 1; i++)//ktra sanh nho hon
                {
                    if (Math.Floor(arrTable[i]) + 1 != Math.Floor(arrTable[i + 1]) || Math.Floor(arrTable[i + 1]) == 15)
                    {
                        run = false;//!=3456
                    }
                }
                for(int i=0;i< x;i=i+2)//? 33 44 55<77 88 99 //!33 45 55 //
                {
                    if ((Math.Floor(arrTable[i]) != Math.Floor(arrTable[i + 1]))&&(Math.Floor(arrTable[i]) + 1 != Math.Floor(arrTable[i + 2])) || Math.Floor(arrTable[i + 1]) == 15)
                    {
                        thong = false;
                    }
                }
                if (arr[k - 1] > arrTable[x - 1] && run && runortrip == 2) { runortrip = 0;  return false; }
                if (arr[k - 1] > arrTable[x - 1] && triple && runortrip == 1) { runortrip = 0; return false; }
                if (arr[k - 1] > arrTable[x - 1]) { /*MessageBox.Show(run + " " + triple +" ");*/ return (run || triple||thong) & true; }
                //this have also check higher than 15.1,...
            }//
            //th tu quy chat 3 doi thong
            if (x == 6)//if pair run, 3 doi thong 33 44 55 tren table
            {      //{
                for (int i = 0; i < x; i = i + 2)
                {
                    if (Math.Floor(arrTable[i]) == Math.Floor(arrTable[i + 1])) check++;//except 33 45 66
                }
                if (x == 6 && check == 3)//if pair run 33(1) 44(1) 66(1)
                {
                    for (int i = 0; i < x; i = i + 2)//33 44 55
                    {
                        if (Math.Floor(arrTable[i]) + 1 == Math.Floor(arrTable[i + 2]) && Math.Floor(arrTable[i + 1]) != 15)
                        {
                            count_run++;
                            //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                        }
                    }
                }
                int count_r = 0;
                for (int i = 0; i < k - 1; i++)// tu quy tren tay
                {
                    if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) count_r++;//3 3 3 3
                }
                if (count_r == k - 1 && count_run == 2) { return true; }
            }
            int check1 = 0; int count_run1 = 0; int count_r1 = 0;
            //th 4 thong chat dc 3 thong + /*chat dc tu quy*/
            if (k == 8)//if pair run, 4 doi thong 33 44 55 66 //4 doi thong tren tay
            {      //{
                for (int i = 0; i < k; i = i + 2)
                {
                    if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) check1++;//except 33 45 66
                }
                if (k == 8 && check1 == 4)//if pair run 33(1) 44(1) 66(1)
                {
                    for (int i = 0; i < k; i = i + 2)//33 44 55
                    {
                        if (Math.Floor(arr[i]) + 1 == Math.Floor(arr[i + 2]) && Math.Floor(arr[i + 1]) != 15)
                        {
                            count_run1++;
                            //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                        }
                    }
                }
                //
                int count_r = 0;
                for (int i = 0; i < x - 1; i++)// tu quy tren table
                {
                    if (Math.Floor(arrTable[i]) == Math.Floor(arrTable[i + 1])) count_r++;//3 3 3 3
                }
                if (count_r == 3 && count_run1 == 3) { return true; }
                //
                int check2 = 0;
                if(x==6)//3 doi thong tren table
                {
                    for (int i = 0; i < x; i = i + 2)
                    {
                        if (Math.Floor(arrTable[i]) == Math.Floor(arrTable[i + 1])) check2++;//except 33 45 66
                    }
                    if (x == 6 && check2 == 3)//if pair run 33(1) 44(1) 66(1)
                    {
                        for (int i = 0; i < x; i = i + 2)//33 44 55
                        {
                            if (Math.Floor(arrTable[i]) + 1 == Math.Floor(arrTable[i + 2]) && Math.Floor(arrTable[i + 1]) != 15)
                            {
                                count_r1++;
                                //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                            }
                        }
                    }
                }
            }
            if (count_run1 == 3 && count_r1 == 2) return true;
            return false;
        }
       
        public bool Play_Check(double[] arr, int k)//checking 4 5 6, 33, 3 , 333,3333
        {
            int dem = 0; int check = 0; int count_r = 0;
            switch (k)
            {
                case 1: //single 
                    return true;
                case 2: //double 
                    return Math.Floor(arr[0]) == Math.Floor(arr[1]);
                case 3: //triple or 3 run
                    //check triple
                    Boolean triple = true;
                    Boolean run = true;
                    for (int i = 0; i < k - 1; i++)
                    {
                        if (Math.Floor(arr[i]) != Math.Floor(arr[i + 1]))
                        {
                            triple = false;
                        }
                    }
                    //check run
                    for (int i = 0; i < k - 1; i++)
                    {
                        if (Math.Floor(arr[i]) + 1 != Math.Floor(arr[i + 1]) || Math.Floor(arr[i + 1]) == 15)
                        {
                            run = false;
                        }
                    }
                    if (triple) runortrip = 2;
                    if (run) runortrip = 1;
                    return triple || run;//
                default: //check run >= 4
                    {
                        //MessageBox.Show(k+"");
                        //check 33 44 55
                        for (int i = 0; i < k-1; i++)
                        {
                            if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) count_r++;//3 3 3 3
                        }
                        if (count_r == k-1) {return true; }
                        for (int i = 0; i < k; i = i + 2)
                        {
                            if (Math.Floor(arr[i]) == Math.Floor(arr[i + 1])) check++;//except 33 45 66
                        }
                        if (k >= 6 && k % 2 == 0 && check == k / 2)//if pair run
                        {

                            for (int i = 0; i < k; i = i + 2)
                            {
                                if (Math.Floor(arr[i]) + 1 == Math.Floor(arr[i + 2]) && Math.Floor(arr[i + 1]) != 15)
                                {
                                    dem++;
                                    //must be 33 44 55, 33 44 55 66, 33 44 55 66 77,...
                                }
                            }
                            if (dem == (k / 2) - 1) return true;
                        }
                        for (int i = 0; i < k - 1; i++)
                        {
                            if (Math.Floor(arr[i]) + 1 != Math.Floor(arr[i + 1]) || Math.Floor(arr[i + 1]) == 15)
                            {
                                //MessageBox.Show("falset");
                                return false;
                            }
                        }
                        
                    }return true;
                    
            } 
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (imgCurr[0] == pictureBox1.Top)
            { pictureBox1.Top -= 25; }
            else pictureBox1.Top = imgCurr[0];
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (imgCurr[1] == pictureBox2.Top)
            { pictureBox2.Top -= 25; }// clicking card and popping it up right? ye

            else pictureBox2.Top = imgCurr[1];
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (imgCurr[2] == pictureBox3.Top)
                pictureBox3.Top -= 25;
            else pictureBox3.Top = imgCurr[2];
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (imgCurr[3] == pictureBox4.Top)
                pictureBox4.Top -= 25;
            else pictureBox4.Top = imgCurr[3];
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (imgCurr[4] == pictureBox5.Top)
                pictureBox5.Top -= 25;
            else pictureBox5.Top = imgCurr[4];
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (imgCurr[5] == pictureBox6.Top)
                pictureBox6.Top -= 25;
            else pictureBox6.Top = imgCurr[5];
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (imgCurr[6] == pictureBox7.Top)
                pictureBox7.Top -= 25;
            else pictureBox7.Top = imgCurr[6];
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (imgCurr[7] == pictureBox8.Top)
                pictureBox8.Top -= 25;
            else pictureBox8.Top = imgCurr[7];
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (imgCurr[8] == pictureBox9.Top)
                pictureBox9.Top -= 25;
            else pictureBox9.Top = imgCurr[8];
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (imgCurr[9] == pictureBox10.Top)
                pictureBox10.Top -= 25;
            else pictureBox10.Top = imgCurr[9];
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            if (imgCurr[10] == pictureBox11.Top)
                pictureBox11.Top -= 25;
            else pictureBox11.Top = imgCurr[10];
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            if (imgCurr[11] == pictureBox12.Top)
                pictureBox12.Top -= 25;
            else pictureBox12.Top = imgCurr[11];
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if (imgCurr[12] == pictureBox13.Top)
                pictureBox13.Top -= 25;
            else pictureBox13.Top = imgCurr[12];
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            tcpForPlayer.CloseConnection();
            tcpForOpponent.CloseConnection();
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(Environment.ExitCode);
        }

        private void button3_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(New + "");
            string[] hand = new string[13]; 
            double[] inHand = new double[13];
            for (int i = 0; i < 13; i++)
            {
                if (string.Equals(deck[i], "3.1"))//can "pass" when 3.1 on hand
                {
                    if (arrPB[i].Visible) { MessageBox.Show("Ban danh loi!"); return; }//
                }
            }
            // MessageBox.Show(New + "");
            
            if (New) return;
            New = false;
            string str = "pass";// as u can see here
            tcpForPlayer.SendData(str);
            tcpForPlayer.SendData(str);
            button2.Enabled = false;
            button3.Enabled = false;
            button3.BackColor = Color.Coral;
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

        }
    }
}
