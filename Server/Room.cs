using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Room
    {
        static int M = 52;
        QuanBai[] labai = new QuanBai[52];
        double[] s = new double[M];
        string[] CtoR;
        int count = 0;
        List<SocketModel> clients = new List<SocketModel>();
        int currentPlayer;
        int pass_count;
        bool done = false;
        public void Deal()
        {
            string str;//
            for (int i = 0; i < clients.Count; i++)
            {
                str = "";
                for (int j = 0; j < 13; j++)
                {
                    str = str + s[13 * i + j] + ";";//each client will have 13 cards
                }
                clients[i].SendData(str);
            }//
            /*
            clients[0].SendData("4.1;4.2;6.2;7.1;7.2;7.3;7.4;8.2;8.1;9.2;9.3;10.1;11.2");
             clients[1].SendData("11.4;9.4;10.3;11.3;12.3;12.1;12.2;13.1;13.4;14.3;14.1;15.4;14.4;");
             clients[2].SendData("3.1;5.4;6.1;8.4;10.4;11.1;12.4;13.2;13.3;14.2;15.2;15.1;15.3");
             clients[3].SendData("3.2;3.3;3.4;4.3;4.4;5.2;5.2;5.3;6.3;6.4;9.1;10.2;8.3");
             */
        }
        public void Play()
        {
            string str = ""; string str1 = "OK";
            bool yes = true;
            while (true)
                {
                    Console.WriteLine("Player " + currentPlayer + " turn");
                    clients[currentPlayer].SendData(str1);//here
                if (pass_count == 3) { Console.WriteLine("new"); /*pass_count = 0;*/  clients[currentPlayer].SendData("new"); }
                    str = clients[currentPlayer].ReceiveData();
                if (str.Contains('~'))
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (i != currentPlayer)
                        {
                            clients[i].SendData(str); //error appear
                        }
                    }
                    Console.WriteLine(str);
                    done = true;
                    return;
                }
                try { CtoR = str.Split(';'); } catch { yes = false; }
                if (yes)
                    {
                        string temp = "";
                        for (int i = 0; i < CtoR.Count() - 1; i++)
                        {
                            temp = temp + CtoR[i] + '/';
                        }
                    //for (int i = 0; i < clients.Count; i++)
                    //{
                    //    if (i != currentPlayer)
                    //    { 
                    string temp1 = "";string temp2 = "";string temp3 = "";
                    if (!string.Equals(temp, ""))//tý qua xem bên m có cái này chưa
                    {
                        switch (currentPlayer)
                        {
                            case 0:
                                {
                                    temp1 = "r>" + temp;
                                    clients[1].SendData(temp1);
                                    temp2 = "m>" + temp;
                                    clients[2].SendData(temp2);
                                    temp3 = "l>" + temp;
                                    clients[3].SendData(temp3);
                                    break;
                                }
                            case 1:
                                {
                                    temp1 = "l>" + temp;
                                    clients[0].SendData(temp1);
                                    temp2 = "r>" + temp;
                                    clients[2].SendData(temp2);
                                    temp3 = "m>" + temp;
                                    clients[3].SendData(temp3);
                                    break;
                                }
                            case 2:
                                {
                                    temp1 = "m>" + temp;
                                    clients[0].SendData(temp1);
                                    temp2 = "l>" + temp;
                                    clients[1].SendData(temp2);
                                    temp3 = "r>" + temp;
                                    clients[3].SendData(temp3);
                                    break;
                                }
                            case 3:
                                {
                                    temp1 = "r>" + temp;
                                    clients[0].SendData(temp1);
                                    temp2 = "m>" + temp;
                                    clients[1].SendData(temp2);
                                    temp3 = "l>" + temp;
                                    clients[2].SendData(temp3);
                                    break;
                                }
                        }
                    }//right,mid,left
                            //if (currentPlayer == 1) { temp = "2>3>0>" + temp; }
                            //if (currentPlayer == 2) { temp = "3>0>1" + temp; }
                            //if (currentPlayer == 3) { temp = "0>1>2" + temp; }
                                    //clients[i].SendData(temp); //error appear
                            //    }
                            //}
                    }
                    if (currentPlayer == 3) currentPlayer = -1;
                    if (string.Equals(str, "pass")) //whats that? pass signal
                    { currentPlayer += 0; pass_count++;}
                    if (!string.Equals(str, "pass")) { pass_count = 0; }
              
                Console.WriteLine(pass_count);
                    currentPlayer++;
                } 
        }
        public int CheckFirst()
        {
            for (int i = 0; i < 52; i++)
            {
                if (s[i] == 3.1)
                    return i;
            }
            return -1;
        }
        public void Shuffle()
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
        public bool Check()
        {
            if (count == 4) return true;
            return false;
        }
        public void Add(SocketModel a)
        {
            clients.Add(a); count++;
            if (Check())
            {
                Shuffle();//
                currentPlayer = CheckFirst() / 13;//2;////init curr player that have 3.1
                Deal();//
                Thread l = new Thread(Play);//play turn
                 l.Start();
                if (done) { l.Abort(); count = 0; clients.Clear(); }
            }
        }
    }
}
